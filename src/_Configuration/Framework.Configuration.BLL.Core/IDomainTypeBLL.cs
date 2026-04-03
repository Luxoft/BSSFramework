using Framework.BLL;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface IDomainTypeBLL : IPathBLL<DomainType>
{
    DomainType GetByType(Type domainObjectType);

    void ForceEvent(DomainTypeEventModel eventModel);
}
