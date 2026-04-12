using System.Collections.Frozen;

using CommonFramework;
using CommonFramework.DictionaryCache;

using Framework.Application.Events;
using Framework.Application.Lock;
using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.BLL;
using Framework.BLL.Domain.Exceptions;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.TargetSystem;
using Framework.BLL.Services;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.Core.TypeResolving;
using Framework.Database;
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
    IEnumerable<ITargetSystemService> targetSystemServices,
    IEnumerable<TargetSystemInfo> targetSystemInfoList,
    ICurrentRevisionService currentRevisionService,
    [FromKeyedServices(nameof(SystemConstant))] ISerializerFactory<string> systemConstantSerializerFactory) : SecurityBLLBaseContext<PersistentDomainObjectBase, Guid, IConfigurationBLLFactoryContainer>(serviceProvider, operationSender, accessDeniedExceptionService, hierarchicalObjectExpanderFactory)
{
    private FrozenDictionary<TargetSystemInfo, ITargetSystemService> TargetSystemServices =>
        field ??= this.Logics.TargetSystem.GetUnsecureQueryable().Select(ts =>
        {
            var

            return (ts, serviceProxyFactory.Create<ITargetSystemService>());
        });

    public ITrackingService<PersistentDomainObjectBase> TrackingService { get; } = trackingService;

    public IRootSecurityService SecurityService { get; } = securityService;

    public ITypeResolver<TypeNameIdentity> TargetSystemTypeResolver => targetSystemTypeResolverContainer.TypeResolver;


    public override IConfigurationBLLFactoryContainer Logics { get; } = logics;

    public IValidator Validator { get; } = validator;

    public IAuthorizationBLLContext Authorization { get; } = authorization;

    public ISerializerFactory<string> SystemConstantSerializerFactory { get; } = systemConstantSerializerFactory;

    public INamedLockService NamedLockService { get; } = namedLockService;

    public IDomainObjectEventMetadata EventOperationSource { get; } = eventOperationSource;

    public DomainType GetDomainType(TypeNameIdentity typeNameIdentity)
    {

    }

    public DomainType? TryGetDomainType(Type type)
    {

    }

    public ITargetSystemService GetTargetSystemService(TargetSystem targetSystem)
    {
        if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

        return this.TargetSystemServices.Values.Single(ts => ts.TargetSystem == targetSystem);
    }

    public ITargetSystemService GetTargetSystemService(Type domainType) =>

        this.lazyTargetSystemServiceCache.Value.Values.Single(
            service => service.IsAssignable(domainType),
            () => new BusinessLogicException($"Target System for type {domainType.Name} not found"),
            () => new BusinessLogicException($"To many Target Systems for type {domainType.Name}"));

    public ITargetSystemService GetTargetSystemService(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return this.lazyTargetSystemServiceCache.Value.Values.GetByName(name);
    }

    public TargetSystemInfo GetTargetSystemInfo(Type domainType) => this.targetSystemInfoDict[domainType];

    public DomainTypeInfo GetDomainTypeInfo(Type domainType) => this.domainTypeInfoDict[domainType];

    public IEnumerable<ITargetSystemService> GetTargetSystemServices() => this.lazyTargetSystemServiceCache.Value.Values;

    public DomainType GetDomainType(Type type)
    {
        var domainType = this.domainTypeCache[type];

        if (domainType == null && throwOnNotFound)
        {
            throw new BusinessLogicException("TargetSystem with domainType \"{0}\" not found", type.FullName);
        }

        return domainType;
    }

    public DomainType GetDomainType(IDomainType type, bool throwOnNotFound = true)
    {
        var domainType = this.domainTypeNameCache[type];

        if (domainType == null && throwOnNotFound)
        {
            throw new BusinessLogicException($"DomainType \"{type}\" not found");
        }

        return domainType;
    }

    private DomainType GetDomainType(ITargetSystemService targetService, Type type)
    {
        var domainTypes = this.Logics.DomainType.GetListBy(domainType => domainType.TargetSystem == targetService.TargetSystem
                                                                         && domainType.Name == type.Name
                                                                         && domainType.Namespace == type.Namespace);

        if (domainTypes.Count > 1)
        {
            var message =
                $"Configuration database contains more than one record for domain type '{domainTypes.First().Name}' and target system '{targetService.TargetSystem.Name}'. Remove excess records and restart application.";
            throw new Exception(message);
        }

        return domainTypes.SingleOrDefault();
    }
}
