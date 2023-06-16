using System;

namespace ShareInstances.Services.Interfaces;
interface IWaiter
{
    public Guid WaiterId {get; init;}
    public ReadOnlyMemory<char> WaiterName {get; init;}
}
