using System.Runtime.Serialization;

namespace Framework.OData;

[DataContract(Name = "SelectOperationResultOf{0}")]
public class SelectOperationResult<T> : ISelectOperationResult<T>
{
    public SelectOperationResult(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        this.Items = items.ToList();
        this.TotalCount = this.Items.Count;
    }

    public SelectOperationResult(IEnumerable<T> items, int totalCount)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        if (totalCount < 0) throw new ArgumentOutOfRangeException(nameof(totalCount));

        this.Items = items.ToList();
        this.TotalCount = totalCount;
    }

    [DataMember]
    public List<T> Items { get; private set; }

    [DataMember]
    public int TotalCount { get; private set; }

    #region ISelectOperationResult Members

    Type ISelectOperationResult.ElementType { get; } = typeof(T);

    IEnumerable<T> ISelectOperationResult<T>.Items => this.Items;

    #endregion
}
