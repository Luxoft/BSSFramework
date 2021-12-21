using System.Collections.Generic;

using Framework.Configuration.BLL;
using Framework.Configuration.Core;
using Framework.Configuration.SubscriptionModeling;
using Framework.Notification;

namespace SampleSystem.Subscriptions.Metadata.RunRegularJobModel.Create
{
    public sealed class GenerationLambda : LambdaMetadata<IConfigurationBLLContext, Framework.Configuration.Domain.RunRegularJobModel, IEnumerable<NotificationMessageGenerationInfo>>
    {
        public GenerationLambda()
        {
            this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Create;
            this.Lambda = this.GetRecipients;
        }

        private NotificationMessageGenerationInfo[] GetRecipients(
            IConfigurationBLLContext context,
            DomainObjectVersions<Framework.Configuration.Domain.RunRegularJobModel> versions)
        {
            return new[] { new NotificationMessageGenerationInfo("tester@luxoft.com", versions.Previous, versions.Current) };
        }
    }
}
