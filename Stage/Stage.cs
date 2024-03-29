using ShareInstances.Services.Castle;
using ShareInstances.Configure;

using System;
using System.Threading.Tasks;
namespace ShareInstances.Stage;

public sealed class Stage 
{
    #region Castle
    private Castle castle = new();
    #endregion

    #region Configuration Region
    public IConfigure Configure {get; set;}
    #endregion

    #region Default Waiters
    //public static Filer Filer {get; private set;}
    #endregion
    
    #region Current Components
    public IPlayer PlayerInstance => castle.ResolvePluginBag().GetCurrentPlayer() ?? null;
    public ICube CubeInstance => castle.ResolvePluginBag().GetCurrentCube() ?? null;
    #endregion
        
    #region Tagging result
    public bool CompletionResult {get; private set;}

    public event Action OnInitialized;
    public event Action OnComponentMuted;
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
            castle.Pack();
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
                    await castle.RegisterPlayers(docker.Players);
                    await castle.RegisterCubes(docker.Cubes);
                }
                else if(dock == -1)
                {
                    throw new Exception("Could not load all defined components");
                }
            }

            isCompleted = true;
        }
        catch(Exception ex)
        {
            throw ex;
        }
       
        return isCompleted;
    }

    #region Current instance Switching
    public void SwitchPlayer(int index)
    {
        castle.SwitchPlayer(index);
    }

    public void SwitchCube(int index)
    {
        castle.SwitchCube(index);
    }
    #endregion

    #region Clear
    public void Clear()
    {
    }
    #endregion    

}
