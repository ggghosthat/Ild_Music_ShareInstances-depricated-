using ShareInstances.Instances;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareInstances.Services.InstanceProducer;
internal struct InstanceProducer : IDisposable
{        
    public Artist ArtistInstance { get; private set; } = default!;
    public Playlist PlaylistInstance { get; private set; } = default!;
    public Track TrackInstance { get; private set; } = default!;

    public InstanceProducer(Memory<char> name,
                            Memory<char> description, 
                            Memory<char> avatar)
    {
        ArtistInstance = new Artist(name: name,
                                    description: description,
                                    avatarPath: avatar);
    }

    public InstanceProducer(Memory<char> name,
                            Memory<char> description,
                            Memory<char> avatar,
                            IList<Track> tracks,
                            IList<Artist> artists)
    {
        var playlist  = new Playlist(name: name,
                                     description: description,
                                     avatarPath: avatar);

        if (tracks != null && tracks.Count > 0)
        {
            tracks.ToList().ForEach(t => playlist.AddTrack(t));
        }

        if (artists != null && artists.Count > 0)
        {
            artists.ToList().ForEach(a => 
            {
                a.AddPlaylist(playlist.Id);
                playlist.Artists.Add(a.Id);
            });            
        }

        PlaylistInstance = playlist;
    }

    public InstanceProducer(Memory<char> pathway,
                            Memory<char> name,
                            Memory<char> description,
                            Memory<byte> avatar,
                            TimeSpan duration,
                            IList<Artist> artists = null)
    {
        TrackInstance = new Track(pathway: pathway,
                                  name: name,
                                  description: description,
                                  avatarSource: avatar,
                                  duration: duration);
        var track = TrackInstance;

        if (artists != null && artists.Count > 0)
        {
            artists.ToList().ForEach(a => 
            {
                a.AddTrack(track.Id);
                track.Artists.Add(a.Id);
            });

        }
    }

    public void Dispose()
    {
        ArtistInstance = default!;
        PlaylistInstance = default!; 
        TrackInstance = default!;
    }
}
