using System;

using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Security.Cryptography;
using Framework.SecuritySystem.Rules.Builders;
using Framework.Validation;

using JetBrains.Annotations;

using Microsoft.Extensions.Options;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

using AuditPersistentDomainObjectBase = SampleSystem.Domain.AuditPersistentDomainObjectBase;
using AvailableValues = Framework.DomainDriven.AvailableValues;
using NamedLock = SampleSystem.Domain.NamedLock;
using NamedLockOperation = SampleSystem.Domain.NamedLockOperation;
using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;

namespace SampleSystem.ServiceEnvironment
{
    public class SampleSystemServiceEnvironment :
        ServiceEnvironmentBase<SampleSystemBLLContextContainer, ISampleSystemBLLContext, PersistentDomainObjectBase,
        AuditPersistentDomainObjectBase, SampleSystemSecurityOperationCode, NamedLock, NamedLockOperation>,
        ISystemMetadataTypeBuilderContainer
    {

        protected static readonly ITypeResolver<string> CurrentTargetSystemTypeResolver = new[] { TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver(), TypeSource.FromSample<BusinessUnitSimpleDTO>().ToDefaultTypeResolver() }.ToComposite();

        protected ICryptService<CryptSystem> CryptService { get; } = new CryptService<CryptSystem>();

        protected readonly bool? isDebugMode;
        public readonly ValidatorCompileCache ValidatorCompileCache;


        public readonly ValidatorCompileCache CustomAuthorizationValidatorCompileCache;


        protected readonly IFetchService<PersistentDomainObjectBase, FetchBuildRule> FetchService;
        protected readonly Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> SecurityExpressionBuilderFactoryFunc;

        public readonly SmtpSettings SmtpSettings;

        public readonly IRewriteReceiversService RewriteReceiversService;


        public SampleSystemServiceEnvironment(
            IServiceProvider serviceProvider,
            INotificationContext notificationContext,
            [NotNull] AvailableValues availableValues,
            IOptions<SmtpSettings> smtpSettings,
            IRewriteReceiversService rewriteReceiversService = null,
            bool? isDebugMode = null,
            Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<SampleSystem.Domain.PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc = null)
            : base(serviceProvider, notificationContext, availableValues)
        {
            this.SystemMetadataTypeBuilder = new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.All, typeof(PersistentDomainObjectBase).Assembly);
            this.SecurityExpressionBuilderFactoryFunc = securityExpressionBuilderFactoryFunc;
            this.isDebugMode = isDebugMode;

            this.ValidatorCompileCache =

                    availableValues
                    .ToValidation()
                    .ToBLLContextValidationExtendedData<ISampleSystemBLLContext, SampleSystem.Domain.PersistentDomainObjectBase, Guid>()
                    .Pipe(extendedData => new SampleSystemValidationMap(extendedData))
                    .ToCompileCache();

            this.FetchService = new SampleSystemMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<PersistentDomainObjectBase>.OData);

            this.SmtpSettings = smtpSettings.Value;
            this.RewriteReceiversService = rewriteReceiversService;
        }

        public ISystemMetadataTypeBuilder SystemMetadataTypeBuilder { get; }

        public override bool IsDebugMode => this.isDebugMode ?? base.IsDebugMode;
    }
}
