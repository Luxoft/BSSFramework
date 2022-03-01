using System;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.BLL.Tracking;
using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.Security.Cryptography;
using Framework.SecuritySystem;
using Framework.Validation;

using JetBrains.Annotations;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL
{
    public partial class SampleSystemBLLContext
    {
        private readonly Func<string, ISampleSystemBLLContext> _impersonateFunc;

        public SampleSystemBLLContext(
            IServiceProvider serviceProvider,
            [NotNull] IDALFactory<PersistentDomainObjectBase, Guid> dalFactory,
            [NotNull] BLLOperationEventListenerContainer<DomainObjectBase> operationListeners,
            [NotNull] BLLSourceEventListenerContainer<PersistentDomainObjectBase> sourceListeners,
            [NotNull] IObjectStateService objectStateService,
            [NotNull] IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IStandartExpressionBuilder standartExpressionBuilder,
            [NotNull] IValidator validator,
            [NotNull] IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            [NotNull] IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            [NotNull] IDateTimeService dateTimeService,
            [NotNull] ISampleSystemSecurityService securityService,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull] ISampleSystemBLLFactoryContainer logics,
            [NotNull] IAuthorizationBLLContext authorization,
            [NotNull] Framework.Configuration.BLL.IConfigurationBLLContext configuration,
            [NotNull] ICryptService<CryptSystem> cryptService,
            [NotNull] Func<string, ISampleSystemBLLContext> impersonateFunc,
            [NotNull] ITypeResolver<string> currentTargetSystemTypeResolver)
            : base(serviceProvider, dalFactory, operationListeners, sourceListeners, objectStateService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService, dateTimeService)
        {
            this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));

            this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            this.Logics = logics ?? throw new ArgumentNullException(nameof(logics));

            this.Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this.CryptService = cryptService ?? throw new ArgumentNullException(nameof(cryptService));

            this._impersonateFunc = impersonateFunc ?? throw new ArgumentNullException(nameof(impersonateFunc));
            this.TypeResolver = currentTargetSystemTypeResolver ?? throw new ArgumentNullException(nameof(currentTargetSystemTypeResolver));
        }

        public ISampleSystemSecurityService SecurityService { get; }

        public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }

        public override ISampleSystemBLLFactoryContainer Logics { get; }

        public IAuthorizationBLLContext Authorization { get; }

        public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; }

        public ICryptService<CryptSystem> CryptService { get; }

        public ITypeResolver<string> TypeResolver { get; }

        public ISampleSystemBLLContext Impersonate(string principalName)
        {
            return this._impersonateFunc(principalName);
        }

        public override bool AllowVirtualPropertyInOdata(Type domainType)
        {
            return base.AllowVirtualPropertyInOdata(domainType) || domainType == typeof(BusinessUnitProgramClass);
        }
    }
}
