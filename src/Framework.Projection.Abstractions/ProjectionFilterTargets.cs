namespace Framework.Projection;

/// <summary>
/// Применимость фильтра проекций
/// </summary>
[Flags]
public enum ProjectionFilterTargets
{
    /// <summary>
    /// Для выгрузки обычных коллекций
    /// </summary>
    Collection = 1,

    /// <summary>
    /// Для выгрузки OData
    /// </summary>
    OData = 2,

    /// <summary>
    /// Для выгрузки единственного экземпляра
    /// </summary>
    Single = 4,

    /// <summary>
    /// Для выгрузки дерева
    /// </summary>
    ODataTree = 8,

    /// <summary>
    /// Все варианты применимости фильтра
    /// </summary>
    All = Collection + OData + Single + ODataTree
}
