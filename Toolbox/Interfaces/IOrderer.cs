using System;
using System.Linq;

namespace Toolbox.Interfaces
{
    public interface IOrderer<T>
    {
        IOrderedQueryable<T> Order(IQueryable<T> source, bool initialOrdering = false);
    }
}
