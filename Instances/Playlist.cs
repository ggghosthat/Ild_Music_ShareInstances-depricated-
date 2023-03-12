using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;


namespace ShareInstances.Instances;
public record PlaylistV2(string name, string description, IList<Guid> tracks = null) : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Name {get; set;} = name;
	public string Description {get; set;} = description;
    public string AvatarBase64 {get; set;}      

	public IList<Guid> Tracks = new List<Guid>(tracks);
    public int Count => Tracks.Count;
	public Guid Head {get; private set;}
	public Guid Tail {get; private set;}

	public int CurrentIndex {get; set;}
	public bool IsOrdered { get; private set; } = false;

    #region Collection Manipulation Methods
    public void AddTrack(TrackV2 track)
    {        
    	Tracks.Add(track.Id);
    }

    public void RemoveTrack(TrackV2 track)
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
    public void SetAvatar(string path)
    {
        if(File.Exists(path))
        {}
    }
    #endregion
}