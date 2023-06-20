using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShareInstances.Services.Castle;
public class Castle : ICastle
{
    public bool IsCenterActive { get; set; } = false;

    private static Dictionary<ReadOnlyMemory<char>, IGhost> ghostRegister = new();
    private static Dictionary<ReadOnlyMemory<char>, IWaiter> waiterRegister = new();

    #region Constant ghosts
    private SupportGhost supporterService = new();
    private FactoryGhost factoryService = new();
    private PlayerGhost playerService = new();
    private UIControlHolder<object> controlHolder = new ();
    private ViewModelHolder<object> holder = new();
    #endregion

    #region Constant waiters
    private Filer filer;
    #endregion

    //I dont know what here happens,
    //but please dont remove constructor params
    public Castle()
    {
        OnCenterRegisterActivate();
    }

    public void OnCenterRegisterActivate()
    {
        LodgeGhost((IGhost)supporterService);
        LodgeGhost((IGhost)playerService);
        LodgeGhost((IGhost)factoryService);

        LodgeGhost((IGhost)controlHolder);
        LodgeGhost((IGhost)holder);
        
        var factory = (FactoryGhost)ghostRegister["FactoryGhost".AsMemory()];
        filer = new Filer(ref factory);
        RegistWaiter(filer);

        IsCenterActive = true;
    }


    #region Accomodation & recieving region
    //accomodate ghost from entire scope 
    private void LodgeGhost(IGhost ghost) =>
        ghostRegister.Add(ghost.GhostName, ghost);

    //accomodate ghost from external scope
    public void RegistGhost(IGhost ghost) =>
        ghostRegister.Add(ghost.GhostName, ghost);
    
    //return ghost by its own name
    public IGhost GetGhost(ReadOnlyMemory<char> name)
    {
        if (ghostRegister.Keys.ToList().Contains(name))
            return ghostRegister[name];
        return null;
    }

    //accomodating waiter inside the castle
    public void RegistWaiter(IWaiter waiter) =>
        waiterRegister.Add(waiter.WaiterName, waiter);
        
    //Recover waiter instance
    public void UpdateWaiter(IWaiter waiter) =>
        waiterRegister[waiter.WaiterName] = waiter;

    //return waiter by its own name
    public IWaiter GetWaiter(ReadOnlyMemory<char> name) 
    {
        if (waiterRegister.Keys.ToList().Contains(name))
            return waiterRegister[name];
        return null;
    }

    //return all ghost instances
    public IList<ReadOnlyMemory<char>> GetGhosts() =>
        ghostRegister.ToList().Select(x => x.Value.GhostName).ToList();
    
    //return all ghost instances
    public IList<ReadOnlyMemory<char>> GetWaiters() =>
        waiterRegister.ToList().Select(x => x.Value.WaiterName).ToList();

    #endregion

    #region resolve region
    public void ResolveSupporter(ICube syncCube)
    {
        var supporter = (SupportGhost)GetGhost(((IGhost)supporterService).GhostName);
        var factory = (FactoryGhost)GetGhost(((IGhost)factoryService).GhostName);

        supporter.Init(ref syncCube);
        factory.Init(ref supporter);

        //UpdateGhost(ref (IGhost)supporter);
        //UpdateGhost(ref (IGhost)factory);
    }

    public void ResolvePlayer(IPlayer _player)
    {
        var player = (PlayerGhost)GetGhost(((IGhost)playerService).GhostName);
        player.Init(ref _player);
    } 
    #endregion
}
