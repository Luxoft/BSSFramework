namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Интерфейс для контейнера объектов
    /// </summary>
    public interface IWorkflowElement
    {
        Workflow Workflow { get; }
    }
}