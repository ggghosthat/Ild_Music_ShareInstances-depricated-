using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;

using System;
using System.Linq;
using System.Collections.Generic;

namespace ShareInstances.StoreSpace;
public struct Store
{
	private static DateTime InitTime;

	private static SupporterService supporterService;

	public Store(SupporterService supporter)
	{
		supporterService = supporter;
		InitTime = DateTime.Now;
	}


	#region Getting by id
	public IList<Artist> GetArtistsById(IList<Guid> ids)
	{
		return ids.ToEntity<Artist>(supporterService.ArtistsCollection);
	}

	public IList<Playlist> GetPlaylistsById(IList<Guid> ids)
	{
		return ids.ToEntity<Playlist>(supporterService.PlaylistsCollection);
	}

	public IList<Track> GetTracksById(IList<Guid> ids)
	{
		return ids.ToEntity<Track>(supporterService.TracksCollection);
	}	
	#endregion
}