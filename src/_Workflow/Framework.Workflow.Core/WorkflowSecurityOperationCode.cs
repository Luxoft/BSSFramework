using Framework.Security;

namespace Framework.Workflow
{
    public enum WorkflowSecurityOperationCode
    {
        Disabled = 0,

        #region Workflow

        [SecurityOperation(SecurityOperationCode.WorkflowView)]
        WorkflowView,

        [SecurityOperation(SecurityOperationCode.WorkflowEdit)]
        WorkflowEdit,

        #endregion

        #region Integration

        [SecurityOperation(SecurityOperationCode.SystemIntegration)]
        SystemIntegration

        #endregion
    }
}
