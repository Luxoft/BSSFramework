using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL.Security;
using Framework.Notification;
using Framework.Configuration.Domain;
using Framework.Core.MessageSender;
using Framework.Core.TypeResolving;
using Framework.DomainDriven.Lock;
using Framework.Events;
using Framework.Persistent;
using Framework.Tracking;

using SecuritySystem.Notification;

using PersistentDomainObjectBase = Framework.Configuration.Domain.PersistentDomainObjectBase;

namespace Framework.Configuration.BLL;

public partial interface IConfigurationBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITypeResolverContainer<string>,

    ITrackingServiceContainer<PersistentDomainObjectBase>
{
    INotificationPrincipalExtractor<Principal> NotificationPrincipalExtractor { get; }

    IDomainObjectEventMetadata EventOperationSource { get; }

    INamedLockService NamedLockService { get; }

    IEmployeeSource EmployeeSource { get; }

    IMessageSender<MessageTemplateNotification> SubscriptionSender { get; }

    bool SubscriptionEnabled { get; }

    ISerializerFactory<string> SystemConstantSerializerFactory { get; }

    ITypeResolver<string> SystemConstantTypeResolver { get; }

    ITypeResolver<DomainType> ComplexDomainTypeResolver { get; }

    DomainType? GetDomainType(Type type, bool throwOnNotFound);

    DomainType? GetDomainType(IDomainType type, bool throwOnNotFound = true);

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
