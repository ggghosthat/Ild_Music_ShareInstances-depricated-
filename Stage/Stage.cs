using ShareInstances.Services.Castle;
using ShareInstances.Services.Entities;
using ShareInstances.Configure;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace ShareInstances.Stage;

public sealed class Stage 
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
    
    public Stage(ref IConfigure configure)
    {
        Configure = configure;
    }
    #endregion


    public async Task Build()
    {
        try 
        {
            CompletionResult = await DockComponents();
            OnInitialized?.Invoke();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }       

    //TODO: this method is needed in more impored reliable solution
    private async Task<bool> DockComponents()
    {
        bool isCompleted = false;
        try
        {
            using (var docker = new Docker(Configure))
            {
                var dock = await docker.Dock();

                if(dock == 0)
                {
                    _players = docker.Players;
                    _areas = docker.Cubes;

                    _playerInstance = _players.FirstOrDefault();
                    _areaInstance = _areas.FirstOrDefault();
                }
                else if(dock == -1)
                {
                    throw new Exception("Could not load all defined components");
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
