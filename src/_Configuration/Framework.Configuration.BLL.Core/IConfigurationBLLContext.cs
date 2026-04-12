using Framework.Application.Events;
using Framework.Application.Lock;
using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.BLL;
using Framework.BLL.Domain.IdentityObject;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.Core.TypeResolving;
using Framework.Tracking;
using Framework.Validation;

using SecuritySystem.Notification;

using PersistentDomainObjectBase = Framework.Configuration.Domain.PersistentDomainObjectBase;

namespace Framework.Configuration.BLL;

public partial interface IConfigurationBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITypeResolverContainer<string>,

    ITrackingServiceContainer<PersistentDomainObjectBase>
{
    IValidator Validator { get; }

    INotificationPrincipalExtractor<Principal> NotificationPrincipalExtractor { get; }

    IDomainObjectEventMetadata EventOperationSource { get; }

    INamedLockService NamedLockService { get; }

    ISerializerFactory<string> SystemConstantSerializerFactory { get; }

    ITypeResolver<string> SystemConstantTypeResolver { get; }

    ITypeResolver<TypeNameIdentity> ComplexDomainTypeResolver { get; }

    DomainType GetDomainType(Type type);

    DomainType? TryGetDomainType(Type type);

    ITargetSystemService GetTargetSystemService(TargetSystem targetSystem);

    ITargetSystemService GetTargetSystemService(Type domainObjectType);

    IEnumerable<ITargetSystemService> GetTargetSystemServices();

    /// <summary>
    /// Получение текущей ревизии из аудита (пока возвращает 0, если вызван до флаша сессии)
    /// </summary>
    /// <returns></returns>
    long GetCurrentRevision();
}
