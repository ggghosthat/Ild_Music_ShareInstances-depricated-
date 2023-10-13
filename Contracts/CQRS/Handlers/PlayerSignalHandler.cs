using ShareInstances.CQRS.Notifications;
using ShareInstances.CQRS.Signals;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
namespace ShareInstances.CQRS.Handlers;
public class PlayerNotificationHandler : INotificationHandler<PlayerNotification>
{
    
    public Task Handle(PlayerNotification playerNotification, CancellationToken token)
    {
        var action = playerNotification.PlayerSignal switch
        {
           PlayerSignal.ON => 0, 
           PlayerSignal.OFF =>1,
           PlayerSignal.REPEAT => 2,
           PlayerSignal.PAUSE => 3,
           PlayerSignal.TRACK => 4,
           PlayerSignal.PLAYLIST => 5,
           PlayerSignal.SHIFT_LEFT => 6,
           PlayerSignal.SHIFT_RIGHT => 7
        }; 
        return Task.CompletedTask;
    }
}
