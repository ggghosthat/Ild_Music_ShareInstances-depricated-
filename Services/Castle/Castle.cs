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

public enum Ghosts {SUPPORT, FACTORY, PLAYER}

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
        builder.RegisterType<SupportGhost>().As<IGhost>().Keyed<IGhost>(Ghosts.SUPPORT);
        builder.RegisterType<FactoryGhost>().As<IGhost>().Keyed<IGhost>(Ghosts.FACTORY);
        builder.RegisterType<PlayerGhost>().As<IGhost>().Keyed<IGhost>(Ghosts.PLAYER);
        builder.RegisterType<Filer>().As<IWaiter>();      
        
        container = builder.Build();
        IsCenterActive = true;
    }


    #region resolve region
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
