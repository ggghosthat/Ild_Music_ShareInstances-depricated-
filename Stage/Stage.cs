using ShareInstances.Services.Castle;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Configure;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
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
        
    #region Event
    public event Action OnInitialized;
    public event Action OnComponentMuted;
    #endregion

    #region Castle
    public Castle castle = new();
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


    #region Init methods
    public async Task ObserveLoading()
    {
        CompletionResult = await InitAsync(Configure.ConfigSheet.Players, Configure.ConfigSheet.Synches);
        OnInitialized?.Invoke();
    }       

    public async Task<bool> InitAsync(IEnumerable<Memory<char>> playerAssembly,
                                      IEnumerable<Memory<char>> synchAssembly)
    {
        bool isCompleted = false;
        try
        {
            AssemblyProcess(ref playerAssembly, PlayerInstance);
            AssemblyProcess(ref synchAssembly, AreaInstace);

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
    #endregion    
    
    #region Assembly loading methods
    private void AssemblyProcess<T>(ref IEnumerable<Memory<char>> assembliesPaths,
                                    T assemblyType)
    {
        (Type, IEnumerable<T>) components = FindComponents<T>(ref assembliesPaths);

        if (typeof(IPlayer).IsAssignableFrom(components.Item1))
        {
            components.Item2.ToList()
                        .ForEach(player => _players.Add((IPlayer)player));
            _playerInstance = _players[0];
        }
        else if (typeof(ICube).IsAssignableFrom(components.Item1))
        {
            components.Item2.ToList()
                        .ForEach(area => _areas.Add((ICube)area));
            _areaInstance = _areas[0];
        }
    }

    private (Type,IEnumerable<T>) FindComponents<T>(ref IEnumerable<Memory<char>> dllsPath)
    {
        var list = new List<T>();
        foreach (Memory<char> path in dllsPath)
        {
            if(File.Exists(path.ToString()))
            {
                var assembly = Assembly.LoadFrom(path.ToString());
                var exportedTypes = assembly.ExportedTypes;
                exportedTypes.Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(T)))
                            .Select(t => t)
                            .ToList()
                            .ForEach(t => 
                            {
                                T instance = (T)Activator.CreateInstance(t);
                                list.Add(instance);
                            });
                dllsPath.ToList().Remove(path);
            }
        }
        return (typeof(T), list);
    }
    #endregion
    
    #region Clear
    public void Clear()
    {
        _players.Clear();
        _areas.Clear();
    }
    #endregion    

    #region Service Center methods
    public IGhost GetGhostInstance(Memory<char> name)
    {
        return castle.GetGhost(name);
    }
    #endregion
}