using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.StoreSpace;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities;
public class StoreService : IService
{
	public string ServiceName {get; init;} = "StoreService"; 

	public Store StoreInstance {get; private set;}

	public void SetStore(ref Store store)
	{
		StoreInstance = store;
	}
}