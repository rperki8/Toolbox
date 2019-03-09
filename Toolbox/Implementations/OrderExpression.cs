using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Toolbox.Interfaces;

namespace Toolbox.Implementations
{
    public class OrderExpression<T, U> : IOrderer<T>
    {
        private bool _descending;
        private Expression<Func<T, U>> _orderExpression;

        public OrderExpression(Expression<Func<T, U>> orderExpression, bool descending)
        {
            _descending = descending;
            _orderExpression = orderExpression;
        }

        public IOrderedQueryable<T> Order(IQueryable<T> query, bool initialOrdering = false)
        {
            var orderedSource = query as IOrderedQueryable<T>;

            if (initialOrdering || orderedSource == null)
            {
                return _descending
                            ? query.OrderByDescending(_orderExpression)
                            : query.OrderBy(_orderExpression);
            }
            else
            {
                return _descending
                        ? orderedSource.ThenByDescending(_orderExpression)
                        : orderedSource.ThenBy(_orderExpression);
            }
        }
    }
}
