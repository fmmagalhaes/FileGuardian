using AutoMapper;
using FileGuardian.Api.DTOs;
using FileGuardian.Domain.Entities;

namespace FileGuardian.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // API to application
        CreateMap<CreateFileRequest, File>();

        // Application to API
        CreateMap<File, GetFilesResponse>();
        CreateMap<File, GetFileResponse>()
            .ForMember(f => f.Users, opt => opt.MapFrom(src => src.FileUsers.Select(fu => fu.UserId)))
            .ForMember(f => f.Groups, opt => opt.MapFrom(src => src.FileGroups.Select(fu => fu.GroupId)));
        CreateMap<Group, GetGroupResponse>()
            .ForMember(f => f.Users, opt => opt.MapFrom(src => src.GroupUsers.Select(fu => fu.User)));

        // Both directions
        CreateMap<GroupDto, Group>().ReverseMap();
    }
}
