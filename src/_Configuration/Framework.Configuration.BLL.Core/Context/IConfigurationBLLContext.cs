using Framework.Authorization.BLL;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;

using Framework.Notification;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Lock;
using Framework.Events;
using Framework.Persistent;

namespace Framework.Configuration.BLL;

public partial interface IConfigurationBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITypeResolverContainer<string>,

    ITrackingServiceContainer<PersistentDomainObjectBase>
{
    IDomainObjectEventMetadata EventOperationSource { get; }

    INamedLockService NamedLockService { get; }

    IEmployeeSource EmployeeSource { get; }

    IMessageSender<MessageTemplateNotification> SubscriptionSender { get; }

    bool SubscriptionEnabled { get; }

    ISerializerFactory<string> SystemConstantSerializerFactory { get; }

    ITypeResolver<DomainType> ComplexDomainTypeResolver { get; }

    DomainType GetDomainType(Type type, bool throwOnNotFound);

    DomainType GetDomainType(IDomainType type, bool throwOnNotFound = true);

    ISubscriptionSystemService GetSubscriptionSystemService(Type domainType);

    ITargetSystemService GetTargetSystemService(TargetSystem targetSystem);

    ITargetSystemService GetTargetSystemService(Type domainType, bool throwOnNotFound);

    ITargetSystemService GetTargetSystemService(string name);

    TargetSystemInfo GetTargetSystemInfo(Type domainType);

    DomainTypeInfo GetDomainTypeInfo(Type domainType);

    ITargetSystemService GetMainTargetSystemService();

    IEnumerable<ITargetSystemService> GetTargetSystemServices();

    /// <summary>
    /// Получение текущей ревизии из аудита (пока возвращает 0, если вызван до флаша сессии)
    /// </summary>
    /// <returns></returns>
    long GetCurrentRevision();
}
