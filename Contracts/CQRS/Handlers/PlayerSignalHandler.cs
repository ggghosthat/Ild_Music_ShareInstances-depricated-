using ShareInstances.CQRS.Notifications;
using ShareInstances.CQRS.Signals;
using ShareInstances.CQRS.Handlers.Delegatebag;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
namespace ShareInstances.CQRS.Handlers;
public class PlayerNotificationHandler : INotificationHandler<PlayerNotification>
{  
    private readonly DelegateBag _delegateBag;

    public PlayerNotificationHandler(DelegateBag delegateBag)
    {
        _delegateBag = delegateBag;
    }

    public Task Handle(PlayerNotification playerNotification, CancellationToken token)
    {
        var action = playerNotification.PlayerSignal switch
        {
           PlayerSignal.PLAYER_ON => 0, 
           PlayerSignal.PLAYER_OFF =>1,
           PlayerSignal.PLAYER_REPEAT => 2,
           PlayerSignal.PLAYER_PAUSE => 3,
           PlayerSignal.PLAYER_SET_TRACK => 4,
           PlayerSignal.PLAYER_SET_PLAYLIST => 5,
           PlayerSignal.PLAYER_SHIFT_LEFT => 6,
           PlayerSignal.PLAYER_SHIFT_RIGHT => 7,
           _ => default
        }; 
        return Task.CompletedTask;
    }
}
