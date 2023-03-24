using System;
using System.IO;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public record Artist : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Name {get; set;}
	public string Description {get; set;}
	public string AvatarBase64 {get; set;}

	public IList<Guid> Tracks {get; private set;} = new List<Guid>();
	public IList<Guid> Playlists {get; private set;} = new List<Guid>();


	public Artist (string name,
				   string description,
				   string avatar = null )
	{
		Name = name;
		Description = description;
		AvatarBase64 = SetAvatar(avatar);
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
            var result = Convert.FromBase64String(AvatarBase64);
            return result;
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            throw ex;
            // return null;
        }
    }


    public void DefineAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
        	try
        	{
	        	byte[] file = System.IO.File.ReadAllBytes(path);
                string result = Convert.ToBase64String(file); 
        	}
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;
            }
        }
    }

    public string SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
        	try
        	{
	        	byte[] file = System.IO.File.ReadAllBytes(path);
                string result = Convert.ToBase64String(file); 
                return result;
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
