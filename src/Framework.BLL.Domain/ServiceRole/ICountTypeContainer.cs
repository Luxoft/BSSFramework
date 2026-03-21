namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Контейнер маркировки количественной обработки
/// </summary>
public interface ICountTypeContainer
{
    /// <summary>
    /// Обработка единственного объекта и/или коллекции объектов
    /// </summary>
    CountType CountType { get; }
}
