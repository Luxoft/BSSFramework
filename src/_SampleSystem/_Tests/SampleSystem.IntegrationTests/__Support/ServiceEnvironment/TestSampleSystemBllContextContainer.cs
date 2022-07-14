using System;
using System.Collections.Generic;

using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Notification.DTO;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Security.Cryptography;
using Framework.Validation;

using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class TestSampleSystemBllContextContainer : SampleSystemBllContextContainer
{
    public TestSampleSystemBllContextContainer(
            SampleSystemServiceEnvironment serviceEnvironment,
            IServiceProvider scopedServiceProvider,
            IDBSession dbSession,
            IUserAuthenticationService userAuthenticationService,
            IDateTimeService dateTimeService,
            SubscriptionMetadataStore subscriptionMetadataStore,
            ValidatorCompileCache defaultAuthorizationValidatorCompileCache,
            ValidatorCompileCache validatorCompileCache,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            ICryptService<CryptSystem> cryptService,
            ITypeResolver<string> currentTargetSystemTypeResolver,
            string currentPrincipalName, SmtpSettings smtpSettings,
            IRewriteReceiversService rewriteReceiversService)

            : base(serviceEnvironment, scopedServiceProvider, dbSession, userAuthenticationService, dateTimeService, subscriptionMetadataStore, defaultAuthorizationValidatorCompileCache, validatorCompileCache, fetchService, cryptService, currentTargetSystemTypeResolver, smtpSettings, rewriteReceiversService)
    {
    }

    protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender()
    {
        return new LocalDBNotificationEventDTOMessageSender(this.Configuration);
    }
    protected override IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
    {
        foreach (var dalListener in base.GetBeforeTransactionCompletedListeners())
        {
            yield return dalListener;
        }

        yield return new PermissionWorkflowDALListener(this.MainContext);
    }
}
