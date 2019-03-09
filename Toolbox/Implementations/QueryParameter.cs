using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Toolbox.Interfaces;

namespace Toolbox.Implementations
{
    public class QueryParameter<T> : QueryParameter<T, T> where T : class
    {
    }

    public class QueryParameter<T, U> where T : class
    {
        public QueryParameter()
        {
            Includes = new List<Expression<Func<T, object>>>();
            OrderExpressions = new List<IOrderer<T>>();
        }

        public Expression<Func<T, bool>> WhereClause { get; protected set; }
        public Expression<Func<T, U>> Projection { get; protected set; }
        public List<Expression<Func<T, object>>> Includes { get; protected set; }
        public List<IOrderer<T>> OrderExpressions { get; protected set; }

        public int? PageSize { get; protected set; }
        public int PageIndex { get; protected set; } = 0;
        public int Skip => PageIndex * (PageSize ?? 0);
        public bool IsDistinct { get; protected set; }

        public QueryParameter<T, U> Where(Expression<Func<T, bool>> whereClause)
        {
            if (whereClause == null)
                throw new NullReferenceException("whereClause");

            if (WhereClause == null)
            {
                WhereClause = whereClause;
            }
            else
            {
                var parameter = whereClause.Parameters[0];
                var body = Expression.AndAlso(WhereClause.Body, whereClause.Body);
                WhereClause = Expression.Lambda<Func<T, bool>>(body, parameter);
            }

            return this;
        }

        public QueryParameter<T, U> Include(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
            return this;
        }

        public QueryParameter<T, U> OrderBy<V>(Expression<Func<T, V>> orderBy, bool descending = false)
        {
            OrderExpressions.Add(new OrderExpression<T, V>(orderBy, descending));
            return this;
        }

        public QueryParameter<T, U> OrderByDescending<V>(Expression<Func<T, V>> orderBy) => OrderBy(orderBy, true);

        public QueryParameter<T, U> ThenBy<V>(Expression<Func<T, V>> orderBy, bool descending = false) => OrderBy(orderBy, descending);

        public QueryParameter<T, U> ThenByDescending<V>(Expression<Func<T, V>> orderBy) => ThenBy(orderBy, true);

        public QueryParameter<T, U> Take(int count)
        {
            PageSize = count;
            return this;
        }

        public QueryParameter<T, U> Page(int pageIndex)
        {
            PageIndex = pageIndex;
            return this;
        }

        public QueryParameter<T, U> Select(Expression<Func<T, U>> selector)
        {
            Projection = selector;
            return this;
        }

        public QueryParameter<T, U> Distinct()
        {
            IsDistinct = true;
            return this;
        }

        public static QueryParameter<T, U> Create()
        {
            return new QueryParameter<T, U>();
        }

    }
}
