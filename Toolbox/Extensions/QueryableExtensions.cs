using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Interfaces;

namespace Toolbox.Extensions
{
    public static class QueryableExtensions
    {
        public static int Count<T, U>(this IQueryable<T> queryable, Expression<Func<T, bool>> queryExpression) where T : class
        {
            return queryable.Count(queryExpression);
        }

        public static U FirstOrDefault<T, U>(this IQueryable<T> queryable, IQueryParameter<T, U> queryParameter) where T : class
        {
            return queryable.GetQueryable(queryParameter).FirstOrDefault();
        }

        public static List<U> Get<T, U>(this IQueryable<T> queryable, IQueryParameter<T, U> queryParameter) where T : class
        {
            return queryable.GetQueryable(queryParameter).ToList();
        }

        public static Task<int> CountAsync<T, U>(this IQueryable<T> queryable, Expression<Func<T, bool>> queryExpression) where T : class => queryable.CountAsync(queryExpression);

        public static Task<U> FirstOrDefaultAsync<T, U>(this IQueryable<T> queryable, IQueryParameter<T, U> queryParameter) where T : class
        {
            return queryable.GetQueryable(queryParameter).FirstOrDefaultAsync();
        }

        public static Task<List<U>> GetAsync<T, U>(this IQueryable<T> queryable, IQueryParameter<T, U> queryParameter) where T : class
        {
            return queryable.GetQueryable(queryParameter).ToListAsync();
        }

        private static IQueryable<U> GetQueryable<T, U>(this IQueryable<T> queryable, IQueryParameter<T, U> queryParameter) where T : class
        {
            var query = queryParameter.Includes.Aggregate(
                queryable.Where(queryParameter.WhereClause ?? ((x) => true)),
            (current, include) => current.Include(include)).AsQueryable();

            var isOrdered = queryParameter.OrderExpressions.Any();

            if (isOrdered)
            {
                for (var i = 0; i < queryParameter.OrderExpressions.Count; i++)
                    query = queryParameter.OrderExpressions[i].Order(query, i == 0);
            }

            if (queryParameter.PageSize.HasValue)
            {
                if (!isOrdered)
                    query = query.OrderBy(x => 1);

                query = query
                    .Skip(queryParameter.Skip)
                    .Take(queryParameter.PageSize.Value);

                //query = query
                //    .Skip(() => queryParameter.Skip)
                //    .Take(() => queryParameter.PageSize.Value);
            }

            IQueryable<U> projectedQuery;
            if (queryParameter.Projection != null)
                projectedQuery = query.Select(queryParameter.Projection);
            else
                projectedQuery = query as IQueryable<U>;

            if (queryParameter.IsDistinct)
                projectedQuery = projectedQuery.Distinct();

            return projectedQuery;
        }
    }
}
