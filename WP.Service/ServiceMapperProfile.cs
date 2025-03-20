using AutoMapper;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Comments;
using WP.EDTOs.Medias;
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

            CreateMap<WpPost, WP_POST_ADD_DTO>()
           .ForMember(src => src.Post_Title, opt => opt.MapFrom(dest => dest.PostTitle))
           .ForMember(src => src.Post_Content, opt => opt.MapFrom(dest => dest.PostContent))
           .ForMember(src => src.Post_Name, opt => opt.MapFrom(dest => dest.PostName))
           .ForMember(src => src.Post_Status, opt => opt.MapFrom(dest => dest.PostStatus))
           .ForMember(src => src.Post_Date, opt => opt.MapFrom(dest => dest.PostDate))
           .ForMember(src => src.Post_Date_Gmt, opt => opt.MapFrom(dest => dest.PostDateGmt))
           .ForMember(src => src.Post_Author, opt => opt.MapFrom(dest => dest.PostAuthor));


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


            CreateMap<WP_POST_MEDIA_ADD, WP_POST_ADD_DTO>();
            CreateMap<WP_POST_ADD_DTO, WP_POST_MEDIA_ADD>();

            CreateMap<WP_POST_MEDIA_ADD, WpPost>()
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Post_Title))
                .ForMember(dest => dest.PostName, opt => opt.MapFrom(src => src.Post_Name))
                .ForMember(dest => dest.PostDate, opt => opt.MapFrom(src => src.Post_Date))
                .ForMember(dest => dest.PostDateGmt, opt => opt.MapFrom(src => src.Post_Date_Gmt))
                .ForMember(dest => dest.PostAuthor, opt => opt.MapFrom(src => src.Post_Author));

            CreateMap<WpPost, WP_POST_MEDIA_ADD>()
                .ForMember(dest => dest.Post_Title, opt => opt.MapFrom(src => src.PostTitle))
                .ForMember(dest => dest.Post_Name, opt => opt.MapFrom(src => src.PostName))
                .ForMember(dest => dest.Post_Date, opt => opt.MapFrom(src => src.PostDate))
                .ForMember(dest => dest.Post_Date_Gmt, opt => opt.MapFrom(src => src.PostDateGmt))
                .ForMember(dest => dest.Post_Author, opt => opt.MapFrom(src => src.PostAuthor));

            CreateMap<WpComment, EditCommentDto>()
                .ForMember(dest => dest.CommentId, opt => opt.Ignore());

            CreateMap<EditCommentDto, WpComment>()
                .ForMember(dest => dest.CommentId, opt => opt.Ignore());

            CreateMap<WpCommentDto, WpComment>();
            CreateMap<WpComment, WpCommentDto>();



        }
    }
}
