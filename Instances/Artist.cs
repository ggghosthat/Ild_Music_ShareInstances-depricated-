using System;
using System.IO;
using System.Collections.Generic;

namespace ShareInstances.Instances;
public struct Artist
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public ReadOnlyMemory<char> Name {get; set;}
	public ReadOnlyMemory<char> Description {get; set;}
	public ReadOnlyMemory<char> AvatarBase64 {get; set;}

	public IList<Guid> Tracks {get; private set;} = new List<Guid>();
	public IList<Guid> Playlists {get; private set;} = new List<Guid>();


	public Artist (ReadOnlyMemory<char> name,
				   ReadOnlyMemory<char> description,
				   ReadOnlyMemory<char> avatarPath)
	{
		Name = name;
		Description = description;
        AvatarBase64 = avatarPath; 
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
        if (AvatarBase64.ToString() is not null)
        {
            try
            {
                return Convert.FromBase64String(AvatarBase64.ToString());
            }
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;
                // return null;
            }
        }
        else return null;
    }

    public string SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
        	try
        	{
	        	byte[] file = System.IO.File.ReadAllBytes(path);
                return Convert.ToBase64String(file); 
        	}
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;
            }            
        }
        else return null;
    }
    #endregion
}
