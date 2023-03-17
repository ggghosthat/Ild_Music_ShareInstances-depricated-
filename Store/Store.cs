using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;

using System;
using System.Collections.Generic;

namespace ShareInstances.Store;
public struct Store
{
	private static DateTime InitTime;

	public IList<ICoreEntity> ArtistStore {get; set;}
	public IList<ICoreEntity> PlaylistStore {get; set;}
	public IList<ICoreEntity> TrackStore {get; set;}

	public Store(DateTime initTime = default)
	{
		InitTime = DateTime.Now;

		ArtistStore = new List<ICoreEntity>();
		PlaylistStore = new List<ICoreEntity>();
		TrackStore = new List<ICoreEntity>();
	}

	public Store(IList<ICoreEntity> artistStore,
				 IList<ICoreEntity> playlistStore,
				 IList<ICoreEntity> trackStore)
	{
		InitTime = DateTime.Now;

		ArtistStore = artistStore;
		PlaylistStore = playlistStore;
		TrackStore = trackStore;
	}
}