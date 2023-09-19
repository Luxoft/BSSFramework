namespace Framework.HierarchicalExpand;

/// <summary>
/// Интерфейс для получнения реального типа
/// </summary>
public interface IRealTypeResolver
{
    Type Resolve(Type identity);
}
