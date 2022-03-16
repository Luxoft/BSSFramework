using Framework.Security;
using Framework.Attachments;

namespace AttachmentsSampleSystem
{
    [BaseSecurityOperationType]
    [BaseSecurityOperationType(typeof(AttachmentsSecurityOperationCode))]
    public enum AttachmentsSampleSystemSecurityOperationCode
    {
        /// <summary>
        /// Специальная операция для отключения безопасности
        /// </summary>
        Disabled = 0,

        #region Employee

        [SecurityOperation("Employee", true, "{C73DD1F6-74D5-4445-A265-2E96832A7F89}", "Employee View", DomainType = "Employee")]
        EmployeeView,

        [SecurityOperation("Employee", true, "{45E8E5B8-620C-42C6-BAF5-20AB1CF27B8E}", "Employee Edit", DomainType = "Employee")]
        EmployeeEdit,

        #endregion

        #region BusinessUnit

        [SecurityOperation("BusinessUnit", true, "{B79E7132-845E-4BBF-8C3A-8FA3F6B31CF6}", "Business Unit View", DomainType = "BusinessUnit")]
        BusinessUnitView,

        [SecurityOperation("BusinessUnit", true, "{10000000-71C4-47CD-8683-000000000003}", "Business Unit Edit", DomainType = "BusinessUnit")]
        BusinessUnitEdit,

        #endregion

        #region HRDepartment

        [SecurityOperation("HRDepartmentView", false, "{00000000-71C4-47CD-8683-000000000001}", DomainType = "HRDepartment")]
        HRDepartmentView,

        [SecurityOperation("HRDepartmentEdit", false, "{00000000-71C4-47CD-8683-000000000002}", DomainType = "HRDepartment")]
        HRDepartmentEdit,

        #endregion

        #region Location

        [SecurityOperation("LocationView", false, "E5377866-FF6D-4D05-912F-2D3C72F27FA7", DomainType = "Location")]
        LocationView,

        [SecurityOperation("LocationEdit", false, "034C4E00-9C62-422B-98B8-B119C1991596", DomainType = "Location")]
        LocationEdit,

        #endregion

        [SecurityOperation(SecurityOperationCode.SystemIntegration)]
        SystemIntegration,




        [SecurityOperation("TestAttachments", false, "{939EC98C-131B-4E3E-B97C-9DF95620C758}", "Required operation for approve", adminHasAccess: false)]
        ApproveAttachmentsOperation,

        [AttachmentsSampleSystemApproveOperation(ApproveAttachmentsOperation)]
        [SecurityOperation("TestAttachments", false, "{927E4AFC-8CC2-4EDA-B6EE-FE6B2C53D0BA}", "Operation testing Attachments")]
        ApprovingAttachmentsOperation
    }
}
