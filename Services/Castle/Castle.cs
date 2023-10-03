using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Stage;
using ShareInstances;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
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

        builder.RegisterMediatR(configuration);
        builder.RegisterType<PluginBag>().As<IPluginBag>().SingleInstance();
        builder.RegisterType<SupportGhost>().As<IGhost>();
        builder.RegisterType<FactoryGhost>().As<IGhost>();
        builder.RegisterType<PlayerGhost>().As<IGhost>();
        builder.RegisterType<Filer>().As<IWaiter>();     

        builder.RegisterType<UIControlHolder<object>>().As<IGhost>();
        builder.RegisterType<ViewModelHolder<object>>().As<IGhost>();
       
         
        container = builder.Build();
        //IsCenterActive = true;
    }


    #region Accomodation & recieving region
    //accomodate ghost from entire scope 
    private void LodgeGhost(IGhost ghost)
    {}

    //accomodate ghost from external scope
    public void RegistGhost(IGhost ghost) 
    {}
    
    //return ghost by its own name
    public IGhost GetGhost(ReadOnlyMemory<char> name)
    {return null;}

    //accomodating waiter inside the castle
    public void RegistWaiter(IWaiter waiter)
    {}

    //Recover waiter instance
    public void UpdateWaiter(IWaiter waiter)
    {}
    //return waiter by its own name
    public IWaiter GetWaiter(ReadOnlyMemory<char> name) 
    {
       return null; 
    }

    //return all ghost instances
    public IList<ReadOnlyMemory<char>> GetGhosts()
    {return null;} 
    //return all ghost instances
    public IList<ReadOnlyMemory<char>> GetWaiters()
    {return null;}
    #endregion

    #region resolve region
    public void RegisterCube(ICube cube)
    {
        using (var cubeTransaction = container.BeginLifetimeScope(
            builder =>
            {
                builder.RegisterInstance(cube).As<ICube>()
                       .SingleInstance();
            }))
        {
            using (var transaction = cubeTransaction.BeginLifetimeScope())
            {
                var _cube = transaction.Resolve<ICube>();
                var supporter = transaction.Resolve<SupportGhost>();
                var factory = transaction.Resolve<FactoryGhost>();

                supporter.Init(ref _cube);
                factory.Init(ref supporter);
            }
        }
        //UpdateGhost(ref (IGhost)supporter);
        //UpdateGhost(ref (IGhost)factory);
    }

    public void RegisterPlayer(IPlayer _player)
    {
        using (var playerTransaction = container.BeginLifetimeScope(
            builder =>
            {
                builder.RegisterInstance(_player)
                       .InstancePerMatchingLifetimeScope("PlayerTransaction");
            }))
        {
            using (var transaction = playerTransaction.BeginLifetimeScope("PlayerTransaction"))
            {
                var player = transaction.Resolve<PlayerGhost>();
                player.Init(ref _player);
            }
        }
    } 
    #endregion
}
