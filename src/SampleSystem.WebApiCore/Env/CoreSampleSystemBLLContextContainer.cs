using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.TemplateEvaluator;
using Framework.Events;
using Framework.Notification.DTO;
using Framework.NotificationCore.Senders;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Persistent;
using Framework.Report;
using Framework.Security.Cryptography;
using Framework.Validation;

using SampleSystem.BLL;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore
{
    public class CoreSampleSystemBLLContextContainer : SampleSystemBLLContextContainerStandard
    {
        private readonly SmtpSettings smtpSettings;

        private readonly IRewriteReceiversService rewriteReceiversService;

        public CoreSampleSystemBLLContextContainer(
            SampleSystemServiceEnvironmentStandard serviceEnvironment,
            IServiceProvider scopedServiceProvider,
            ValidatorCompileCache defaultAuthorizationValidatorCompileCache,
            ValidatorCompileCache validatorCompileCache,
            Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<SampleSystem.Domain.PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc,
            IFetchService<SampleSystem.Domain.PersistentDomainObjectBase, FetchBuildRule> fetchService,
            ICryptService<CryptSystem> cryptService,
            ITypeResolver<string> currentTargetSystemTypeResolver,
            IDBSession session,
            string currentPrincipalName,
            SmtpSettings smtpSettings,
            IRewriteReceiversService rewriteReceiversService)
            : base(serviceEnvironment, scopedServiceProvider, defaultAuthorizationValidatorCompileCache, validatorCompileCache, securityExpressionBuilderFactoryFunc, fetchService, cryptService, currentTargetSystemTypeResolver, session, currentPrincipalName)
        {
            this.smtpSettings = smtpSettings;
            this.rewriteReceiversService = rewriteReceiversService;
        }

        protected override ITemplateEvaluatorFactory GetTemplateEvaluatorFactory() => new TemplateEvaluatorFactory();

        protected override IStandardSubscriptionService CreateSubscriptionService() => new LocalDBSubscriptionService(this.Configuration);

        protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender() =>
            new SmtpMessageSender(LazyHelper.Create(() => this.smtpSettings), LazyHelper.Create(() => this.rewriteReceiversService), this.Configuration);

        protected override IMessageSender<Exception> GetExceptionSender()
        {
            return MessageSender<Exception>.Trace;
        }

        protected override IEnumerable<ITargetSystemService> GetConfigurationTargetSystemServices(
            SubscriptionMetadataStore subscriptionMetadataStore)
        {
            yield return new CustomTargetSystemService<ISampleSystemBLLContext, SampleSystem.Domain.PersistentDomainObjectBase>(
                this.Configuration,
                this.MainContext,
                this.Configuration.Logics.TargetSystem.GetObjectBy(ts => ts.IsMain, true),
                this.GetMainEventDALListeners(),
                this.ServiceEnvironment.SubscriptionMetadataStore);

            yield return new CustomTargetSystemService<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>(
                this.Configuration,
                this.Configuration,
                this.Configuration.Logics.TargetSystem.GetByName(TargetSystemHelper.ConfigurationName, true),
                this.GetConfigurationEventDALListeners(),
                this.ServiceEnvironment.SubscriptionMetadataStore);

            yield return new CustomTargetSystemService<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>(
                this.Configuration,
                this.Authorization,
                this.Configuration.Logics.TargetSystem.GetByName(TargetSystemHelper.AuthorizationName, true),
                this.GetAuthorizationEventDALListeners(),
                this.ServiceEnvironment.SubscriptionMetadataStore);
        }

        private class CustomTargetSystemService<TBLLContext, TPersistentDomainObjectBase> : TargetSystemService<TBLLContext, TPersistentDomainObjectBase>
            where TBLLContext : class, ITypeResolverContainer<string>,
            ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>>,
            IDefaultBLLContext<TPersistentDomainObjectBase, Guid>, IBLLOperationEventContext<TPersistentDomainObjectBase>
            where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        {
            public CustomTargetSystemService(
                IConfigurationBLLContext context,
                TBLLContext targetSystemContext,
                TargetSystem targetSystem,
                IEnumerable<IManualEventDALListener<TPersistentDomainObjectBase>> eventDalListeners,
                SubscriptionMetadataStore subscriptionMetadataStore = null)
                : base(context, targetSystemContext, targetSystem, eventDalListeners, subscriptionMetadataStore)
            {
            }

            protected override bool IsNewSubscriptionServiceRequired() => true;
        }
    }
}
