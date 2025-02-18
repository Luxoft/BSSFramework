using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;

namespace Framework.Configuration.BLL;

public partial interface IDomainTypeBLL : IPathBLL<DomainType>
{
    DomainType GetByType(Type domainObjectType);

    void ForceEvent(DomainTypeEventModel eventModel);
}
