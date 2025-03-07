namespace SSM.Rerpository
{
    public static class OrderByExtensions
    {
        //public static IOrderedQueryable<T> OrderBy<T>(
        //    this IQueryable<T> source,
        //    string property)
        //{
        //    if (property.IndexOf(',') != -1)
        //    {
        //        return OrderBy<T>(
        //               source,
        //               property.Split(','));
        //    }
        //    else if (property.StartsWith("-"))
        //    {
        //        return ApplyOrder<T>(
        //               source,
        //               property.Substring(1).Trim(),
        //               "OrderByDescending");
        //    }
        //    else
        //    {
        //        return ApplyOrder<T>(
        //               source,
        //               property,
        //               "OrderBy");
        //    }
        //}

        //public static IOrderedQueryable<T> OrderBy<T>(this string property, IQueryable<T> source) => source.OrderBy(property);


        //public static IOrderedQueryable<T> OrderByDescending<T>(
        //    this IQueryable<T> source,
        //    string property)
        //{
        //    return ApplyOrder<T>(
        //           source,
        //           property,
        //           "OrderByDescending");
        //}

        //public static IOrderedQueryable<T> OrderByDescending<T>(this string property, IQueryable<T> source) => source.OrderByDescending(property);

        //public static IOrderedQueryable<T> ThenBy<T>(
        //    this IOrderedQueryable<T> source,
        //    string property)
        //{
        //    return ApplyOrder<T>(
        //           source,
        //           property,
        //           "ThenBy");
        //}

        //public static IOrderedQueryable<T> ThenBy<T>(this string property, IOrderedQueryable<T> source) => source.ThenBy(property);

        //public static IOrderedQueryable<T> ThenByDescending<T>(
        //    this IOrderedQueryable<T> source,
        //    string property)
        //{
        //    return ApplyOrder<T>(
        //           source,
        //           property,
        //           "ThenByDescending");
        //}

        //public static IOrderedQueryable<T> ThenByDescending<T>(this string property, IOrderedQueryable<T> source) => source.ThenByDescending(property);

        //static IOrderedQueryable<T> ApplyOrder<T>(
        //    IQueryable<T> source,
        //    string property,
        //    string methodName)
        //{
        //    string[] props = property.Split('.');
        //    Type type = typeof(T);
        //    ParameterExpression arg = Expression.Parameter(type, "x");
        //    Expression expr = arg;

        //    foreach (string prop in props)
        //    {
        //        // use reflection (not ComponentModel) to mirror LINQ
        //        PropertyInfo pi = type.GetProperty(
        //                prop,
        //                (BindingFlags.NonPublic
        //                 | BindingFlags.Public
        //                 | BindingFlags.Instance));

        //        // raise error if property is not found
        //        if (pi == null)
        //        {
        //            throw new ArgumentException(string.Format(
        //                "Sort Error. Property '{0}' not found on type {1}",
        //                prop, type.FullName));
        //        }

        //        expr = Expression.Property(expr, pi);
        //        type = pi.PropertyType;
        //    }

        //    Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);

        //    LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

        //    object result = typeof(Queryable)
        //                       .GetMethods()
        //                       .Single(method => method.Name == methodName
        //                           && method.IsGenericMethodDefinition
        //                           && method.GetGenericArguments().Length == 2
        //                           && method.GetParameters().Length == 2)
        //                       .MakeGenericMethod(typeof(T), type)
        //                       .Invoke(null, new object[] { source, lambda });

        //    return (IOrderedQueryable<T>)result;
        //}

        //public static IOrderedQueryable<T> OrderBy<T>(
        //    this IQueryable<T> source,
        //    IEnumerable<string> properties)
        //{
        //    // iterate the properties and sort
        //    IOrderedQueryable<T> sortedData = null;

        //    foreach (string prop in properties)
        //    {
        //        string propName = prop.Trim();
        //        bool ascending = true;

        //        if (string.IsNullOrEmpty(propName))
        //        {
        //            continue;
        //        }

        //        if (propName.StartsWith("-"))
        //        {
        //            ascending = false;
        //            propName = prop.Substring(1).Trim();
        //        }

        //        // first iteration
        //        if (sortedData == null)
        //        {
        //            if (ascending)
        //            {
        //                sortedData = source.AsQueryable().OrderBy(propName);
        //            }
        //            else
        //            {
        //                sortedData = source.AsQueryable().OrderByDescending(propName);
        //            }
        //        }
        //        else
        //        {
        //            // subsequent iterations...
        //            if (ascending)
        //            {
        //                sortedData = sortedData.ThenBy(propName);
        //            }
        //            else
        //            {
        //                sortedData = sortedData.ThenByDescending(propName);
        //            }
        //        }
        //    }

        //    return sortedData;
        //}

        //public static IOrderedQueryable<T> OrderBy<T>(this IEnumerable<string> properties, IQueryable<T> source) => OrderBy(source, properties);

    }

   



}
