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

	public IList<Guid> Tracks {get; set;} = new List<Guid>();
	public IList<Guid> Playlists {get; set;} = new List<Guid>();


	public Artist (ReadOnlyMemory<char> name,
				   ReadOnlyMemory<char> description,
				   ReadOnlyMemory<byte> avatarSource)
	{
		Name = name;
		Description = description;
        AvatarSource = avatarSource; 
	}


	public void AddTrack(Guid trackId)
	{
		Tracks.Add(trackId);
	}

	public void AddPlaylist(Guid playlistId)
	{
		Playlists.Add(playlistId);
	}

	public void DeleteTrack(Guid trackId) 
	{
		if (Tracks.Contains(trackId))
		{
			Tracks.Remove(trackId);
		}
	}

	public void DeletePlaylist(Guid playlistId) 
	{
		if (Tracks.Contains(playlistId))
		{
			Tracks.Remove(playlistId);
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
