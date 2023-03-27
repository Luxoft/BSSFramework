using Framework.DomainDriven.ServiceModel.Subscriptions;

namespace Framework.DomainDriven.ServiceModel.IAD;

/// <summary>
/// Костыль для выноса всех зависимостей от WCF из сборки Framework.DomainDriven.ServiceModel.IAD
/// Является копией интерфейса IDefaultActiveSubscriptionService
/// </summary>
public interface IStandardSubscriptionService
{
    void ProcessChanged(ObjectModificationInfoDTO<Guid> changedObjectInfo);
}
