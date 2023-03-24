using ShareInstances.Instances;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.InstanceProducer;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities;
public class FactoryService : IService
{
    public string ServiceName {get; init;} = "FactoryService";        

    public SupporterService SupporterService {get; set;}
    private InstanceProducer.InstanceProducer producer = default;
    

    #region Public Methods
    public void CreateArtist(string name,
                             string description,
                             string avatar = null)
    {
        try
        {
            producer = new InstanceProducer.InstanceProducer(name, description, avatar);
            SupporterService.AddInstance(producer.ArtistInstance);
            producer.Dispose();
        }
        catch (InvalidArtistException ex)
        {
            throw ex;
        }
    }
        

    public void CreatePlaylist(string name,
                               string description,
                               string avatar = null,
                               IList<Track> tracks = null,
                               IList<Artist> artists = null)
    {   
        try
        {
            producer = new InstanceProducer.InstanceProducer(name, description, avatar, tracks, artists);
            SupporterService.AddInstance(producer.PlaylistInstance);
            producer.Dispose();
        }
        catch (InvalidPlaylistException ex)
        {
            throw ex;
        }
    }

    public void CreateTrack(string pathway,
                            string name,
                            string description,
                            string avatar = null,
                            IList<Artist> artists = null)
    {      
        try
        {      
            producer = new InstanceProducer.InstanceProducer(pathway, name, description, avatar, artists);
            SupporterService.AddInstance(producer.TrackInstance);
            producer.Dispose();
        }
        catch (InvalidTrackException ex)
        {
            throw ex;
        }
    }
    #endregion
}