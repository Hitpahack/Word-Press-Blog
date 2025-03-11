using AutoMapper;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Post;
using WP.EDTOs.Users;

namespace WP.Service
{

    public class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile()
        {
            // Example: Map `Post` entity to `PostDto`
            CreateMap<POST_SP_RESPONSE, POST_DT_RESPONSE>();
            CreateMap<POST_DT_RESPONSE, POST_SP_RESPONSE>();

            CreateMap<USERS_SP_RESPONSE, USERS_DT_RESPONSE>();
            CreateMap<USERS_DT_RESPONSE, USERS_SP_RESPONSE>();

            CreateMap<PAGE_SP_RESPONSE, PAGE_DT_RESPONSE>();
            CreateMap<PAGE_DT_RESPONSE, PAGE_SP_RESPONSE>();

            CreateMap<WP_POST_ADD_DTO, WpPost>()
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Post_Title))
                .ForMember(dest => dest.PostContent, opt => opt.MapFrom(src => src.Post_Content))
                .ForMember(dest => dest.PostName, opt => opt.MapFrom(src => src.Post_Name))
                .ForMember(dest => dest.PostStatus, opt => opt.MapFrom(src => src.Post_Status))
                .ForMember(dest => dest.PostDate, opt => opt.MapFrom(src => src.Post_Date))
                .ForMember(dest => dest.PostDateGmt, opt => opt.MapFrom(src => src.Post_Date_Gmt))
                .ForMember(dest => dest.PostAuthor, opt => opt.MapFrom(src => src.Post_Author));

            CreateMap<WpPost, POST_DTO>();
            CreateMap<POST_DTO, WpPost>();

            CreateMap<WpPost, WP_POST_ADD_DTO>();
            CreateMap<WP_POST_ADD_DTO, WpPost>();

            CreateMap<WpPost, WP_PAGE_ADD_DTO>();
            CreateMap<WP_PAGE_ADD_DTO, WpPost>()
                 .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Post_Title))
                .ForMember(dest => dest.PostContent, opt => opt.MapFrom(src => src.Post_Content))
                .ForMember(dest => dest.PostName, opt => opt.MapFrom(src => src.Post_Name))
                .ForMember(dest => dest.PostStatus, opt => opt.MapFrom(src => src.Post_Status))
                .ForMember(dest => dest.PostDate, opt => opt.MapFrom(src => src.Post_Date))
                .ForMember(dest => dest.PostDateGmt, opt => opt.MapFrom(src => src.Post_Date_Gmt))
                .ForMember(dest => dest.PostAuthor, opt => opt.MapFrom(src => src.Post_Author));

        }
    }
}
