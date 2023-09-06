using System.Configuration;

using Framework.Authorization.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.Tracking;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Notification;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL;

public partial class ConfigurationBLLContext
{
    private readonly IBLLSimpleQueryBase<IEmployee> employeeSource;

    private readonly Lazy<Dictionary<TargetSystem, ITargetSystemService>> lazyTargetSystemServiceCache;

    private readonly IDictionaryCache<Type, DomainType> domainTypeCache;

    private readonly IDictionaryCache<IDomainType, DomainType> domainTypeNameCache;

    private readonly ICurrentRevisionService currentRevisionService;

    public ConfigurationBLLContext(
            IServiceProvider serviceProvider,
            IOperationEventSenderContainer<PersistentDomainObjectBase> operationSenders,
            ITrackingService<PersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            IConfigurationValidator validator,
            IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            IMessageSender<MessageTemplateNotification> subscriptionSender,
            IConfigurationSecurityService securityService,
            IConfigurationBLLFactoryContainer logics,
            IAuthorizationBLLContext authorization,
            IBLLSimpleQueryBase<IEmployee> employeeSource,
            IEnumerable<ITargetSystemService> targetSystemServices,
            IConfigurationBLLContextSettings settings,
            ICurrentRevisionService currentRevisionService)
            : base(serviceProvider, operationSenders, trackingService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
    {
        this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));
        this.SubscriptionSender = subscriptionSender ?? throw new ArgumentNullException(nameof(subscriptionSender));

        this.SecurityService = securityService;
        this.Logics = logics;

        this.Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        this.employeeSource = employeeSource ?? throw new ArgumentNullException(nameof(employeeSource));
        this.currentRevisionService = currentRevisionService ?? throw new ArgumentNullException(nameof(currentRevisionService));

        this.lazyTargetSystemServiceCache = LazyHelper.Create(() => targetSystemServices.ToDictionary(s => s.TargetSystem));

        this.domainTypeCache = new DictionaryCache<Type, DomainType>(type =>

                                                                             this.GetTargetSystemService(type, false).Maybe(targetService => this.GetDomainType(targetService, type))).WithLock();

        this.domainTypeNameCache = new DictionaryCache<IDomainType, DomainType>(
                                                                                domainType => this.Logics.DomainType.GetByDomainType(domainType),
                                                                                new EqualityComparerImpl<IDomainType>((dt1, dt2) => dt1.Name == dt2.Name && dt1.NameSpace == dt2.NameSpace, dt => dt.Name.GetHashCode() ^ dt.NameSpace.GetHashCode())).WithLock();

        this.SystemConstantSerializerFactory = settings.SystemConstantSerializerFactory;

        this.ComplexDomainTypeResolver = TypeResolverHelper.Create(
                                                                   (DomainType domainType) =>
                                                                   {
                                                                       if (domainType.TargetSystem.IsBase)
                                                                       {
                                                                           return TypeResolverHelper.Base.Resolve(domainType.FullTypeName);
                                                                       }
                                                                       else
                                                                       {
                                                                           return this.GetTargetSystemService(domainType.TargetSystem).TypeResolver.Resolve(domainType);
                                                                       }
                                                                   },
                                                                   () => this.GetTargetSystemServices().SelectMany(tss => tss.TypeResolver.GetTypes()).Concat(TypeResolverHelper.Base.GetTypes())).WithCache().WithLock();

        this.TypeResolver = settings.TypeResolver;
    }

    public IMessageSender<MessageTemplateNotification> SubscriptionSender { get; }

    public IConfigurationSecurityService SecurityService { get; }

    public ITypeResolver<string> TypeResolver { get; }

    public ITypeResolver<DomainType> ComplexDomainTypeResolver { get; }

    public override IConfigurationBLLFactoryContainer Logics { get; }

    public IAuthorizationBLLContext Authorization { get; }

    public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }

    public bool DisplayInternalError => this.Authorization.HasAccess(ConfigurationSecurityOperation.DisplayInternalError);

    public ISerializerFactory<string> SystemConstantSerializerFactory { get; }

    public bool SubscriptionEnabled => this.lazyTargetSystemServiceCache.Value.Values.Any(tss => tss.TargetSystem.SubscriptionEnabled);

    IConfigurationBLLContext IConfigurationBLLContextContainer<IConfigurationBLLContext>.Configuration => this;

    /// <inheritdoc />
    public long GetCurrentRevision()
    {
        return this.currentRevisionService.GetCurrentRevision();
    }

    public IBLLSimpleQueryBase<IEmployee> GetEmployeeSource()
    {
        return this.employeeSource;
    }

    public IPersistentTargetSystemService GetPersistentTargetSystemService(TargetSystem targetSystem)
    {
        if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

        return (this.GetTargetSystemService(targetSystem) as IPersistentTargetSystemService)
                .FromMaybe(() => new ArgumentException($"Target System {targetSystem.Name} is not persistent", nameof(targetSystem)));
    }

    public IEnumerable<IPersistentTargetSystemService> GetPersistentTargetSystemServices()
    {
        return this.lazyTargetSystemServiceCache.Value.Values.OfType<IPersistentTargetSystemService>();
    }


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

    public ITargetSystemService GetMainTargetSystemService()
    {
        return this.lazyTargetSystemServiceCache.Value.Values.Single(
                                                                     service => service.TargetSystem.IsMain,
                                                                     () => new BusinessLogicException("Main Target System not found"),
                                                                     () => new BusinessLogicException("To many Main Target Systems"));
    }

    public ITargetSystemService GetTargetSystemService(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return this.lazyTargetSystemServiceCache.Value.Values.GetByName(name);
    }

    public IEnumerable<ITargetSystemService> GetTargetSystemServices()
    {
        return this.lazyTargetSystemServiceCache.Value.Values;
    }

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

    public ISubscriptionSystemService GetSubscriptionSystemService(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return this.GetTargetSystemService(domainType, true).SubscriptionService;
    }

    private DomainType GetDomainType(ITargetSystemService targetService, Type type)
    {
        var domainTypes = this.Logics.DomainType.GetListBy(domainType => domainType.TargetSystem == targetService.TargetSystem
                                                                         && domainType.Name == type.Name
                                                                         && domainType.NameSpace == type.Namespace);

        if (domainTypes.Count > 1)
        {
            var message = $"Configuration database contains more than one record for domain type '{domainTypes.First().Name}' and target system '{targetService.TargetSystem.Name}'. Remove excess records and restart application.";
            throw new ConfigurationErrorsException(message);
        }

        return domainTypes.SingleOrDefault();
    }
}
