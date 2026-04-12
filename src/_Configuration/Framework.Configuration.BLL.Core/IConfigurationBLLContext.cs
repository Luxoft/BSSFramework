using Framework.Application.Events;
using Framework.Application.Lock;
using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.Core.TypeResolving;
using Framework.Tracking;
using Framework.Validation;

using PersistentDomainObjectBase = Framework.Configuration.Domain.PersistentDomainObjectBase;

namespace Framework.Configuration.BLL;

public partial interface IConfigurationBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITrackingServiceContainer<PersistentDomainObjectBase>
{
    IValidator Validator { get; }

    IDomainObjectEventMetadata EventOperationSource { get; }

    INamedLockService NamedLockService { get; }

    ISerializerFactory<string> SystemConstantSerializerFactory { get; }

    ITypeResolver<TypeNameIdentity> TargetSystemTypeResolver { get; }

    DomainType GetDomainType(Type type);

    DomainType? TryGetDomainType(Type type);

    ITargetSystemService GetTargetSystemService(TargetSystem targetSystem);

    ITargetSystemService GetTargetSystemService(Type domainObjectType);

    IEnumerable<ITargetSystemService> GetTargetSystemServices();
}
