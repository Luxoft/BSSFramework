using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.OData
{
    public interface ISelectOperationResult
    {
        int TotalCount { get; }

        Type ElementType { get; }
    }

    public interface ISelectOperationResult<out T> : ISelectOperationResult
    {
        IEnumerable<T> Items { get; }
    }
}