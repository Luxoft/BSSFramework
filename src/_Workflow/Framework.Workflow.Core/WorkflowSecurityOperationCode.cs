using Framework.Security;

namespace Framework.Workflow
{
    public enum WorkflowSecurityOperationCode
    {
        Disabled = 0,

        #region Workflow

        [SecurityOperation("View Workflow", false, "{10CE7EDF-45C3-4285-81FB-4399A5907890}", "Can view Workflow", DomainType = "Workflow")]
        WorkflowView,

        [SecurityOperation("Edit Workflow", false, "{3C84B2B4-40CE-4F37-9CD7-D4CC38E8C9C0}", "Can edit Workflow", DomainType = "Workflow")]
        WorkflowEdit,

        #endregion

        #region Integration

        [SecurityOperation(SecurityOperationCode.SystemIntegration)]
        SystemIntegration

        #endregion
    }
}
