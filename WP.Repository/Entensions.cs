using AutoMapper;

namespace WP.Repository
{
    public static class Entensions
    {
        public static TResult GenericMap<TResult, TEntity>(this IQueryable<TEntity> query, IMapper mapper)
        {
            if (query == null || !query.Any())
            {
                return default(TResult); // Return default if query is empty or null
            }

            // Map from TEntity to TResult
            return mapper.Map<TResult>(query.FirstOrDefault());
        }
        public static TResult GenericMap<TResult, TEntity>(this TEntity query, IMapper mapper)
        {
            if (query == null)
            {
                return default(TResult); // Return default if query is empty or null
            }

            // Map from TEntity to TResult
            return mapper.Map<TResult>(query);
        }
    }

}
