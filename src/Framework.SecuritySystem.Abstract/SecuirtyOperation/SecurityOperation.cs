using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

/// <summary>
/// Операция доступа
/// </summary>
public record SecurityOperation(string Name)
{
    /// <summary>
    /// Тип разворачивания деревьев (как правило для операции просмотра самого дерева выбирается HierarchicalExpandType.All)
    /// </summary>
    public HierarchicalExpandType ExpandType { get; init; } = HierarchicalExpandType.Children;

    public string Description { get; init; }

    public sealed override string ToString() => this.Name;
}
