using Framework.Configuration.SubscriptionModeling;
using Framework.Workflow.BLL;

namespace SampleSystem.Subscriptions.Metadata.WorkflowCreateModel.Create
{
    public sealed class ConditionLambda : LambdaMetadata<IWorkflowBLLContext, Framework.Workflow.Domain.WorkflowCreateModel, bool>
    {
        public ConditionLambda()
        {
            this.DomainObjectChangeType = DomainObjectChangeType.Create;
            this.Lambda = (context, versions) => true;
        }
    }
}
