using Framework.Core;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Атрибут для удаления объекта через интеграцию
/// </summary>
public class BLLIntegrationRemoveRoleAttribute : BLLServiceRoleAttribute, ICountTypeContainer
{
    /// <summary>
    /// Удаление единственного объекта и/или коллекции объектов
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
