using Framework.Configuration.Core;
using Framework.Notification;

using SampleSystem.BLL;
using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.DataModel.Create
{
    public sealed class GenerationLambda : GenerationLambdaBase<DateModel>
    {
        public GenerationLambda()
        {
            this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Create;
            this.Lambda = this.GetRecipients;
        }

        private NotificationMessageGenerationInfo[] GetRecipients(
            ISampleSystemBLLContext context,
            DomainObjectVersions<DateModel> versions)
        {
            return new[] { new NotificationMessageGenerationInfo("tester@luxoft.com", versions.Previous, versions.Current) };
        }
    }
}
