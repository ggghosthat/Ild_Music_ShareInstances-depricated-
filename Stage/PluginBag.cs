using ShareInstances.Instances;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
namespace ShareInstances.Stage;
public class PluginBag : IPluginBag
{
    private readonly IMediator _mediator;

    private IDictionary<Guid, IPlayer> playerPlugins = new Dictionary<Guid, IPlayer>();
    private IDictionary<Guid, ICube> cubePlugins = new Dictionary<Guid, ICube>();

    private IPlayer currentPlayer = null;
    private ICube currentCube = null;

    public IPlayer CurrentPlayer => currentPlayer;
    public ICube CurrentCube => currentCube;

    public int PlayersCount => playerPlugins.Count;
    public int CubesCount => cubePlugins.Count;

    

    public PluginBag(IMediator mediator)
    {
        _mediator = mediator;
    }


    public void AddPlayerPlugin(IPlayer newPlayer)
    {

        if(!playerPlugins.ContainsKey(newPlayer.PlayerId))
        {
            newPlayer.ConnectMediator(_mediator);
            playerPlugins.Add(newPlayer.PlayerId, newPlayer);
        }

        if(playerPlugins.Values.Count > 0)
            currentPlayer = playerPlugins.Values.First();
    }

    public void AddCubePlugin(ICube newCube)
    {
        if(!cubePlugins.ContainsKey(newCube.CubeId))
        {
           newCube.ConnectMediator(_mediator);
           cubePlugins.Add(newCube.CubeId, newCube);
        }

        if(cubePlugins.Values.Count > 0)
            currentCube = cubePlugins.Values.First();

    }

    public async Task AddPlayerPluginsAsync(IEnumerable<IPlayer> players)
    {
        foreach(IPlayer player in players)
        {
            if(!playerPlugins.ContainsKey(player.PlayerId))
            {
                player.ConnectMediator(_mediator);
                playerPlugins.Add(player.PlayerId, player);
            }
        }

        if(playerPlugins.Values.Count > 0)
            currentPlayer = playerPlugins.Values.First();
    }
    
    public async Task AddCubePluginsAsync(IEnumerable<ICube> cubes)
    {
        foreach(ICube cube in cubes)
        {
            if(!cubePlugins.ContainsKey(cube.CubeId))
            {
                cube.ConnectMediator(_mediator);
                cubePlugins.Add(cube.CubeId, cube);
            }
        }

        if(playerPlugins.Values.Count > 0)
            currentPlayer = playerPlugins.Values.First();
    }

    public IEnumerable<IPlayer> GetPlayers()
    {
        return playerPlugins.Values;
    }

    public IEnumerable<ICube> GetCubes()
    {
        return cubePlugins.Values;
    }

    public void SetCurrentPlayer(Guid newPlayerId)
    {
        if(playerPlugins.ContainsKey(newPlayerId))
            currentPlayer = playerPlugins[newPlayerId];
    }

    public void SetCurrentCube(Guid newCubeId)
    {
        if(cubePlugins.ContainsKey(newCubeId))
            currentCube = cubePlugins[newCubeId];
    }


    public void DeletePlayer(Guid playerId)
    {
        if(playerPlugins.ContainsKey(playerId))
            playerPlugins.Remove(playerId);
    }

    public void DeleteCube(Guid cubeId)
    {
        if(cubePlugins.ContainsKey(cubeId))
            cubePlugins.Remove(cubeId);
    }


    public void ClearPlayers()
    {
        playerPlugins.Clear();        
    }

    public void ClearCubes()
    {
        cubePlugins.Clear();
    }
}
