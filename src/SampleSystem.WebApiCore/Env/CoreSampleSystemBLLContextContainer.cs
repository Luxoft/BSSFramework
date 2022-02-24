using System;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Notification;
using Framework.Notification.DTO;
using Framework.NotificationCore.Senders;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
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

        protected override IStandardSubscriptionService CreateSubscriptionService() => new LocalDBSubscriptionService(this.Configuration);

        protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender() =>
            new SmtpMessageSender(LazyHelper.Create(() => this.smtpSettings), LazyHelper.Create(() => this.rewriteReceiversService), this.Configuration);
        
        protected override IMessageSender<Exception> GetExceptionSender()
        {
            return MessageSender<Exception>.Trace;
        }
    }
}
