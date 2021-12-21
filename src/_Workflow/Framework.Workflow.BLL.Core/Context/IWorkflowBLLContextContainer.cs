namespace Framework.Workflow.BLL
{
    public interface IWorkflowBLLContextContainer
    {
        IWorkflowBLLContext Workflow { get; }
    }
}