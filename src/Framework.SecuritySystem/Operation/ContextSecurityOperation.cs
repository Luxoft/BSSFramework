using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

/// <summary>
/// Констектстная операция доступа
/// </summary>
public abstract record ContextSecurityOperation(string Name, HierarchicalExpandType ExpandType) : SecurityOperation(Name)
{
}

public record ContextSecurityOperation<TIdent>(string Name, HierarchicalExpandType ExpandType, TIdent Id) : ContextSecurityOperation(Name, ExpandType)
{
}
