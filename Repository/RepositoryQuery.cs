using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace WP.Repository
{
    public sealed class RepositoryQuery<TEntity> where TEntity : class, new()
    {
        private readonly Repository<TEntity> _repository;
        private Expression<Func<TEntity, bool>> _filter;
        private Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> _orderByQuerable;
        private Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>> _includeProperties;
        private int? _page;
        private int? _pageSize;
        private bool _trackingEnabled;

        public RepositoryQuery(Repository<TEntity> repository)
        {
            _repository = repository;
            _trackingEnabled = true;
        }

        public RepositoryQuery<TEntity> Filter(
            Expression<Func<TEntity, bool>> filter)
        {
            _filter = filter;
            return this;
        }

        public RepositoryQuery<TEntity> AsNoTracking()
        {
            _trackingEnabled = false;
            return this;
        }

        public RepositoryQuery<TEntity> OrderBy(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        public RepositoryQuery<TEntity> Includes(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
        {
            _includeProperties = includeProperties;
            return this;
        }

        public IEnumerable<TEntity> GetPage(
            int page, int pageSize, out int totalCount)
        {
            _page = page;
            _pageSize = pageSize;
            totalCount = _repository.GetAll(_filter).Count();

            return _repository.Get(
                _filter,
                _trackingEnabled,
                _orderByQuerable,
                _includeProperties,
                _page,
                _pageSize);
        }

        public IEnumerable<TEntity> Get()
        {
            return _repository.Get(
                _filter,
                _trackingEnabled,
                _orderByQuerable,
                _includeProperties,
                _page,
                _pageSize);
        }

        public IQueryable<TEntity> GetQuerable()
        {
            return _repository.Get(
                _filter,
                _trackingEnabled,
                _orderByQuerable,
                _includeProperties,
                _page,
                _pageSize);
        }
    }
}
