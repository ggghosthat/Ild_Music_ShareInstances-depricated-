using ShareInstances.Instances;
using ShareInstances.Instances.Map;
using ShareInstances.Services.Interfaces;

using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

namespace ShareInstances.Services.Entities;
public sealed class MapGhost : Profile, IGhost
{    
	public ReadOnlyMemory<char> GhostName {get; init;} = "MapGhost".AsMemory(); 
    public MapGhost()
    {
        CreateMap<Artist, ArtistMap>()
            .ForMember(dest => dest.Buid, opt => opt.MapFrom(src => src.Id.ToByteArray()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarSource));

        CreateMap<Playlist, PlaylistMap>()
            .ForMember(dest => dest.Buid, opt => opt.MapFrom(src => src.Id.ToByteArray()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarSource));

        CreateMap<Track, TrackMap>()
            .ForMember(dest => dest.Buid, opt => opt.MapFrom(src => src.Id.ToByteArray()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarSource))
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.IsValid))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));

        CreateMap<Artist, Store<Track>>()
            .ConvertUsing((src) => GenerateStore<Track>(0, src.Id, src.Tracks));

        CreateMap<Artist, Store<Playlist>>()
            .ConvertUsing((src) => GenerateStore<Playlist>(1, src.Id, src.Playlists));

        CreateMap<Track, Store<Artist>>()
            .ConvertUsing((src) => GenerateStore<Artist>(2, src.Id, src.Artists));

        CreateMap<Track, Store<Playlist>>()
            .ConvertUsing((src) => GenerateStore<Playlist>(5, src.Id, src.Playlists));

        CreateMap<Playlist, Store<Artist>>()
            .ConvertUsing((src) => GenerateStore<Artist>(3, src.Id, src.Artists));

        CreateMap<Playlist, Store<Track>>()
            .ConvertUsing((src) => GenerateTrackStoreFromPlaylist(src.Id, src.GetTracks() ));
    }

    private Store<T> GenerateStore<T>(int tag, Guid main, ICollection<Guid> items)
    {
        var store = new Store<T>(tag);
        items.ToList().ForEach(i =>
        {
            store.Pairs.Add(new Pair(main, i));
        });

        return store;
    }

    private Store<Track> GenerateTrackStoreFromPlaylist(Guid playlistId, IList<Track> tracks)
    {
        var store = new Store<Track>(4);
        tracks.ToList().ForEach(t =>
        {
            store.Pairs.Add(new Pair(playlistId, t.Id));
        });

        return store;
    }
}
