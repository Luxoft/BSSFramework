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
using Framework.Attachments.BLL;

using JetBrains.Annotations;

using AttachmentsSampleSystem.Domain;

namespace AttachmentsSampleSystem.BLL
{
    public partial class AttachmentsSampleSystemBLLContext
    {
        private readonly Func<string, IAttachmentsSampleSystemBLLContext> _impersonateFunc;

        public AttachmentsSampleSystemBLLContext(
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
            [NotNull] IAttachmentsSampleSystemSecurityService securityService,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull] IAttachmentsSampleSystemBLLFactoryContainer logics,
            [NotNull] IAuthorizationBLLContext authorization,
            [NotNull] Framework.Configuration.BLL.IConfigurationBLLContext configuration,
            [NotNull] IAttachmentsBLLContext Attachments,
            [NotNull] ICryptService<CryptSystem> cryptService,
            [NotNull] Func<string, IAttachmentsSampleSystemBLLContext> impersonateFunc,
            [NotNull] ITypeResolver<string> currentTargetSystemTypeResolver)
            : base(serviceProvider, dalFactory, operationListeners, sourceListeners, objectStateService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService, dateTimeService)
        {
            this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));

            this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            this.Logics = logics ?? throw new ArgumentNullException(nameof(logics));

            this.Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Attachments = Attachments ?? throw new ArgumentNullException(nameof(Attachments));

            this.CryptService = cryptService ?? throw new ArgumentNullException(nameof(cryptService));

            this._impersonateFunc = impersonateFunc ?? throw new ArgumentNullException(nameof(impersonateFunc));
            this.TypeResolver = currentTargetSystemTypeResolver ?? throw new ArgumentNullException(nameof(currentTargetSystemTypeResolver));
        }

        public IAttachmentsSampleSystemSecurityService SecurityService { get; }

        public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }

        public override IAttachmentsSampleSystemBLLFactoryContainer Logics { get; }

        public IAuthorizationBLLContext Authorization { get; }

        public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; }

        public IAttachmentsBLLContext Attachments { get; }

        public ICryptService<CryptSystem> CryptService { get; }

        public ITypeResolver<string> TypeResolver { get; }

        public IAttachmentsSampleSystemBLLContext Impersonate(string principalName)
        {
            return this._impersonateFunc(principalName);
        }
    }
}
