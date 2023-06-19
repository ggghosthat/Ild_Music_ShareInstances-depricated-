using ShareInstances.Services.Castle;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Configure;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShareInstances.Stage;
public record struct DumpStructure(string Name, string Path, string Type);

public class Stage 
{
    #region Configure Region
    public IConfigure Configure {get; set;}
    #endregion

    #region Filer
    public static Filer Filer {get; private set;}
    #endregion

    #region Player Region
    private IList<IPlayer> _players = new List<IPlayer>();
    public IList<IPlayer> Players => _players;

    private IPlayer _playerInstance;
    public IPlayer PlayerInstance => _playerInstance;
    #endregion

    #region Synch Region
    private IList<ICube> _areas = new List<ICube> ();
    public IList<ICube> Areas => _areas;

    private ICube _areaInstance;
    public ICube AreaInstace => _areaInstance;
    #endregion
    
    #region Paths
    public List<string> PlayerPaths { get; set; }
    public List<string> SynchPaths { get; set; }

    public string DumpPath { get; set; } = Environment.CurrentDirectory + "\\components.json";
    #endregion
    
    #region Event
    public event Action OnInitialized;
    public event Action OnComponentMuted;
    #endregion

    #region Service Supply
    public Castle castle = new();
    #endregion

    #region Store Supply
    #endregion

    #region Properties
    public bool CompletionResult {get; private set;}
    #endregion

    #region Constructors
    public Stage(){}
    
    public Stage(IConfigure configure)
    {
        Configure = configure;
        Task.Factory.StartNew(async () => await ObserveLoading());
    }
    #endregion


    #region Inits
    public async Task ObserveLoading(string playerAssembly, string synchAssembly)
    {
        CompletionResult = await InitAsync(playerAssembly, synchAssembly);
        OnInitialized?.Invoke();
    }

    public async Task ObserveLoading()
    {
        CompletionResult = await InitAsync(Configure.ConfigSheet.Players, Configure.ConfigSheet.Synches);
        OnInitialized?.Invoke();
    }
       
    public async Task<bool> InitAsync(string playerAssembly, string synchAssembly)
    {
        bool isCompleted = false;
        try
        {
            AssemblyProcess(playerAssembly, PlayerInstance);
            AssemblyProcess(synchAssembly, AreaInstace);

            castle.ResolveSupporter(AreaInstace);
            castle.ResolvePlayer(PlayerInstance);

            Filer = (Filer)castle.GetWaiter("Filer".AsMemory());
           
            isCompleted = true;
        }
        catch(Exception ex)
        {
            isCompleted = false;
        }
        
        return isCompleted;
    }

    public async Task<bool> InitAsync(IEnumerable<string> playerAssembly, IEnumerable<string> synchAssembly)
    {
        bool isCompleted = false;
        try
        {
            AssemblyProcess(playerAssembly, PlayerInstance);
            AssemblyProcess(synchAssembly, AreaInstace);

            castle.ResolveSupporter(AreaInstace);
            castle.ResolvePlayer(PlayerInstance);

            Filer = (Filer)castle.GetWaiter("Filer".AsMemory());

            isCompleted = true;
        }
        catch(Exception ex)
        {
            isCompleted = false;
        }
        
        return isCompleted;
    }

    private void InitUnit(string path, string type)
    {
        if (type == "player")
        {
            AssemblyProcess(path, PlayerInstance);
            _playerInstance = _players[0];
        }
        if (type == "syncharea")
        {
            AssemblyProcess(path, AreaInstace);
            _areaInstance = _areas[0];
        }
    }   
    #endregion    
    
    #region AssemblySearchingMethods
    private void AssemblyProcess<T>(string assemblyPath, T assemblyType)
    {
        try
        {
            IEnumerable<string> dlls = FindDlls(assemblyPath);
            (Type, IEnumerable<T>) result = FindSpecialTypes<T>(ref dlls);

            if (typeof(IPlayer).IsAssignableFrom(result.Item1))
            {
                result.Item2.ToList()
                            .ForEach(player => _players.Add((IPlayer)player));
                
                _playerInstance = _players[0];                            
            }
            else if (typeof(ICube).IsAssignableFrom(result.Item1))
            {
                result.Item2.ToList()
                            .ForEach(area => _areas.Add((ICube)area));
                _areaInstance = _areas[0];
            }
        }
        catch
        {
            throw;
        }
    }

    private void AssemblyProcess<T>(IEnumerable<string> assembliesPaths, T assemblyType)
    {
        try
        {
            (Type, IEnumerable<T>) result = FindSpecialTypes<T>(ref assembliesPaths);

            if (typeof(IPlayer).IsAssignableFrom(result.Item1))
            {
                result.Item2.ToList()
                            .ForEach(player => _players.Add((IPlayer)player));
                _playerInstance = _players[0];
            }

            else if (typeof(ICube).IsAssignableFrom(result.Item1))
            {
                result.Item2.ToList()
                            .ForEach(area => _areas.Add((ICube)area));
                _areaInstance = _areas[0];
            }            
        }
        catch
        {
            throw;
        }
    }

    private IEnumerable<string> FindDlls(string path) 
    {
        if (path.EndsWith(".dll"))
            return new List<string>(){path};
        return Directory.EnumerateFileSystemEntries(path, "*.dll");
    }
    
    private (Type,IEnumerable<T>) FindSpecialTypes<T>(ref IEnumerable<string> dllsPath)
    {
        var list = new List<T>();
        foreach (string path in dllsPath)
        {
            var assembly = Assembly.LoadFrom(path);
            var exportedTypes = assembly.ExportedTypes;
            exportedTypes.Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(T)))
                         .Select(t => t)
                         .ToList()
                         .ForEach(t => 
                         {
                            var instance = (T)Activator.CreateInstance(t);
                            if (!IsDumpIgnore)
                            {
                                if (instance is IPlayer player) 
                                    dumps.Add(new DumpStructure(player.PlayerName.ToString(), path.Replace("\\","/"), "player"));
                                if (instance is ICube area)
                                    dumps.Add(new DumpStructure(area.CubeName.ToString(), path.Replace("\\","/"), "syncharea"));
                            }
                            list.Add(instance);
                         });
            dllsPath.ToList().Remove(path);
        }
        return (typeof(T), list);
    }
    #endregion
    
    #region Clear
    public void Clear()
    {
        _players.Clear();
        _areas.Clear();
        dumps.Clear();
    }
    #endregion    

    #region Service Center methods
    public IGhost GetGhostInstance(Memory<char> name)
    {
        return castle.GetGhost(name);
    }
    #endregion

    #region Mutabillity
    public void ChangeComponent(IShare component)
    {
        if (component is IPlayer playerInstance)
            _playerInstance = playerInstance;
        else if(component is ICube areaInstance)
            _areaInstance = areaInstance;
        OnComponentMuted?.Invoke();
    }

    public async Task ChangeComponentAsync(IShare component) =>
        await new Task( () =>
        {
            if (component is IPlayer playerInstance)
                _playerInstance = playerInstance;
            else if(component is ICube areaInstance)
                _areaInstance = areaInstance;
            OnComponentMuted?.Invoke();
        }); 
    #endregion  
}
