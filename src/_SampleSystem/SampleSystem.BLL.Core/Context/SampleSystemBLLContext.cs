using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.Events;
using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class SampleSystemBLLContext
{
    public SampleSystemBLLContext(
            IServiceProvider serviceProvider,
            [FromKeyedServices("BLL")] IEventOperationSender operationSender,
            ITrackingService<PersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            ISampleSystemValidator validator,
            IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            IRootSecurityService<PersistentDomainObjectBase> securityService,
            ISampleSystemBLLFactoryContainer logics,
            IAuthorizationBLLContext authorization,
            Framework.Configuration.BLL.IConfigurationBLLContext configuration,
            ISampleSystemBLLContextSettings settings,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            ISecurityRuleParser securityRuleParser)
            : base(serviceProvider, operationSender, trackingService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
    {
        this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        this.Logics = logics ?? throw new ArgumentNullException(nameof(logics));

        this.Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory;
        this.SecurityRuleParser = securityRuleParser;

        this.TypeResolver = settings.TypeResolver;
    }

    public IRootSecurityService<PersistentDomainObjectBase> SecurityService { get; }

    public ISecurityRuleParser SecurityRuleParser { get; }

    public override ISampleSystemBLLFactoryContainer Logics { get; }

    public IAuthorizationBLLContext Authorization { get; }

    public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; }

    public ISecurityExpressionBuilderFactory SecurityExpressionBuilderFactory { get; }

    public ITypeResolver<string> TypeResolver { get; }

    public override bool AllowVirtualPropertyInOdata(Type domainType)
    {
        return base.AllowVirtualPropertyInOdata(domainType) || domainType == typeof(BusinessUnitProgramClass);
    }
}
