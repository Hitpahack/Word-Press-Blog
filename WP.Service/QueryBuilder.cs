using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq.Expressions;

namespace WP.Service
{

    public class QueryBuilder<TEntity>
    {
        private IQueryable<TEntity> _query;

        public QueryBuilder(IQueryable<TEntity> query)
        {
            _query = query;
        }

        // Apply filter to the query
        public QueryBuilder<TEntity> ApplyFilter(Expression<Func<TEntity, bool>> filter)
        {
            _query = _query.Where(filter);
            return this;
        }

        // Apply join to the query
        public QueryBuilder<TEntity> ApplyJoin<TJoin, TKey>(
            IQueryable<TJoin> inner,
            Expression<Func<TEntity, TKey>> outerKeySelector,
            Expression<Func<TJoin, TKey>> innerKeySelector,
            Expression<Func<TEntity, TJoin, TEntity>> resultSelector)
        {
            _query = _query.Join(inner, outerKeySelector, innerKeySelector, resultSelector);
            return this;
        }

        // Build the final query
        public IQueryable<TEntity> Build() => _query;
    }



    public class QueryBuilder2<T1, T2, TResult>
    {
        private IQueryable _query;
        Func<T1, T2, TResult> selector = 
        Expression<Func<T1, T2, bool>> =


        public QueryBuilder2()
        {
            
        }

        // Apply filter to the query
        public QueryBuilder2<T1, T2, TResult> ApplyFilter(Expression<Func<T1, T2, bool>> filter)
        {
            _query = _query.Where(filter);
            return this;
        }

        // Apply join to the query
        public QueryBuilder2<TResult> ApplyJoin<TEntity, TJoin, TKey>(
            IQueryable<TEntity> entity,
            IQueryable<TJoin> inner,
            Expression<Func<TEntity, TKey>> outerKeySelector,
            Expression<Func<TJoin, TKey>> innerKeySelector,
            Expression<Func<TEntity, TJoin, TResult>> resultSelector)
        {
            _query = entity.Join(inner, outerKeySelector, innerKeySelector, resultSelector);
            return this;
        }

        // Build the final query
        public IQueryable<TResult> Build() => _query;
    }


}
