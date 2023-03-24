using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;


namespace ShareInstances.Instances;
public record Playlist : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Name {get; set;}
	public string Description {get; set;}
    public string AvatarBase64 {get; set;}      

	public IList<Guid> Tracks = new List<Guid>();
    public int Count => Tracks.Count;
	public Guid Head {get; private set;}
	public Guid Tail {get; private set;}

	public int CurrentIndex {get; set;}
	public bool IsOrdered { get; private set; } = false;

    public Guid this[int i]
    {
        get
        {
            return Tracks[i];
        }
    }

    public Playlist(string name, 
                    string description,
                    string avatar = null,
                    IList<Guid> tracks = null)
    {
        Name = name;
        Description = description;
        AvatarBase64 = SetAvatar(avatar);
        if(tracks != null)
        {
            Tracks = new List<Guid>(tracks);
        }
        else 
        {
            Tracks = new List<Guid>();
        }
    }

    #region Collection Manipulation Methods
    public void AddTrack(Track track)
    {        
    	Tracks.Add(track.Id);
    }

    public void RemoveTrack(Track track)
    {        
    	if(Tracks.Contains(track.Id))
    	{
    		Tracks.Remove(track.Id);
    	}
    }
    #endregion

    #region Shuffle
    public void Shuffle() 
    {
        IList<Guid> shuffledList = Tracks.OrderBy(i => Guid.NewGuid()).ToList();
        Tracks = shuffledList;
    }
    #endregion

    #region Avatar Manipulation
    public byte[] GetAvatar()
    {
        try
        {
            byte[] result;
            return Convert.FromBase64String(AvatarBase64);
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            return null;
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