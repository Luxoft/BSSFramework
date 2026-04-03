using Framework.BLL.DTOMapping.Domain;

namespace Framework.Infrastructure.SubscriptionService;

/// <summary>
/// Костыль для выноса всех зависимостей от WCF
/// Является копией интерфейса IDefaultActiveSubscriptionService
/// </summary>
public interface IStandardSubscriptionService
{
    void ProcessChanged(ObjectModificationInfoDTO<Guid> changedObjectInfo);
}
