using System;
using System.IO;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;
public record Artist (string name, string description ) : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Name {get; set;} = name;
	public string Description {get; set;} = description;
	public char[]? AvatarBase64 {get; set;}

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

	#region Avatar Manipulation
	public byte[] GetAvatar()
    {
        try
        {
            byte[] result;
            return Convert.FromBase64CharArray(AvatarBase64, 0, AvatarBase64.Length);
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            return null;
        }
    }

    public async void SetAvatar(string path)
    {
        if(File.Exists(path))
        {
        	try
        	{
	        	byte[]? fileBytes;
    	    	using (FileStream fileStream = File.Open(path, FileMode.Open))
        		{
        			fileBytes = new byte[fileStream.Length];
        			await fileStream.ReadAsync(fileBytes, 0, (int)fileStream.Length);
        		}
        		if (fileBytes != null)
        		{
        			AvatarBase64 = null;
        			AvatarBase64 = new char[fileBytes.Length];
        			Convert.ToBase64CharArray(fileBytes, 0, fileBytes.Length, AvatarBase64, 0);
        		}
        		fileBytes = null;
        	}
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                return;
            }
        }
    }
    #endregion
}
