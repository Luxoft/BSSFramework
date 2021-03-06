using System;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.BLL.Tracking;
using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.Security.Cryptography;
using Framework.SecuritySystem;

using JetBrains.Annotations;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL
{
    public partial class SampleSystemBLLContext
    {
        public SampleSystemBLLContext(
            IServiceProvider serviceProvider,
            [NotNull] IDALFactory<PersistentDomainObjectBase, Guid> dalFactory,
            [NotNull] IOperationEventSenderContainer<PersistentDomainObjectBase> operationSenders,
            [NotNull] BLLSourceEventListenerContainer<PersistentDomainObjectBase> sourceListeners,
            [NotNull] IObjectStateService objectStateService,
            [NotNull] IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IStandartExpressionBuilder standartExpressionBuilder,
            [NotNull] ISampleSystemValidator validator,
            [NotNull] IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            [NotNull] IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            [NotNull] ISampleSystemSecurityService securityService,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull] ISampleSystemBLLFactoryContainer logics,
            [NotNull] IAuthorizationBLLContext authorization,
            [NotNull] Framework.Configuration.BLL.IConfigurationBLLContext configuration,
            [NotNull] ICryptService<CryptSystem> cryptService,
            [NotNull] ISampleSystemBLLContextSettings settings)
            : base(serviceProvider, dalFactory, operationSenders, sourceListeners, objectStateService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
        {
            this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));

            this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            this.Logics = logics ?? throw new ArgumentNullException(nameof(logics));

            this.Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this.CryptService = cryptService ?? throw new ArgumentNullException(nameof(cryptService));

            this.TypeResolver = settings.TypeResolver;
        }

        public ISampleSystemSecurityService SecurityService { get; }

        public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }

        public override ISampleSystemBLLFactoryContainer Logics { get; }

        public IAuthorizationBLLContext Authorization { get; }

        public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; }

        public ICryptService<CryptSystem> CryptService { get; }

        public ITypeResolver<string> TypeResolver { get; }

        public override bool AllowVirtualPropertyInOdata(Type domainType)
        {
            return base.AllowVirtualPropertyInOdata(domainType) || domainType == typeof(BusinessUnitProgramClass);
        }

        ITypeResolver<string> ISecurityTypeResolverContainer.SecurityTypeResolver => this.TypeResolver;
    }
}
