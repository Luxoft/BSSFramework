using System.Collections.Frozen;

using Framework.Application.Events;
using Framework.Application.Lock;
using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.BLL.Domain.TargetSystem;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.Core.TypeResolving;
using Framework.Validation;

namespace Framework.Configuration.BLL;

public partial interface IConfigurationBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>
{
    IValidator Validator { get; }

    IDomainObjectEventMetadata EventOperationSource { get; }

    INamedLockService NamedLockService { get; }

    ISerializerFactory<string> SystemConstantSerializerFactory { get; }

    ITypeResolver<TypeNameIdentity> TargetSystemTypeResolver { get; }

    FrozenDictionary<PersistentTargetSystemInfo, ITargetSystemService> TargetSystemServices { get; }

    DomainType GetDomainType(TypeNameIdentity type);

    DomainType? TryGetDomainType(TypeNameIdentity type);
}
