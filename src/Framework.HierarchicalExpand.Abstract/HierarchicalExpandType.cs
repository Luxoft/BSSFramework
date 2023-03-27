namespace Framework.HierarchicalExpand;

/// <summary>
/// Направление разворачивания иерархических объектов
/// </summary>
[Flags]
public enum HierarchicalExpandType
{
    /// <summary>
    /// Происходит выгрузка только самого стартового объекта
    /// </summary>
    None = 0,

    /// <summary>
    /// Происходит выгрузка стартового объекта и всех его родителей
    /// </summary>
    Parents = 1,

    /// <summary>
    /// Происходит выгрузка стартового объекта и всех его детей
    /// </summary>
    Children = 2,

    /// <summary>
    /// Выгружаются сам стартовый объект, его родители и дети
    /// </summary>
    All = Parents + Children
}
