using ShareInstances.Instances;
using ShareInstances.Filer;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.InstanceProducer;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities;
public class FactoryGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "FactoryGhost".AsMemory(); 

    public static SupportGhost SupportGhost {get; private set;}
    private static InstanceProducer.InstanceProducer producer = default;
    
    public FactoryGhost(){}

    public void Init(ref SupportGhost supportGhost)
    {
        SupportGhost = supportGhost;
    }

    #region Public Methods
    public void CreateArtist(string name,
                             string description,
                             string avatarPath = null)
    {
        try
        {
            producer = new InstanceProducer.InstanceProducer(name.AsMemory(),
                                                             description.AsMemory(),
                                                             avatarPath.AsMemory());
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
                            string name=null,
                            string description=null,
                            string avatarPath = null,
                            IList<Artist> artists = null)
    {      
        try
        {               
            using(var taglib = TagLib.File.Create(pathway))
            {
                
                Memory<char> trackName;
                Memory<char> trackDescription;
                Memory<byte> trackAvatarSource;

                if(name is null)
                {
                    trackName = taglib.Tag.Title.ToCharArray()
                                    ?? Path.GetFileName(pathway).ToCharArray();
                }
                else trackName = name.ToCharArray();

                if(description is not null)
                {
                    trackDescription = description.ToCharArray();
                }

                if (string.IsNullOrEmpty(avatarPath))
                {
                    if(taglib.Tag.Pictures.Length > 0)
                    {
                         trackAvatarSource = taglib.Tag.Pictures[0].Data.Data;
                    }
                }
                else trackAvatarSource = ExtractTrackAvatar(avatarPath.AsMemory()).Result;
                

                producer = new InstanceProducer.InstanceProducer(pathway.AsMemory(),
                                                                 name.AsMemory(),
                                                                 description.AsMemory(),
                                                                 avatarPath.AsMemory(),
                                                                 artists);
                SupportGhost.AddTrackInstance(producer.TrackInstance);
                producer.Dispose();
            }
        }
        catch (InvalidTrackException ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Accessory Methods

    private async ValueTask<Memory<byte>> ExtractTrackAvatar(ReadOnlyMemory<char> pathway)
    {
        return await ShareInstances.Filer.Filer.ReadFileAsync(pathway.ToString());
    }
    #endregion
}
