using ShareInstances.Services.Castle;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Configure;

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using Autofac;
namespace ShareInstances.Stage;

public class Stage 
{
    ContainerBuilder builder = new ();

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
    
    public Stage(ref IConfigure configure)
    {
        Configure = configure;
        ObserveLoading().Wait();
    }
    #endregion


    public async Task ObserveLoading()
    {
        CompletionResult = await InitAsync();
        OnInitialized?.Invoke();
    }       

    public async Task<bool> InitAsync()
    {
        bool isCompleted = false;
        try
        {
            using (var docker = new Docker(Configure))
            {
                var dock = docker.Dock();
                dock.Wait();

                if(dock.IsCompleted)
                {
                    _players = docker.Players;
                    _areas = docker.Cubes;

                    _playerInstance = _players.FirstOrDefault();
                    _areaInstance = _areas.FirstOrDefault();
                }
            }

            //init filer
            //Filer = (Filer)castle.GetWaiter("Filer".AsMemory());

            isCompleted = true;
        }
        catch(Exception ex)
        {
            isCompleted = false;
        }
        
        return isCompleted;
    }
    #region Clear
    public void Clear()
    {
        _players.Clear();
        _areas.Clear();
    }
    #endregion    

}
