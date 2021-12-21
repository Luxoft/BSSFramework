namespace Framework.Workflow.BLL
{
    public interface IOwnerWorkflowContainer<out TOwnerWorkflow>
    {
        TOwnerWorkflow Owner { get; }
    }
}