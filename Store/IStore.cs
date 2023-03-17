using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;

using System;
using System.Collections.Generic;

namespace ShareInstances.Store;
public interface IStore
{
	public IList<ICoreEntity> ArtistStore {get; set;}
	public IList<ICoreEntity> PlaylistStore {get; set;}
	public IList<ICoreEntity> TrackStore {get; set;}
}