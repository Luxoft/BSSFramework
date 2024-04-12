using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace SampleSystem;

public static class SampleSystemSecurityOperation
{
    #region Employee

    public static SecurityOperation EmployeeView { get; } = new(nameof(EmployeeView));

    public static SecurityOperation EmployeeEdit { get; } = new(nameof(EmployeeEdit));

    #endregion

    #region BusinessUnit

    public static SecurityOperation BusinessUnitView { get; } = new(nameof(BusinessUnitView)) { ExpandType = HierarchicalExpandType.All };

    public static SecurityOperation BusinessUnitEdit { get; } = new(nameof(BusinessUnitEdit));

    #endregion

    #region BusinessUnitType

    public static SecurityOperation BusinessUnitTypeModuleOpen { get; } = new(nameof(BusinessUnitTypeModuleOpen));

    public static SecurityOperation BusinessUnitTypeView { get; } = new(nameof(BusinessUnitTypeView));

    public static SecurityOperation BusinessUnitTypeEdit { get; } = new(nameof(BusinessUnitTypeEdit));

    #endregion

    #region BusinessUnitManagerCommissionLink

    public static SecurityOperation BusinessUnitManagerCommissionLinkView { get; } = new(nameof(BusinessUnitManagerCommissionLinkView));

    public static SecurityOperation BusinessUnitManagerCommissionLinkEdit { get; } = new(nameof(BusinessUnitManagerCommissionLinkEdit));

    #endregion

    #region BusinessUnitHrDepartment

    public static SecurityOperation BusinessUnitHrDepartmentView { get; } = new(nameof(BusinessUnitHrDepartmentView));

    public static SecurityOperation BusinessUnitHrDepartmentEdit { get; } = new(nameof(BusinessUnitHrDepartmentEdit));

    #endregion

    #region Management Unit

    public static SecurityOperation ManagementUnitView { get; } = new(nameof(ManagementUnitView)) { ExpandType = HierarchicalExpandType.All };

    public static SecurityOperation ManagementUnitEdit { get; } = new(nameof(ManagementUnitEdit));

    public static SecurityOperation ManagementUnitChangeView { get; } = new(nameof(ManagementUnitChangeView));

    public static SecurityOperation ManagementUnitChangeEdit { get; } = new(nameof(ManagementUnitChangeEdit));

    public static SecurityOperation CreateManagementUnitStart { get; } = new(nameof(CreateManagementUnitStart));

    #endregion

    #region ManagementUnitAndBusinessUnitLink

    public static SecurityOperation ManagementUnitAndBusinessUnitLinkView { get; } = new(nameof(ManagementUnitAndBusinessUnitLinkView));

    public static SecurityOperation ManagementUnitAndBusinessUnitLinkEdit { get; } = new(nameof(ManagementUnitAndBusinessUnitLinkEdit));

    #endregion

    #region ManagementUnitAndHRDepartmentLink

    public static SecurityOperation ManagementUnitAndHRDepartmentLinkView { get; } = new(nameof(ManagementUnitAndHRDepartmentLinkView));

    public static SecurityOperation ManagementUnitAndHRDepartmentLinkEdit { get; } = new(nameof(ManagementUnitAndHRDepartmentLinkEdit));

    #endregion

    #region EmployeeSpecialization

    public static SecurityOperation EmployeeSpecializationOpenModule { get; } = new(nameof(EmployeeSpecializationOpenModule));

    public static SecurityOperation EmployeeSpecializationView { get; } = new(nameof(EmployeeSpecializationView));

    #endregion

    #region EmployeeRole

    public static SecurityOperation EmployeeRoleView { get; } = new(nameof(EmployeeRoleView));

    #endregion

    #region EmployeeRoleDegree

    public static SecurityOperation EmployeeRoleDegreeView { get; } = new(nameof(EmployeeRoleDegreeView));

    #endregion

    #region HRDepartment

    public static SecurityOperation HRDepartmentView { get; } = new(nameof(HRDepartmentView));

    public static SecurityOperation HRDepartmentEdit { get; } = new(nameof(HRDepartmentEdit));

    public static SecurityOperation HRDepartmentAdvancedEdit { get; } = new(nameof(HRDepartmentAdvancedEdit));

    #endregion

    #region Location

    public static SecurityOperation LocationView { get; } = new(nameof(LocationView));

    public static SecurityOperation LocationEdit { get; } = new(nameof(LocationEdit));

    #endregion

    #region Country

    public static SecurityOperation CountryView { get; } = new(nameof(CountryView));

    public static SecurityOperation CountryEdit { get; } = new(nameof(CountryEdit));

    #endregion

    #region CompanyLegalEntity

    public static SecurityOperation CompanyLegalEntityView { get; } = new(nameof(CompanyLegalEntityView));

    public static SecurityOperation CompanyLegalEntityEdit { get; } = new(nameof(CompanyLegalEntityEdit));

    #endregion

    #region EmployeePosition

    public static SecurityOperation EmployeePositionView { get; } = new(nameof(EmployeePositionView));

    public static SecurityOperation EmployeePositionEdit { get; } = new(nameof(EmployeePositionEdit));

    #endregion

    #region EmployeePersonalCellPhone

    public static SecurityOperation EmployeePersonalCellPhoneView { get; } = new(nameof(EmployeePersonalCellPhoneView));

    public static SecurityOperation EmployeePersonalCellPhoneEdit { get; } = new(nameof(EmployeePersonalCellPhoneEdit));

    #endregion

    public static SecurityOperation SearchNotificationOperation { get; } = new(nameof(SearchNotificationOperation));

    #region AuthWorkflow

    public static SecurityOperation ApproveWorkflowOperation { get; } = new(nameof(ApproveWorkflowOperation));

    public static SecurityOperation ApprovingWorkflowOperation { get; } = new(nameof(ApprovingWorkflowOperation));

    #endregion
}
