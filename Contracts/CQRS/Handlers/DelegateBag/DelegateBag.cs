using ShareInstances.CQRS.Signals;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ShareInstances.CQRS.Handlers.Delegatebag;
public sealed class DelegateBag
{
    private IDictionary<PlayerSignal, Func<int>> _bag = new Dictionary<PlayerSignal, Func<int>>();

    public void Allocate(PlayerSignal signal, Func<int> action) =>
       _bag[signal] = action;

    public Task AllocateAsync(PlayerSignal signal, Func<int> action)
    {
       _bag[signal] = action;
       return Task.CompletedTask;
    }

    public Func<int> GetAction(PlayerSignal signal) =>
        _bag[signal];
}
