using CommonFramework;
using CommonFramework.DictionaryCache;

using Framework.Application.Events;
using Framework.Application.Lock;
using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.BLL;
using Framework.BLL.Domain.Exceptions;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.IdentityObject;
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
using SecuritySystem.Notification;

using PersistentDomainObjectBase = Framework.Configuration.Domain.PersistentDomainObjectBase;

namespace Framework.Configuration.BLL;

public partial class ConfigurationBLLContext
{
    private readonly Lazy<Dictionary<TargetSystem, ITargetSystemService>> lazyTargetSystemServiceCache;

    private readonly IDictionaryCache<Type, DomainType> domainTypeCache;

    private readonly IDictionaryCache<IDomainType, DomainType> domainTypeNameCache;

    private readonly ICurrentRevisionService currentRevisionService;

    private readonly IReadOnlyDictionary<Type, TargetSystemInfo> targetSystemInfoDict;

    private readonly IReadOnlyDictionary<Type, DomainTypeInfo> domainTypeInfoDict;

    public ConfigurationBLLContext(
        IServiceProvider serviceProvider,
        [FromKeyedServices("BLL")] IEventOperationSender operationSender,
        IAccessDeniedExceptionService accessDeniedExceptionService,
        IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
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
        ConfigurationBLLContextSettings settings,
        INotificationPrincipalExtractor<Principal> notificationPrincipalExtractor)
        : base(serviceProvider, operationSender, accessDeniedExceptionService, hierarchicalObjectExpanderFactory)
    {
        this.TrackingService = trackingService;
        this.Validator = validator;

        this.SecurityService = securityService;
        this.Logics = logics;

        this.Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        this.NamedLockService = namedLockService;
        this.EventOperationSource = eventOperationSource;
        this.currentRevisionService = currentRevisionService ?? throw new ArgumentNullException(nameof(currentRevisionService));
        this.NotificationPrincipalExtractor = notificationPrincipalExtractor;

        this.lazyTargetSystemServiceCache = LazyHelper.Create(() => targetSystemServices.ToDictionary(s => s.TargetSystem));

        this.domainTypeCache = new DictionaryCache<Type, DomainType>(type =>

                                                                         this.GetTargetSystemService(type, false).Maybe(targetService => this.GetDomainType(targetService, type)))
            .WithLock();

        this.domainTypeNameCache = new DictionaryCache<IDomainType, DomainType>(
                domainType => this.Logics.DomainType.GetByDomainType(domainType),
                new EqualityComparerImpl<IDomainType>(
                    (dt1, dt2) => dt1.Name == dt2.Name && dt1.NameSpace == dt2.NameSpace,
                    dt => dt.Name.GetHashCode() ^ dt.NameSpace.GetHashCode()))
            .WithLock();

        this.SystemConstantSerializerFactory = settings.SystemConstantSerializerFactory;
        this.SystemConstantTypeResolver = settings.SystemConstantTypeResolver;

        this.ComplexDomainTypeResolver = TypeResolverHelper.Create(
            (DomainType domainType) =>
            {
                if (domainType.TargetSystem.IsBase)
                {
                    return TypeResolverHelper.Base.TryResolve(domainType.FullTypeName);
                }
                else
                {
                    return this.GetTargetSystemService(domainType.TargetSystem).TypeResolver.TryResolve(domainType);
                }
            },

            () => this.GetTargetSystemServices().SelectMany(tss => tss.TypeResolver.Types).Concat(TypeResolverHelper.Base.Types));

        this.TypeResolver = settings.TypeResolver;

        var targetSystemInfoDictRequest = from targetSystemInfo in targetSystemInfoList

                                          from domainType in targetSystemInfo.DomainTypes

                                          select (domainType.Type, targetSystemInfo);

        this.targetSystemInfoDict = targetSystemInfoDictRequest.ToDictionary();


        var domainTypeInfoDictRequest = from targetSystemInfo in targetSystemInfoList

                                        from domainType in targetSystemInfo.DomainTypes

                                        select (domainType.Type, domainType);

        this.domainTypeInfoDict = domainTypeInfoDictRequest.ToDictionary();
    }

    public ITrackingService<PersistentDomainObjectBase> TrackingService { get; }

    public IRootSecurityService SecurityService { get; }

    public ITypeResolver<string> TypeResolver { get; }

    public ITypeResolver<DomainType> ComplexDomainTypeResolver { get; }

    public override IConfigurationBLLFactoryContainer Logics { get; }

    public IValidator Validator { get; }

    public INotificationPrincipalExtractor<Principal> NotificationPrincipalExtractor { get; }

    public IAuthorizationBLLContext Authorization { get; }

    public ISerializerFactory<string> SystemConstantSerializerFactory { get; }

    public ITypeResolver<string> SystemConstantTypeResolver { get; }

    public INamedLockService NamedLockService { get; }

    public IDomainObjectEventMetadata EventOperationSource { get; }

    /// <inheritdoc />
    public long GetCurrentRevision() => this.currentRevisionService.GetCurrentRevision();

    public ITargetSystemService GetTargetSystemService(TargetSystem targetSystem)
    {
        if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

        return this.lazyTargetSystemServiceCache.Value[targetSystem];
    }

    public ITargetSystemService GetTargetSystemService(Type domainType, bool throwOnNotFound)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (throwOnNotFound)
        {
            return this.lazyTargetSystemServiceCache.Value.Values.Single(
                service => service.IsAssignable(domainType),
                () => new BusinessLogicException($"Target System for type {domainType.Name} not found"),
                () => new BusinessLogicException($"To many Target Systems for type {domainType.Name}"));
        }
        else
        {
            return this.lazyTargetSystemServiceCache.Value.Values.SingleOrDefault(
                service => service.IsAssignable(domainType),
                () => new BusinessLogicException($"Target System for type {domainType.Name} not found"));
        }
    }

    public ITargetSystemService GetMainTargetSystemService() =>
        this.lazyTargetSystemServiceCache.Value.Values.Single(
            service => service.TargetSystem.IsMain,
            () => new BusinessLogicException("Main Target System not found"),
            () => new BusinessLogicException("To many Main Target Systems"));

    public ITargetSystemService GetTargetSystemService(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return this.lazyTargetSystemServiceCache.Value.Values.GetByName(name);
    }

    public TargetSystemInfo GetTargetSystemInfo(Type domainType) => this.targetSystemInfoDict[domainType];

    public DomainTypeInfo GetDomainTypeInfo(Type domainType) => this.domainTypeInfoDict[domainType];

    public IEnumerable<ITargetSystemService> GetTargetSystemServices() => this.lazyTargetSystemServiceCache.Value.Values;

    public DomainType GetDomainType(Type type, bool throwOnNotFound)
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
                                                                         && domainType.NameSpace == type.Namespace);

        if (domainTypes.Count > 1)
        {
            var message =
                $"Configuration database contains more than one record for domain type '{domainTypes.First().Name}' and target system '{targetService.TargetSystem.Name}'. Remove excess records and restart application.";
            throw new Exception(message);
        }

        return domainTypes.SingleOrDefault();
    }
}
