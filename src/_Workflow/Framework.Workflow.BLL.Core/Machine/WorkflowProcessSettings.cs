using System;

namespace Framework.Workflow.BLL
{
    [Flags]
    public enum WorkflowProcessSettings
    {
        Default = 0,

        SkipTryFinishParallel = 1
    }
}