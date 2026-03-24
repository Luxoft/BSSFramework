namespace Framework.Application.Session.DALObject;

/// <summary>
/// For proxy types
/// </summary>
public interface IdalObject
{
    object Object { get; }

    Type Type { get; }

    /// <summary>
    /// Числовой идентификатор, определающий порядок применения/изменения/получения данной сущности
    /// </summary>
    long ApplyIndex { get; }
}
