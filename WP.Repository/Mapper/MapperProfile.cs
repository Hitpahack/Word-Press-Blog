using AutoMapper;

namespace WP.Repository.Mapper
{

    public class MapperProfile : Profile
    {
        // Generic constructor for mapping TEntity to TResult
        public MapperProfile()
        {
            // This is just an example of generic map configuration
            // In real scenarios, you could conditionally create maps based on your needs
        }

        // Generic method for creating mappings
        public void CreateGenericMap<TEntity, TResult>()
        {
            CreateMap<TEntity, TEntity>();
            CreateMap<TResult, TResult>();
        }
    }
}
