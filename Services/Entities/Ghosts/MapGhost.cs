using ShareInstances.Instances;
using ShareInstances.Instances.Map;
using ShareInstances.Services.Interfaces;

using System;
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
    }
}
