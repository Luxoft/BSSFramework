using Framework.BLL.DTOMapping.Domain;

namespace Framework.Infrastructure.SubscriptionService;

public interface IObjectModificationProcessor
{
    Task ProcessChanged(ObjectModificationInfoDTO<Guid> changedObjectInfo, CancellationToken cancellationToken);
}
