using System.Collections.Generic;
using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Services.Interfaces;

using System;

namespace ShareInstances.Services.Entities;
public class SupportGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "SupporterService".AsMemory();

    //temp solution
    public static ICube CubeArea;

    public IList<Artist> ArtistsCollection => CubeArea.existedArtists;
    public IList<Playlist> PlaylistsCollection => CubeArea.existedPlaylists;
    public IList<Track> TracksCollection => CubeArea.existedTracks;


    public event Action OnArtistsNotifyRefresh = null;
    public event Action OnPlaylistsNotifyRefresh = null;
    public event Action OnTracksNotifyRefresh = null;

    public SupportGhost(){}

    //Initialize and start Synch Area instance 
    public void Init(ref ICube syncCube) 
    {
        CubeArea = syncCube;
        CubeArea.Init();
    }
    
    

    public void AddArtistInstance(Artist artist)
    {
        CubeArea.AddArtistObj(artist);
        CubeArea.SaveArtists();
        OnArtistsNotifyRefresh?.Invoke();
    }

    public void AddPlaylistInstance(Playlist playlist)
    {
        CubeArea.AddPlaylistObj(playlist);
        CubeArea.SavePlaylists();
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void AddTrackInstance(Track track)
    {
        CubeArea.AddTrackObj(track);
        CubeArea.SaveTracks();
        OnTracksNotifyRefresh?.Invoke();
    }


    public void EditArtistInstance(Artist artist)
    {
        CubeArea.EditArtistObj(artist);
        CubeArea.SaveArtists();
        OnArtistsNotifyRefresh?.Invoke();
    }
    
    public void EditPlaylistInstance(Playlist playlist)
    {
        CubeArea.EditPlaylistObj(playlist);
        CubeArea.SavePlaylists();
        OnPlaylistsNotifyRefresh?.Invoke();
    }
    
    public void EditTrackInstance(Track track)
    {
        CubeArea.EditTrackObj(track);
        CubeArea.SaveTracks();
        OnTracksNotifyRefresh?.Invoke();  
    }


    public void DeleteArtistInstance(Artist artist) 
    {
        CubeArea.RemoveArtistObj(artist);
        CubeArea.SaveArtists();
        OnTracksNotifyRefresh?.Invoke();
    }
    
    public void DeletePlaylistInstance(Playlist playlist) 
    {
        CubeArea.RemovePlaylistObj(playlist);
        CubeArea.SavePlaylists();
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void DeleteTrackInstance(Track track)
    {
        CubeArea.RemoveTrackObj(track);
        CubeArea.SaveTracks();
        OnArtistsNotifyRefresh?.Invoke();
    }
       
    public void DumpState(int i = 0)
    {
        switch(i)
        {
            case 0:
                CubeArea.Save();
                break;
            case 1:
                CubeArea.SaveArtists();
                break;
            case 2:
                CubeArea.SavePlaylists();
                break;
            case 3:
                CubeArea.SaveTracks();
                break;
        }
    }
}
