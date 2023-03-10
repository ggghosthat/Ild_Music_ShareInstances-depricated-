using System;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public record ArtistV2 (string name, string description ) : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Name {get; set;} = name;
	public string Description {get; set;} = description;

	public IList<Guid> Tracks {get; private set;} = new List<Guid>();
	public IList<Guid> Playlists {get; private set;} = new List<Guid>();

	public void AddTrack(Guid trackId)
	{
		Tracks.Add(trackId);
	}

	public void AddPlaylist(Guid playlistId)
	{
		Playlists.Add(playlistId);
	}

	public void RemoveTrack(Guid trackId) 
	{
		if (Tracks.Contains(trackId))
		{
			Tracks.Remove(trackId);
		}
	}

	public void RemovePlaylist(Guid playlistId) 
	{
		if (Tracks.Contains(playlistId))
		{
			Tracks.Remove(playlistId);
		}
	}
}
