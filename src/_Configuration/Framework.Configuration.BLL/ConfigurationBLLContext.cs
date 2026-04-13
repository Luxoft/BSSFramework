using System.Collections.Frozen;
using System.Collections.Immutable;

using CommonFramework;

using Framework.Application.Events;
using Framework.Application.Lock;
using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.BLL.Domain.Exceptions;
using Framework.BLL.Domain.TargetSystem;
using Framework.BLL.Services;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.Core.TypeResolving;
using Framework.Tracking;
using Framework.Validation;

using HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.AccessDenied;

using PersistentDomainObjectBase = Framework.Configuration.Domain.PersistentDomainObjectBase;

namespace Framework.Configuration.BLL;

public partial class ConfigurationBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices(nameof(BLL))] IEventOperationSender operationSender,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IServiceProxyFactory serviceProxyFactory,
    ITargetSystemTypeResolverContainer targetSystemTypeResolverContainer,
    ITrackingService<PersistentDomainObjectBase> trackingService,
    IConfigurationValidator validator,
    IRootSecurityService securityService,
    IConfigurationBLLFactoryContainer logics,
    IAuthorizationBLLContext authorization,
    IDomainObjectEventMetadata eventOperationSource,
    INamedLockService namedLockService,
    IEnumerable<PersistentTargetSystemInfo> targetSystemInfoList,
    [FromKeyedServices(nameof(SystemConstant))]
    ISerializerFactory<string> systemConstantSerializerFactory) :

    SecurityBLLBaseContext<PersistentDomainObjectBase, Guid, IConfigurationBLLFactoryContainer>(
    serviceProvider,
    operationSender,
    accessDeniedExceptionService,
    hierarchicalObjectExpanderFactory)
{
    public ITrackingService<PersistentDomainObjectBase> TrackingService { get; } = trackingService;

    public IRootSecurityService SecurityService { get; } = securityService;

    public ITypeResolver<TypeNameIdentity> TargetSystemTypeResolver => targetSystemTypeResolverContainer.TypeResolver;


    public override IConfigurationBLLFactoryContainer Logics { get; } = logics;

    public IValidator Validator { get; } = validator;

    public IAuthorizationBLLContext Authorization { get; } = authorization;

    public ISerializerFactory<string> SystemConstantSerializerFactory { get; } = systemConstantSerializerFactory;

    public INamedLockService NamedLockService { get; } = namedLockService;

    public IDomainObjectEventMetadata EventOperationSource { get; } = eventOperationSource;

    public FrozenDictionary<PersistentTargetSystemInfo, ITargetSystemService> TargetSystemServices =>

        field ??= targetSystemInfoList.Join(
                                          this.TargetSystems,
                                          tsi => tsi.Id,
                                          ts => ts.Id,
                                          (tsi, ts) =>
                                          {
                                              var targetSystemServiceType = typeof(TargetSystemService<,>).MakeGenericType(tsi.BllContextType, tsi.PersistentDomainObjectBaseType);

                                              return (tsi, serviceProxyFactory.Create<ITargetSystemService>(targetSystemServiceType, tsi, ts));
                                          })
                                      .ToFrozenDictionary(pair => pair.tsi, pair => pair.Item2);

    private ImmutableList<TargetSystem> TargetSystems => field ??= [.. this.Logics.TargetSystem.GetUnsecureQueryable()];

    private FrozenDictionary<TypeNameIdentity, DomainType> DomainTypeDict => field ??= this.TargetSystems
                                                                                           .SelectMany(ts => ts.DomainTypes)
                                                                                           .ToFrozenDictionary(dt => (TypeNameIdentity)dt);

    public DomainType? TryGetDomainType(TypeNameIdentity typeNameIdentity) => this.DomainTypeDict.GetValueOrDefault(typeNameIdentity);

    public DomainType GetDomainType(TypeNameIdentity typeNameIdentity) =>

        this.TryGetDomainType(typeNameIdentity) ?? throw new BusinessLogicException("TargetSystem with domainType \"{0}\" not found", typeNameIdentity);
}
