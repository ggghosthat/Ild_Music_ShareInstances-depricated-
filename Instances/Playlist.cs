using System;
using System.Linq;
using System.Collections.Generic;
using ShareInstances.Instances.Interfaces;

namespace ShareInstances.Instances;

public record PlaylistV2(string name, string description, IList<TrackV2> tracks = null) : ICoreEntity
{
	public Guid Id {get; init;} = Guid.NewGuid();
	public string Name {get; set;} = name;
	public string Description {get; set;} = description;

	public IList<TrackV2> TempTracks = new List<TrackV2>(tracks);
	public IList<Guid> Tracks = new List<Guid>();
    public int Count => Tracks.Count;
	public Guid Head {get; private set;}
	public Guid Tail {get; private set;}

	public int CurrentIndex {get; set;}
	public bool IsOrdered { get; private set; } = false;


    #region Order
    public void Order()
    {
        if (Tracks != null && Tracks.Count > 1 && !IsOrdered)
        {
        	Head = Tracks[0];
        	Tail = Tracks[Count -1];

            for (int i = 0; i < Count; i++)
            {
                if (i == 0)                            
                    OrderHead(i);
                else if (i == (Count - 1))
                    OrderTail(i);
                else
                    OrderBody(i);
            }                 
            // RoundCollection();
            IsOrdered = true;
        }            
    }

    private void RoundCollection()
    {
    }

    private void OrderBody(int i)
    {
        var current = TempTracks[i];
        current.NextTrack = TempTracks[i + 1].Id;
        current.PreviousTrack = TempTracks[i - 1].Id;
    }

    private void OrderTail(int i)
    {
        var tail = TempTracks[i];
        tail.PreviousTrack = TempTracks[i - 1].Id;
    }

    private void OrderHead(int i)
    {
        var head = TempTracks[0];
        head.NextTrack = TempTracks[i + 1].Id;
    }
    #endregion

    #region CollectionManipulationMethods
    public void AddTrack(TrackV2 track)
    {        
    }

    public void RemoveTrack(TrackV2 track)
    {        
    }
    #endregion

    #region Shuffle
    public void Shuffle() 
    {
        IList<Guid> shuffledList = Tracks.OrderBy(i => Guid.NewGuid()).ToList();
        Tracks = shuffledList;
    }
    #endregion
}