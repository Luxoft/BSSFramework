namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Интерфейс для экземпляров воркфлоу
    /// </summary>
    public interface IWorkflowInstanceElement
    {
        WorkflowInstance WorkflowInstance { get; }
    }
}