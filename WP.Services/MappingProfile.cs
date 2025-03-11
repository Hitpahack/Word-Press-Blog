using AutoMapper;
using WP.Data;

namespace WP.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Example: Map `Post` entity to `PostDto`
            CreateMap<WpPost, PostDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.PostTitle))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.PostContent))
                .ForMember(dest => dest.Excerpt, opt => opt.MapFrom(src => src.PostExcerpt))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PostStatus))
                .ForMember(dest => dest.PostAuthor, opt => opt.MapFrom(src => src.PostAuthor));

            CreateMap<PostDto, CreatePostDto>();
            CreateMap<WpUser, UserDto>();
            CreateMap<UserDto, EditUserDto>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.UserNicename));

            CreateMap<EditUserDto, UserDto>();

            CreateMap<WpUser, EditUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));


        }
    }
}
