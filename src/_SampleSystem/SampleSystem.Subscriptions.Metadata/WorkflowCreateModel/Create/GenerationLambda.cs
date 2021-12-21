using System.Collections.Generic;

using Framework.Configuration.Core;
using Framework.Configuration.SubscriptionModeling;
using Framework.Notification;
using Framework.Workflow.BLL;

namespace SampleSystem.Subscriptions.Metadata.WorkflowCreateModel.Create
{
    public sealed class GenerationLambda : LambdaMetadata<IWorkflowBLLContext, Framework.Workflow.Domain.WorkflowCreateModel, IEnumerable<NotificationMessageGenerationInfo>>
    {
        public GenerationLambda()
        {
            this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Create;
            this.Lambda = this.GetRecipients;
        }

        private NotificationMessageGenerationInfo[] GetRecipients(
            IWorkflowBLLContext context,
            DomainObjectVersions<Framework.Workflow.Domain.WorkflowCreateModel> versions)
        {
            return new[] { new NotificationMessageGenerationInfo("tester@luxoft.com", versions.Previous, versions.Current) };
        }
    }
}
