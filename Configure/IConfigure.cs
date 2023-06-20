using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; 

namespace ShareInstances.Configure;
public interface IConfigure
{
    public Memory<char> ComponentsFile {get; init;}
 	public Config ConfigSheet {get; set;}    

    public void Parse();
    public Task ParseAsync();
}
