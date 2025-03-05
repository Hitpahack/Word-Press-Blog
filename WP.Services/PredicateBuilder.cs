using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Services
{
    using System;
    using System.Linq.Expressions;

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() => param => true;

        public static Expression<Func<T, bool>> False<T>() => param => false;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            if (first == null) return second;
            if (second == null) return first;

            var invokedExpression = Expression.Invoke(second, first.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(first.Body, invokedExpression), first.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            if (first == null) return second;
            if (second == null) return first;

            var invokedExpression = Expression.Invoke(second, first.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(first.Body, invokedExpression), first.Parameters);
        }
    }

}
