using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

/// <summary>
/// Операция доступа
/// </summary>
public abstract record SecurityOperation(string Name)
{
    /// <summary>
    /// Тип разворачивания деревьев (как правило для операции просмотра самого дерева выбирается HierarchicalExpandType.All)
    /// </summary>
    public HierarchicalExpandType ExpandType { get; init; } = HierarchicalExpandType.Children;

    public string Description { get; init; }

    public bool AdminHasAccess { get; init; } = true;

    public bool IsClient { get; init; }

    public SecurityOperation ApproveOperation { get; init; }

    public sealed override string ToString() => this.Name;

    public static DisabledSecurityOperation Disabled { get; } = new ();
}

public record SecurityOperation<TIdent>(string Name, TIdent Id) : SecurityOperation(Name)
{
}
