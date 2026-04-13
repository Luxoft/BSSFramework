using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface IDomainTypeBLL
{
    Task ForceEventAsync(DomainTypeEventModel eventModel, CancellationToken cancellationToken);
}
