using System;

namespace Framework.Workflow.Domain
{

    /// <summary>
    /// Перечень возможных операци в контексте воркфлоу
    /// </summary>
    /// <remarks>
    /// В рамках контекста операции валидатор проверяет различные условия
    /// </remarks>
    [Flags]
    public enum WorkflowOperationContext
    {
        Create = 1,

        Edit = 2,

        Save = 4,

        Start = 8,

        All = Create | Edit | Save | Start,
    }

    public static class WorkflowOperationContextC
    {
        public const int Create = (int)WorkflowOperationContext.Create;

        public const int Edit = (int)WorkflowOperationContext.Edit;

        public const int Save = (int)WorkflowOperationContext.Save;

        public const int Start = (int)WorkflowOperationContext.Start;

        public const int All = (int)WorkflowOperationContext.All;
    }
}