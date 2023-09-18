namespace Framework.HierarchicalExpand;

/// <summary>
/// Интерфейс для получнения реального разворачиваемого типа
/// </summary>
public interface IRealTypeResolver
{
    Type Resolve(Type identity);
}
