using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Stage;
using ShareInstances.CQRS.Handlers.Delegatebag;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
namespace ShareInstances.Services.Castle;

public class Castle : ICastle
{
    public bool IsCenterActive { get; set; } = false;

    private static IContainer container;

    public Castle()
    {}

    public void OnCenterRegisterActivate()
    {
        var builder = new ContainerBuilder();
        
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(Castle).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        builder.RegisterType<DelegateBag>().SingleInstance();
        builder.RegisterMediatR(configuration);
        builder.RegisterType<PluginBag>().As<IPluginBag>().SingleInstance();
        builder.RegisterType<SupportGhost>().Keyed<IGhost>(Ghosts.SUPPORT);
        builder.RegisterType<FactoryGhost>().Keyed<IGhost>(Ghosts.FACTORY);
        builder.RegisterType<PlayerGhost>().Keyed<IGhost>(Ghosts.PLAYER);
        builder.RegisterType<Filer>().Named<IGhost>("Filer");      
        
        container = builder.Build();
        IsCenterActive = true;
    }


    #region resolve region
    //syncronous way to resolve ghost or waiter from IoC
    public IGhost ResolveGhost(Ghosts ghostTag)
    {
       return container.ResolveKeyed<IGhost>(ghostTag);
    }

    public IWaiter ResolveWaiter(ref string waiterTag)
    {
        return container.ResolveNamed<IWaiter>(waiterTag);
    }


    //asyncronous way to resolve ghost or waiter from castle IoC
    public Task<IGhost> ResolveGhostAsync(Ghosts ghostTag)
    {
       var ghost = container.ResolveKeyed<IGhost>(ghostTag);
       return Task.FromResult(ghost);
    }

    public Task<IWaiter> ResolveWaiterAsync(ref string waiterTag)
    {
        var waiter = container.ResolveNamed<IWaiter>(waiterTag);
        return Task.FromResult(waiter);
    }



    public void RegisterCube(ICube cube)
    {
        using (var cubeTransaction = container.BeginLifetimeScope())
        {
            var bag = cubeTransaction.Resolve<PluginBag>();
            bag.AddCubePlugin(cube);

            var supporter = cubeTransaction.Resolve<SupportGhost>();
            var factory = cubeTransaction.Resolve<FactoryGhost>();

            supporter.Init(ref cube);
            factory.Init(ref supporter);
        } 
    }

    public void RegisterPlayer(IPlayer player)
    {
        using (var playerTransaction = container.BeginLifetimeScope())
        {
            var bag = playerTransaction.Resolve<PluginBag>();
            bag.AddPlayerPlugin(player);

            var playerGhost = playerTransaction.Resolve<PlayerGhost>();
            playerGhost.Init(ref player);
        } 
    }

    public async Task RegisterPlayers(ICollection<IPlayer> players)
    {
        using (var playerTransaction = container.BeginLifetimeScope())
        {
            var bag = playerTransaction.Resolve<PluginBag>();
            var playersCountForBeginning = bag.PlayersCount;

            await bag.AddPlayerPluginsAsync(players);
            
            if(playersCountForBeginning == 0)
            {
                var currentPlayer = bag.CurrentPlayer;
                var playerGhost = playerTransaction.Resolve<PlayerGhost>();
                playerGhost.Init(ref currentPlayer);
            }
        } 
    }

    public async Task RegisterCubes(ICollection<ICube> cubes)
    {
        using (var cubeTransaction = container.BeginLifetimeScope())
        {
            var bag = cubeTransaction.Resolve<PluginBag>();
            var cubesCountForBeginning = bag.CubesCount;

            await bag.AddCubePluginsAsync(cubes);
            
            if(cubesCountForBeginning == 0)
            {
                var currentCube = bag.CurrentCube;
               
                var supporter = cubeTransaction.Resolve<SupportGhost>();
                var factory = cubeTransaction.Resolve<FactoryGhost>();

                supporter.Init(ref currentCube);
                factory.Init(ref supporter);
            }
        } 
    }

    public async Task<IEnumerable<IPlayer>> GetPlayersAsync()
    {
        IEnumerable<IPlayer> _players;
        using (var cubeTransaction = container.BeginLifetimeScope())
        {
            var bag = cubeTransaction.Resolve<PluginBag>();
            _players = bag.GetPlayers();    
        }

        return _players;
    }

    public async Task<IEnumerable<ICube>> GetCubesAsync()
    {
        IEnumerable<ICube> _cubes;
        using (var cubeTransaction = container.BeginLifetimeScope())
        {
            var bag = cubeTransaction.Resolve<PluginBag>();
            _cubes = bag.GetCubes();    
        }

        return _cubes;
    }

    public void SwitchPlayer(Guid playerId)
    {
        using (var cubeTransaction = container.BeginLifetimeScope())
        {
            var bag = cubeTransaction.Resolve<PluginBag>();
            bag.SetCurrentPlayer(playerId);
        }
    }

    public void SwitchCube(Guid cubeId)
    {
        using (var cubeTransaction = container.BeginLifetimeScope())
        {
            var bag = cubeTransaction.Resolve<PluginBag>();
            bag.SetCurrentCube(cubeId);
        }
    }
    #endregion
}
