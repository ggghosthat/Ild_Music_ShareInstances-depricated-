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
    void SetPath(ref string inputPath); 
    void Init();
    #endregion

    #region ResourceCollections
    public IEnumerable<Artist> Artists {get;}
    public IEnumerable<Playlist> Playlists { get; }
    public IEnumerable<Track> Tracks { get; }        
    #endregion

    #region AddMethods
    public Task AddArtistObj(Artist artist);
    public Task AddTrackObj(Track artist);
    public Task AddPlaylistObj(Playlist artist);
    #endregion

    #region EditMethods
    public Task EditArtistObj(Artist artist);
    public Task EditTrackObj(Track artist);
    public Task EditPlaylistObj(Playlist artist);
    #endregion

    #region RemoveMethods
    public Task RemoveArtistObj(Artist artist);
    public Task RemoveTrackObj(Track artist);
    public Task RemovePlaylistObj(Playlist artist);
    #endregion 

    #region LoadMethods
    public Task LoadItems<T>();
    public Task UnloadItems<T>();
    #endregion

    #region ExtendMethods
    public Task<Artist> ExtendSingle(Artist artist);
    public Task<Playlist> ExtendSingle(Playlist playlist);
    public Task<Track> ExtendSingle(Track track);
    #endregion

    #region IntegrityChecks
    public Task<bool> CheckArtistIntegrity(Artist artist);
    public Task<bool> CheckPlaylistIntegrity(Playlist playlist);
    public Task<bool> CheckTrackIntegrity(Track track);
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
