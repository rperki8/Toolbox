using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Toolbox.Interfaces
{
    //public interface IQueryParameter<T> : IQueryParameter<T, T> where T : class
    //{ 
    //}

    public interface IQueryParameter<T, U> where T : class
    {
        Expression<Func<T, bool>> WhereClause { get; }
        Expression<Func<T, U>> Projection { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<IOrderer<T>> OrderExpressions { get; }
        int? PageSize { get; }
        int PageIndex { get; }
        int Skip { get; }
        bool IsDistinct { get; }
    }
}
