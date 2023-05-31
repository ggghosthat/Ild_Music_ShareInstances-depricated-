using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace ShareInstances.Instances;
public struct Playlist
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public ReadOnlyMemory<char> Name {get; private set;}
	public ReadOnlyMemory<char> Description {get; private set;}
    public ReadOnlyMemory<char> AvatarBase64 {get; private set;}      

    //Use for user space playing
    private Lazy<List<Track>> Tracks; 

    //Please, be carefull when you call this property and DO NOT call much this property
    //When playlist contains many track objects, Lazy<T> will init whole list in CLR's heap
    public int Count => Tracks.Value.Count;
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

    public Playlist(ReadOnlyMemory<char> name, 
                    ReadOnlyMemory<char> description,
                    ReadOnlyMemory<char> avatarPath)
    {
        Name = name;
        Description = description;

        if(File.Exists(avatarPath.ToString()))
        {
            AvatarBase64 = Convert.ToBase64String(File.ReadAllBytes(avatarPath.ToString()));
        }

        Tracks = new Lazy<List<Track>>();
    }

    #region Collection Manipulation Methods
    public void AddTrack(Track track)
    {        
    	Tracks.Value.Add(track.Id);
    }

    public void RemoveTrack(Track track)
    {        
    	if(Tracks.Value.Contains(track))
    	{
    		Tracks.Value.Remove(track);
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

    public string SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(path);
                return result = Convert.ToBase64String(file); 
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
