using AutoMapper;
using WP.Data;

namespace WP.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Example: Map `Post` entity to `PostDto`
            CreateMap<WpPost, PostDto>();
            CreateMap<PostDto, CreatePostDto>();
        }
    }
}
