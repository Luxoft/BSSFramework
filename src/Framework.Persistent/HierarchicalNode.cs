namespace Framework.Persistent;

public record HierarchicalNode<TValue, TIdent>(TValue Item, TIdent ParentId, bool OnlyView)
{
    public HierarchicalNode<TNewItem, TIdent> ChangeItem<TNewItem>(Func<TValue, TNewItem> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new HierarchicalNode<TNewItem, TIdent>(Item: selector(this.Item), OnlyView: this.OnlyView, ParentId: this.ParentId);
    }
}
