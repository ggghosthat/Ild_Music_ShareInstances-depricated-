using ShareInstances.Instances;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.InstanceProducer;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities;
public ref struct FactoryGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "FactoryGhost";        

    public SupportGhost SupportGhost {get; private set;}
    private InstanceProducer.InstanceProducer producer = default;
    

    public FactoryGhost(ref SupportGhost supportGhost)
    {
        SupportGhost = supportGhost;
    }

    #region Public Methods
    public void CreateArtist(string name,
                             string description,
                             string avatar = null)
    {
        try
        {
            producer = new InstanceProducer.InstanceProducer(name.AsMemory(),
                                                             description.AsMemory(),
                                                             avatar.AsMemory());
            SupportGhost.AddArtistInstance(producer.ArtistInstance);
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
            producer = new InstanceProducer.InstanceProducer(name.AsMemory(),
                                                             description.AsMemory(),
                                                             avatar.AsMemory(),
                                                             tracks,
                                                             artists);
            SupportGhost.AddPlaylistInstance(producer.PlaylistInstance);
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
            producer = new InstanceProducer.InstanceProducer(pathway.AsMemory(),
                                                             name.AsMemory(),
                                                             description.AsMemory(),
                                                             avatar.AsMemory(),
                                                             artists);
            SupportGhost.AddTrackInstance(producer.TrackInstance);
            producer.Dispose();
        }
        catch (InvalidTrackException ex)
        {
            throw ex;
        }
    }
    #endregion
}
