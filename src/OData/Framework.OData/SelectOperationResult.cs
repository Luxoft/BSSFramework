using System.Collections.Immutable;

namespace Framework.OData;

public record SelectOperationResult<T>(ImmutableArray<T> Items, int TotalCount) : ISelectOperationResult<T>
{
    public SelectOperationResult(ImmutableArray<T> items)
        : this(items, items.Length)
    {
    }

    public SelectOperationResult(IEnumerable<T> items)
        : this([..items])
    {
    }

    public SelectOperationResult(IEnumerable<T> items, int totalCount)
        : this([..items], totalCount)
    {
    }

    #region ISelectOperationResult Members

    Type ISelectOperationResult.ElementType { get; } = typeof(T);

    IReadOnlyList<T> ISelectOperationResult<T>.Items => this.Items;

    #endregion
}
