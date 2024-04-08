using Framework.Authorization;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace SampleSystem;

public static class SampleSystemSecurityOperation
{
    /// <summary>
    /// Специальная операция для отключения безопасности
    /// </summary>
    public static SecurityRule Disabled { get; } = SecurityRule.Disabled;

    #region Employee

    public static SecurityRule EmployeeView { get; } = new(nameof(EmployeeView));

    public static SecurityRule EmployeeEdit { get; } = new(nameof(EmployeeEdit));

    #endregion

    #region BusinessUnit

    public static SecurityRule BusinessUnitView { get; } = new(nameof(BusinessUnitView)) { ExpandType = HierarchicalExpandType.All };

    public static SecurityRule BusinessUnitEdit { get; } = new(nameof(BusinessUnitEdit));

    #endregion

    #region BusinessUnitType

    public static SecurityRule BusinessUnitTypeModuleOpen { get; } = new(nameof(BusinessUnitTypeModuleOpen));

    public static SecurityRule BusinessUnitTypeView { get; } = new(nameof(BusinessUnitTypeView));

    public static SecurityRule BusinessUnitTypeEdit { get; } = new(nameof(BusinessUnitTypeEdit));

    #endregion

    #region BusinessUnitManagerCommissionLink

    public static SecurityRule BusinessUnitManagerCommissionLinkView { get; } = new(nameof(BusinessUnitManagerCommissionLinkView));

    public static SecurityRule BusinessUnitManagerCommissionLinkEdit { get; } = new(nameof(BusinessUnitManagerCommissionLinkEdit));

    #endregion

    #region BusinessUnitHrDepartment

    public static SecurityRule BusinessUnitHrDepartmentView { get; } = new(nameof(BusinessUnitHrDepartmentView));

    public static SecurityRule BusinessUnitHrDepartmentEdit { get; } = new(nameof(BusinessUnitHrDepartmentEdit));

    #endregion

    #region Management Unit

    public static SecurityRule ManagementUnitView { get; } = new(nameof(ManagementUnitView)) { ExpandType = HierarchicalExpandType.All };

    public static SecurityRule ManagementUnitEdit { get; } = new(nameof(ManagementUnitEdit));

    public static SecurityRule ManagementUnitChangeView { get; } = new(nameof(ManagementUnitChangeView));

    public static SecurityRule ManagementUnitChangeEdit { get; } = new(nameof(ManagementUnitChangeEdit));

    public static SecurityRule CreateManagementUnitStart { get; } = new(nameof(CreateManagementUnitStart));

    #endregion

    #region ManagementUnitAndBusinessUnitLink

    public static SecurityRule ManagementUnitAndBusinessUnitLinkView { get; } = new(nameof(ManagementUnitAndBusinessUnitLinkView));

    public static SecurityRule ManagementUnitAndBusinessUnitLinkEdit { get; } = new(nameof(ManagementUnitAndBusinessUnitLinkEdit));

    #endregion

    #region ManagementUnitAndHRDepartmentLink

    public static SecurityRule ManagementUnitAndHRDepartmentLinkView { get; } = new(nameof(ManagementUnitAndHRDepartmentLinkView));

    public static SecurityRule ManagementUnitAndHRDepartmentLinkEdit { get; } = new(nameof(ManagementUnitAndHRDepartmentLinkEdit));

    #endregion

    #region EmployeeSpecialization

    public static SecurityRule EmployeeSpecializationOpenModule { get; } = new(nameof(EmployeeSpecializationOpenModule));

    public static SecurityRule EmployeeSpecializationView { get; } = new(nameof(EmployeeSpecializationView));

    #endregion

    #region EmployeeRole

    public static SecurityRule EmployeeRoleView { get; } = new(nameof(EmployeeRoleView));

    #endregion

    #region EmployeeRoleDegree

    public static SecurityRule EmployeeRoleDegreeView { get; } = new(nameof(EmployeeRoleDegreeView));

    #endregion

    #region HRDepartment

    public static SecurityRule HRDepartmentView { get; } = new(nameof(HRDepartmentView));

    public static SecurityRule HRDepartmentEdit { get; } = new(nameof(HRDepartmentEdit));

    public static SecurityRule HRDepartmentAdvancedEdit { get; } = new(nameof(HRDepartmentAdvancedEdit));

    #endregion

    #region Location

    public static SecurityRule LocationView { get; } = new(nameof(LocationView));

    public static SecurityRule LocationEdit { get; } = new(nameof(LocationEdit));

    #endregion

    #region Country

    public static SecurityRule CountryView { get; } = new(nameof(CountryView));

    public static SecurityRule CountryEdit { get; } = new(nameof(CountryEdit));

    #endregion

    #region CompanyLegalEntity

    public static SecurityRule CompanyLegalEntityView { get; } = new(nameof(CompanyLegalEntityView));

    public static SecurityRule CompanyLegalEntityEdit { get; } = new(nameof(CompanyLegalEntityEdit));

    #endregion

    #region EmployeePosition

    public static SecurityRule EmployeePositionView { get; } = new(nameof(EmployeePositionView));

    public static SecurityRule EmployeePositionEdit { get; } = new(nameof(EmployeePositionEdit));

    #endregion

    #region EmployeePersonalCellPhone

    public static SecurityRule EmployeePersonalCellPhoneView { get; } = new(nameof(EmployeePersonalCellPhoneView));

    public static SecurityRule EmployeePersonalCellPhoneEdit { get; } = new(nameof(EmployeePersonalCellPhoneEdit));

    #endregion

    public static SecurityRule SearchNotificationOperation { get; } = new(nameof(SearchNotificationOperation));

    public static SecurityRule AuthorizationImpersonate { get; } = AuthorizationSecurityOperation.AuthorizationImpersonate;

    #region AuthWorkflow

    public static SecurityRule ApproveWorkflowOperation { get; } = new(nameof(ApproveWorkflowOperation));

    public static SecurityRule ApprovingWorkflowOperation { get; } = new(nameof(ApprovingWorkflowOperation));

    #endregion
}
