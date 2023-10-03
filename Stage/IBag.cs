using ShareInstances.Instances;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace ShareInstances.Stage;
public interface IPluginBag 
{
    public IPlayer CurrentPlayer {get;}
    public ICube CurrentCube {get;}

    public void AddPlayerPlugin(IPlayer newPlayer);
    public void AddCubePlugin(ICube newCube);

    public Task AddPlayerPluginsAsync(IEnumerable<IPlayer> players);
    public Task AddCubePluginsAsync(IEnumerable<ICube> cubes);

    public void SetCurrentPlayer(Guid newPlayerId);
    public void SetCurrentCube(Guid newCubeId);

    public void DeletePlayer(Guid playerId);
    public void DeleteCube(Guid cubeId);

    public void ClearPlayers();
    public void ClearCubes();
}
