namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
///  Атрибут удаления объекта через клиенсткий фасадный слой
/// </summary>
public class BLLRemoveRoleAttribute : BLLServiceRoleAttribute, ICountTypeContainer
{
    /// <summary>
    /// Удаление единственного объекта и/или коллекции объектов
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
