using ShareInstances.Instances;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareInstances;
public interface IShare
{        
}

//Represent Cube instance wich interacts with file system
public interface ICube : IShare
{
    public Guid CubeId { get; }
    public string CubeName { get; }

    public int CubePage {get;}

    #region ToggleMethods
    void Init();
    #endregion

    #region ResourceCollections
    public IEnumerable<Artist> Artists {get;}
    public IEnumerable<Playlist> Playlists { get; }
    public IEnumerable<Track> Tracks { get; }        
    #endregion

    #region AddMethods
    public void AddArtistObj(Artist artist);
    public void AddTrackObj(Track artist);
    public void AddPlaylistObj(Playlist artist);
    #endregion

    #region EditMethods
    public void EditArtistObj(Artist artist);
    public void EditTrackObj(Track artist);
    public void EditPlaylistObj(Playlist artist);
    #endregion

    #region RemoveMethods
    public void RemoveArtistObj(Artist artist);
    public void RemoveTrackObj(Track artist);
    public void RemovePlaylistObj(Playlist artist);
    #endregion 
}

//Represent Player instance
public interface IPlayer : IShare
{
    public Guid PlayerId { get; }
    public string PlayerName { get; }

    public CurrentEntity CurrentEntity { get; }
    public Track CurrentTrack { get; }

    public bool IsSwipe { get; }
    public bool IsEmpty { get; }
    public bool PlayerState { get; }

    public TimeSpan TotalTime { get; }
    public TimeSpan CurrentTime { get; set; }

    public float MaxVolume {get;}
    public float MinVolume {get;}
    public float CurrentVolume {get; set;}

    public event Action TrackStarted;


    public void DropTrack(Track track);

    public void DropPlaylist(Playlist playlist, int index=0);

    public void SetNotifier(Action callBack);


    public Task Play();

    public Task StopPlayer();

    public Task Pause_ResumePlayer();

    public Task RepeatTrack();

    public void DropPrevious();

    public void DropNext();

    public Task ShuffleTrackCollection();

    public Task ChangeVolume(float volume);
}
