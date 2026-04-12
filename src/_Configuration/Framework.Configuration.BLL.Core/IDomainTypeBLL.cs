using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface IDomainTypeBLL
{
    void ForceEvent(DomainTypeEventModel eventModel);
}
