using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

/// <summary>
/// Констектстная операция доступа
/// </summary>
/// <param name="Name">Имя операции</param>
/// <param name="ExpandType">Тип разворачивания деревьев (как правило для операции просмотра самого дерева выбирается HierarchicalExpandType.All)</param>
public abstract record ContextSecurityOperation(string Name, HierarchicalExpandType ExpandType) : SecurityOperation(Name)
{
    public abstract NonContextSecurityOperation ToNonContext();
}

/// <summary>
/// Констектстная операция доступа
/// </summary>
/// <typeparam name="TIdent"></typeparam>
/// <param name="Name">Имя операции</param>
/// <param name="ExpandType">Тип разворачивания деревьев (как правило для операции просмотра самого дерева выбирается HierarchicalExpandType.All)</param>
/// <param name="Id"></param>
public record ContextSecurityOperation<TIdent>(string Name, HierarchicalExpandType ExpandType, TIdent Id) : ContextSecurityOperation(Name, ExpandType), ISecurityOperation<TIdent>
{
    public override NonContextSecurityOperation ToNonContext() => new NonContextSecurityOperation<TIdent>(this.Name, this.Id);
}
