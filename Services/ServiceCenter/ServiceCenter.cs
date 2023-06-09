using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShareInstances.Services.Center;
public class ServiceCenter : ICenter
{
    public bool IsCenterActive { get; set; } = false;

    private static Dictionary<ReadOnlyMemory<char>, IGhost> serviceRegister = new();

    #region App services
    private SupportGhost supporterService = new();
    private FactoryGhost factoryService = new();
    private PlayerGhost playerService = new();
    #endregion
    #region UI services
    private UIControlHolder<object> controlHolder = new ();
    private ViewModelHolder<object> holder = new();
    #endregion

    //I dont know what here happens,
    //but please dont remove constructor params
    public ServiceCenter()
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
        
        IsCenterActive = true;
    }



    //accomodate ghost from entire scope 
    private void LodgeGhost(IGhost ghost) =>
        serviceRegister.Add(ghost.GhostName, ghost);

    //accomodate ghost from external scope
    public void RegistGhost(ref IGhost ghost) =>
        serviceRegister.Add(ghost.GhostName, ghost);

    public void UpdateGhost(ref IGhost ghost) =>
        serviceRegister[ghost.GhostName] = ghost;
    
    public IGhost GetGhost(ReadOnlyMemory<char> name)
    {
        if (serviceRegister.Keys.ToList().Contains(name))
            return serviceRegister[name];
        return null;
    }

    public IList<ReadOnlyMemory<char>> GetGhosts() =>
        serviceRegister.ToList().Select(x => x.Value.GhostName).ToList();
    

    public void ResolveSupporter(ref ISynchArea synchArea)
    {
        var supporter = (SupportGhost)GetGhost(((IGhost)supporterService).GhostName);
        var factory = (FactoryGhost)GetGhost(((IGhost)factoryService).GhostName);

        supporter.Init(ref synchArea);
        factory.Init(ref supporter);

        //UpdateGhost(ref (IGhost)supporter);
        //UpdateGhost(ref (IGhost)factory);
    }

    public void ResolvePlayer(IPlayer _player)
    {
        var player = (PlayerGhost)GetGhost(((IGhost)playerService).GhostName);
        player.Init(ref _player);
    } 
}
