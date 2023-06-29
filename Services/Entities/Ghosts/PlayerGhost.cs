using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.InstanceProducer;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities;
public sealed class PlayerGhost : IGhost
{
	public ReadOnlyMemory<char> GhostName {get; init;} = "PlayerGhost".AsMemory(); 

	public static IPlayer PlayerInstance {get; private set;}

    public PlayerGhost(){}

	public void Init(ref IPlayer player)
    {
		PlayerInstance = player;    	
    }

    public void SwitchPlayer(ref IPlayer player)
    {
        PlayerInstance = player;
    }
}
