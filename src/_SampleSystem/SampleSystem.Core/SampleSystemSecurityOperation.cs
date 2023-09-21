using Framework.Authorization;
using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace SampleSystem;

public static class SampleSystemSecurityOperation
{
    /// <summary>
    /// Специальная операция для отключения безопасности
    /// </summary>
    public static DisabledSecurityOperation Disabled { get; } = SecurityOperation.Disabled;

    #region Employee

    public static ContextSecurityOperation<Guid> EmployeeView { get; } = new(nameof(EmployeeView), HierarchicalExpandType.Children, new Guid("{C73DD1F6-74D5-4445-A265-2E96832A7F89}"));

    public static ContextSecurityOperation<Guid> EmployeeEdit { get; } = new(nameof(EmployeeEdit), HierarchicalExpandType.Children, new Guid("{45E8E5B8-620C-42C6-BAF5-20AB1CF27B8E}"));

    #endregion

    #region BusinessUnit

    public static ContextSecurityOperation<Guid> BusinessUnitView { get; } = new(nameof(BusinessUnitView), HierarchicalExpandType.All, new Guid("{B79E7132-845E-4BBF-8C3A-8FA3F6B31CF6}"));

    public static ContextSecurityOperation<Guid> BusinessUnitEdit { get; } = new(nameof(BusinessUnitEdit), HierarchicalExpandType.Children, new Guid("{10000000-71C4-47CD-8683-000000000003}"));

    #endregion

    #region BusinessUnitType

    public static NonContextSecurityOperation<Guid> BusinessUnitTypeModuleOpen { get; } = new(nameof(BusinessUnitTypeModuleOpen), new Guid("E4243A14-615E-40a8-809E-C7F3D6DF67B0")) { IsClient = true };

    public static NonContextSecurityOperation<Guid> BusinessUnitTypeView { get; } = new(nameof(BusinessUnitTypeView), new Guid("E4243A14-615E-40a8-809E-C7F3D6DF67B1"));

    public static NonContextSecurityOperation<Guid> BusinessUnitTypeEdit { get; } = new(nameof(BusinessUnitTypeEdit), new Guid("E4243A14-615E-40a8-809E-C7F3D6DF67B2"));

    #endregion

    #region BusinessUnitManagerCommissionLink

    public static ContextSecurityOperation<Guid> BusinessUnitManagerCommissionLinkView { get; } = new(nameof(BusinessUnitManagerCommissionLinkView), HierarchicalExpandType.Children, new Guid("1BD2AC62-9660-4A7D-AF5A-6DAEFB89E401"));

    public static ContextSecurityOperation<Guid> BusinessUnitManagerCommissionLinkEdit { get; } = new(nameof(BusinessUnitManagerCommissionLinkEdit), HierarchicalExpandType.Children, new Guid("95CBDBE6-B2DC-40BD-B7C0-F47E31B2D611"));

    #endregion

    #region BusinessUnitHrDepartment

    public static ContextSecurityOperation<Guid> BusinessUnitHrDepartmentView { get; } = new(nameof(BusinessUnitHrDepartmentView), HierarchicalExpandType.Children, new Guid("3DA438A1-6248-4290-BB90-97163E0106E0"));

    public static ContextSecurityOperation<Guid> BusinessUnitHrDepartmentEdit { get; } = new(nameof(BusinessUnitHrDepartmentEdit), HierarchicalExpandType.Children, new Guid("0A943A13-8343-4366-9E98-C5CDF743E329"));

    #endregion

    #region Management Unit

    public static ContextSecurityOperation<Guid> ManagementUnitView { get; } = new(nameof(ManagementUnitView), HierarchicalExpandType.All, new Guid("{7C64E656-71C4-47CD-8683-B1680A954531}"));

    public static ContextSecurityOperation<Guid> ManagementUnitEdit { get; } = new(nameof(ManagementUnitEdit), HierarchicalExpandType.Children, new Guid("{7C64E656-71C4-47CD-8683-B1680A954532}"));

    public static ContextSecurityOperation<Guid> ManagementUnitChangeView { get; } = new(nameof(ManagementUnitChangeView), HierarchicalExpandType.Children, new Guid("{7C64E656-71C4-47CD-8683-B1680A954534}"));

    public static ContextSecurityOperation<Guid> ManagementUnitChangeEdit { get; } = new(nameof(ManagementUnitChangeEdit), HierarchicalExpandType.Children, new Guid("{7C64E656-71C4-47CD-8683-B1680A954533}"));

    public static ContextSecurityOperation<Guid> CreateManagementUnitStart { get; } = new(nameof(CreateManagementUnitStart), HierarchicalExpandType.Children, new Guid("{7C64E656-71C4-47CD-8683-B1680A954535}"));

    #endregion

    #region ManagementUnitAndBusinessUnitLink

    public static ContextSecurityOperation<Guid> ManagementUnitAndBusinessUnitLinkView { get; } = new(nameof(ManagementUnitAndBusinessUnitLinkView), HierarchicalExpandType.Children, new Guid("00000000-71C4-47CD-8683-000000000020"));

    public static ContextSecurityOperation<Guid> ManagementUnitAndBusinessUnitLinkEdit { get; } = new(nameof(ManagementUnitAndBusinessUnitLinkEdit), HierarchicalExpandType.Children, new Guid("00000000-71C4-47CD-8683-000000000021"));

    #endregion

    #region ManagementUnitAndHRDepartmentLink

    public static ContextSecurityOperation<Guid> ManagementUnitAndHRDepartmentLinkView { get; } = new(nameof(ManagementUnitAndHRDepartmentLinkView), HierarchicalExpandType.Children, new Guid("00000000-71C4-47CD-8683-000000000022"));

    public static ContextSecurityOperation<Guid> ManagementUnitAndHRDepartmentLinkEdit { get; } = new(nameof(ManagementUnitAndHRDepartmentLinkEdit), HierarchicalExpandType.Children, new Guid("00000000-71C4-47CD-8683-000000000023"));

    #endregion

    #region EmployeeSpecialization

    public static NonContextSecurityOperation<Guid> EmployeeSpecializationOpenModule { get; } = new(nameof(EmployeeSpecializationOpenModule), new Guid("00000000-71C4-47CD-8683-000000000003"));

    public static NonContextSecurityOperation<Guid> EmployeeSpecializationView { get; } = new(nameof(EmployeeSpecializationView), new Guid("00000000-71C4-47CD-8683-000000000004"));

    #endregion

    #region EmployeeRole

    public static NonContextSecurityOperation<Guid> EmployeeRoleView { get; } = new(nameof(EmployeeRoleView), new Guid("00000000-71C4-47CD-8683-000000000007"));

    #endregion

    #region EmployeeRoleDegree

    public static NonContextSecurityOperation<Guid> EmployeeRoleDegreeView { get; } = new(nameof(EmployeeRoleDegreeView), new Guid("00000000-71C4-47CD-8683-000000000010"));

    #endregion

    #region HRDepartment

    public static NonContextSecurityOperation<Guid> HRDepartmentView { get; } = new(nameof(HRDepartmentView), new Guid("{00000000-71C4-47CD-8683-000000000001}"));

    public static NonContextSecurityOperation<Guid> HRDepartmentEdit { get; } = new(nameof(HRDepartmentEdit), new Guid("{00000000-71C4-47CD-8683-000000000002}"));

    public static NonContextSecurityOperation<Guid> HRDepartmentAdvancedEdit { get; } = new(nameof(HRDepartmentAdvancedEdit), new Guid("{F75385D0-BAF3-4689-B0CE-4E7998B9957C}"));

    #endregion

    #region Location

    public static NonContextSecurityOperation<Guid> LocationView { get; } = new(nameof(LocationView), new Guid("E5377866-FF6D-4D05-912F-2D3C72F27FA7"));

    public static NonContextSecurityOperation<Guid> LocationEdit { get; } = new(nameof(LocationEdit), new Guid("034C4E00-9C62-422B-98B8-B119C1991596"));

    #endregion

    #region Country

    public static NonContextSecurityOperation<Guid> CountryView { get; } = new(nameof(CountryView), new Guid("{D2171DC7-5D6E-4FF4-BF2C-CBC5B7ABFB51}"));

    public static NonContextSecurityOperation<Guid> CountryEdit { get; } = new(nameof(CountryEdit), new Guid("{EE81E81B-DBB6-47E7-8B13-9AC20CA1B730}"));

    #endregion

    #region CompanyLegalEntity

    public static NonContextSecurityOperation<Guid> CompanyLegalEntityView { get; } = new(nameof(CompanyLegalEntityView), new Guid("00000000-71C4-47CD-8683-000000000015"));

    public static NonContextSecurityOperation<Guid> CompanyLegalEntityEdit { get; } = new(nameof(CompanyLegalEntityEdit), new Guid("00000000-71C4-47CD-8683-000000000016"));

    #endregion

    #region EmployeePosition

    public static ContextSecurityOperation<Guid> EmployeePositionView { get; } = new(nameof(EmployeePositionView), HierarchicalExpandType.Children, new Guid("C6FC2405-403C-4885-A2FD-1E53796D5FC3"));

    public static ContextSecurityOperation<Guid> EmployeePositionEdit { get; } = new(nameof(EmployeePositionEdit), HierarchicalExpandType.Children, new Guid("0AD8A590-DC02-4EE3-BC55-3B9E0655EB5C"));

    #endregion

    #region EmployeePersonalCellPhone

    public static NonContextSecurityOperation<Guid> EmployeePersonalCellPhoneView { get; } = new(nameof(EmployeePersonalCellPhoneView), new Guid("{EF42631D-0B49-4418-A3A7-4EE413ACEDF0}"));

    public static NonContextSecurityOperation<Guid> EmployeePersonalCellPhoneEdit { get; } = new(nameof(EmployeePersonalCellPhoneEdit), new Guid("{A30BB9DE-578F-485B-BFFF-1CE6256F34F6}"));

    #endregion

    public static NonContextSecurityOperation<Guid> SearchNotificationOperation { get; } = new(nameof(SearchNotificationOperation), new Guid("{E66C1E91-6290-4192-BDE7-074634562288}"));

    public static NonContextSecurityOperation<Guid> AuthorizationImpersonate { get; } = AuthorizationSecurityOperation.AuthorizationImpersonate;

    public static NonContextSecurityOperation<Guid> SystemIntegration { get; } = BssSecurityOperation.SystemIntegration;

    #region AuthWorkflow

    public static NonContextSecurityOperation<Guid> ApproveWorkflowOperation { get; } = new(nameof(ApproveWorkflowOperation), new Guid("{939EC98C-131B-4E3E-B97C-9DF95620C758}")) { AdminHasAccess = false };

    public static NonContextSecurityOperation<Guid> ApprovingWorkflowOperation { get; } = new(nameof(ApprovingWorkflowOperation), new Guid("{927E4AFC-8CC2-4EDA-B6EE-FE6B2C53D0BA}")) { ApproveOperation = ApproveWorkflowOperation };

    #endregion
}
