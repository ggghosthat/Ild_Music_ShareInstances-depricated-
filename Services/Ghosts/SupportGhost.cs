using System.Collections.Generic;
using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Services.Interfaces;
using ShareInstances.Instances.Interfaces;

using System;

namespace ShareInstances.Services.Entities;
public ref struct SupportGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "SupporterService";

    //temp solution
    public static ISynchArea SynchArea;

    public IList<Artist> ArtistsCollection => SynchArea.existedArtists;
    public IList<Playlist> PlaylistsCollection => SynchArea.existedPlaylists;
    public IList<Track> TracksCollection => SynchArea.existedTracks;


    public event Action OnArtistsNotifyRefresh = null;
    public event Action OnPlaylistsNotifyRefresh = null;
    public event Action OnTracksNotifyRefresh = null;

    //Initialize and start Synch Area instance 
    public SupportGhost(ISynchArea synchArea) 
    {
        SynchArea = synchArea;
        SynchArea.Init();
    }
    
    

    public void AddArtistInstance(Artist artist)
    {
        SynchArea.AddArtistObj(artist);
        SynchArea.SaveArtists();
        OnArtistsNotifyRefresh?.Invoke();
    }

    public void AddPlaylistInstance(Playlist playlist)
    {
        SynchArea.AddPlaylistObj(playlist);
        SynchArea.SavePlaylists();
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void AddTrackInstance(Track track)
    {
        SynchArea.AddTrackObj(track);
        SynchArea.SaveTracks();
        OnTracksNotifyRefresh?.Invoke();
    }


    public void EditArtistInstance(Artist artist)
    {
        SynchArea.EditArtistObj(artist);
        SynchArea.SaveArtists();
        OnArtistsNotifyRefresh?.Invoke();
    }
    
    public void EditPlaylistInstance(Playlist playlist)
    {
        SynchArea.EditPlaylistObj(playlist);
        SynchArea.SavePlaylists();
        OnPlaylistsNotifyRefresh?.Invoke();
    }
    
    public void EditTrackInstance(Track track)
    {
        SynchArea.EditTrackObj(track);
        SynchArea.SaveTracks();
        OnTracksNotifyRefresh?.Invoke();  
    }


    public void DeleteArtistInstance(Artist artist) 
    {
        SynchArea.RemoveArtistObj(artist);
        SynchArea.SaveArtists();
        OnTracksNotifyRefresh?.Invoke();
    }
    
    public void DeletePlaylistInstance(Playlist playlist) 
    {
        SynchArea.RemovePlaylistObj(playlist);
        SynchArea.SavePlaylists();
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void DeleteTrackInstance(Track track)
    {
        SynchArea.RemoveTrackObj(track);
        SynchArea.SaveTracks();
        OnArtistsNotifyRefresh?.Invoke();
    }
       
    public void DumpState(int i = 0)
    {
        switch(i)
        {
            case 0:
                SynchArea.Save();
                break;
            case 1:
                SynchArea.SaveArtists();
                break;
            case 2:
                SynchArea.SavePlaylists();
                break;
            case 3:
                SynchArea.SaveTracks();
                break;
        }
    }
}
