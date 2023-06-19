using ShareInstances.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShareInstances.Services.Interfaces;
public interface ICastle
{
    public bool IsCenterActive {get; set;}


    public void OnCenterRegisterActivate();
    
    public void RegistGhost(IGhost ghost); 

    public IGhost GetGhost(ReadOnlyMemory<char> name);


    public void RegistWaiter(IWaiter waiter); 

    public IWaiter GetWaiter(ReadOnlyMemory<char> name);

}
