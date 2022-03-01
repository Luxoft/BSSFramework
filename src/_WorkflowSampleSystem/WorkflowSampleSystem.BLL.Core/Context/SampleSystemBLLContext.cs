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

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Projections;

namespace WorkflowSampleSystem.BLL
{
    public partial class WorkflowSampleSystemBLLContext
    {
        private readonly Func<string, IWorkflowSampleSystemBLLContext> _impersonateFunc;

        public WorkflowSampleSystemBLLContext(
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
            [NotNull] IWorkflowSampleSystemSecurityService securityService,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull] IWorkflowSampleSystemBLLFactoryContainer logics,
            [NotNull] IAuthorizationBLLContext authorization,
            [NotNull] Framework.Configuration.BLL.IConfigurationBLLContext configuration,
            [NotNull] ICryptService<CryptSystem> cryptService,
            [NotNull] Func<string, IWorkflowSampleSystemBLLContext> impersonateFunc,
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

        public IWorkflowSampleSystemSecurityService SecurityService { get; }

        public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }

        public override IWorkflowSampleSystemBLLFactoryContainer Logics { get; }

        public IAuthorizationBLLContext Authorization { get; }

        public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; }

        public ICryptService<CryptSystem> CryptService { get; }

        public ITypeResolver<string> TypeResolver { get; }

        public IWorkflowSampleSystemBLLContext Impersonate(string principalName)
        {
            return this._impersonateFunc(principalName);
        }

        public override bool AllowVirtualPropertyInOdata(Type domainType)
        {
            return base.AllowVirtualPropertyInOdata(domainType) || domainType == typeof(BusinessUnitProgramClass);
        }
    }
}
