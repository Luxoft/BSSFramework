using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.Tracking;
using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class SampleSystemBLLContext
{
    public SampleSystemBLLContext(
            IServiceProvider serviceProvider,
            IOperationEventSenderContainer<PersistentDomainObjectBase> operationSenders,
            ITrackingService<PersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            ISampleSystemValidator validator,
            IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            ISampleSystemSecurityService securityService,
            ISampleSystemBLLFactoryContainer logics,
            IAuthorizationBLLContext authorization,
            Framework.Configuration.BLL.IConfigurationBLLContext configuration,
            ISampleSystemBLLContextSettings settings,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory)
            : base(serviceProvider, operationSenders, trackingService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
    {
        this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        this.Logics = logics ?? throw new ArgumentNullException(nameof(logics));

        this.Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory;

        this.TypeResolver = settings.TypeResolver;
    }

    public ISampleSystemSecurityService SecurityService { get; }

    public override ISampleSystemBLLFactoryContainer Logics { get; }

    public IAuthorizationBLLContext Authorization { get; }

    public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; }

    public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }

    public ITypeResolver<string> TypeResolver { get; }

    public override bool AllowVirtualPropertyInOdata(Type domainType)
    {
        return base.AllowVirtualPropertyInOdata(domainType) || domainType == typeof(BusinessUnitProgramClass);
    }

    ITypeResolver<string> ISecurityTypeResolverContainer.SecurityTypeResolver => this.TypeResolver;
}
