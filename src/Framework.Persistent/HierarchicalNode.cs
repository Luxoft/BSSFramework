using System.Runtime.Serialization;

namespace Framework.Persistent;

[DataContract(Name = "HierarchicalNodeOf{0}Of{1}", Namespace = "Framework.Persistent")]
public struct HierarchicalNode<TValue, TIdent>
        where TValue : IIdentityObject<TIdent>
{
    [DataMember]
    public TValue Item { get; set; }

    [DataMember]
    public TIdent ParentId { get; set; }

    [DataMember]
    public bool OnlyView { get; set; }

    public HierarchicalNode<TNewItem, TIdent> ChangeItem<TNewItem>(Func<TValue, TNewItem> selector)
            where TNewItem : IIdentityObject<TIdent>
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new HierarchicalNode<TNewItem, TIdent> { Item = selector(this.Item), OnlyView = this.OnlyView, ParentId = this.ParentId };
    }
}
