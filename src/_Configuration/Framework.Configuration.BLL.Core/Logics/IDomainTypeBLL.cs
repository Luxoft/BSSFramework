using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;

namespace Framework.Configuration.BLL;

public partial interface IDomainTypeBLL : IPathBLL<DomainType>
{
    DomainType GetByType(Type domainObjectType);

    /// <summary>
    /// Ручное инициирование события
    /// </summary>
    /// <param name="eventModel">Модель</param>
    void ForceEvent(DomainTypeEventModel eventModel);
}
