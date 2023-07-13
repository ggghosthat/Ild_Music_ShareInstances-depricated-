using System;
using System.IO;
using System.Collections.Generic;

namespace ShareInstances.Instances;
public struct Artist
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public ReadOnlyMemory<char> Name {get; set;}
	public ReadOnlyMemory<char> Description {get; set;}
	public ReadOnlyMemory<byte> AvatarSource {get; set;}

	public ICollection<Guid> Tracks {get; set;} = new List<Guid>();
	public ICollection<Guid> Playlists {get; set;} = new List<Guid>();

	public ICollection<Tag> Tags {get; set;} = new List<Tag>();


	public Artist (ReadOnlyMemory<char> name,
				   ReadOnlyMemory<char> description,
				   ReadOnlyMemory<byte> avatarSource)
	{
		Name = name;
		Description = description;
        AvatarSource = avatarSource; 
	}


	public void AddTrack(ref Track track)
	{
		if(!Tracks.Contains(track.Id))
		{
			Tracks.Add(track.Id);
			track.Artists.Add(Id);
		}
	}

	public void AddPlaylist(ref Playlist playlist)
	{
		if(!Playlists.Contains(playlist.Id))
		{
			Playlists.Add(playlist.Id);
			playlist.Artists.Add(Id);
		}
	}

	public void DeleteTrack(ref Track track) 
	{
		if (Tracks.Contains(track.Id))
		{
			Tracks.Remove(track.Id);
			track.Artists.Remove(Id);
		}
	}

	public void DeletePlaylist(ref Playlist playlist) 
	{
		if (!Playlists.Contains(playlist.Id))
		{
			Playlists.Remove(playlist.Id);
			playlist.Artists.Remove(Id);
		}
	}

	#region Avatar Manipulation
	public byte[] GetAvatar()
    {
		try
		{
			return AvatarSource.ToArray();
		}
		catch(Exception ex)
		{
			//Speciall logging or throwing logic
			throw ex;
			// return null;
		}
    }

    public void SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(path);
                AvatarSource = file; 
            }
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;   
            }            
        }
    }
    #endregion
}
