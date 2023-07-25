using Framework.Security;

namespace SampleSystem;

[BaseSecurityOperationType]
public enum SampleSystemSecurityOperationCode
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

    #region BusinessUnitType

    [SecurityOperation("ModuleOpen", false, "E4243A14-615E-40a8-809E-C7F3D6DF67B0", DomainType = "BusinessUnitType", IsClient = true)]
    BusinessUnitTypeModuleOpen,

    [SecurityOperation("BusinessUnitTypeView", false, "E4243A14-615E-40a8-809E-C7F3D6DF67B1", DomainType = "BusinessUnitType")]
    BusinessUnitTypeView,

    [SecurityOperation("BusinessUnitTypeEdit", false, "E4243A14-615E-40a8-809E-C7F3D6DF67B2", DomainType = "BusinessUnitType")]
    BusinessUnitTypeEdit,

    #endregion

    #region BusinessUnitManagerCommissionLink

    [SecurityOperation("BusinessUnit", true, "1BD2AC62-9660-4A7D-AF5A-6DAEFB89E401", DomainType = "BusinessUnitManagerCommissionLink")]
    BusinessUnitManagerCommissionLinkView,

    [SecurityOperation("BusinessUnit", true, "95CBDBE6-B2DC-40BD-B7C0-F47E31B2D611", DomainType = "BusinessUnitManagerCommissionLink")]
    BusinessUnitManagerCommissionLinkEdit,

    #endregion

    #region BusinessUnitHrDepartment

    [SecurityOperation("BusinessUnitHrDepartment View", true, "3DA438A1-6248-4290-BB90-97163E0106E0", DomainType = "BusinessUnitHrDepartment")]
    BusinessUnitHrDepartmentView,

    [SecurityOperation("BusinessUnitHrDepartment Edit", true, "0A943A13-8343-4366-9E98-C5CDF743E329", DomainType = "BusinessUnitHrDepartment")]
    BusinessUnitHrDepartmentEdit,

    #endregion

    #region Management Unit

    [SecurityOperation("Management Unit ModuleOpen", false, "{7C64E656-71C4-47CD-8683-B1680A954530}", DomainType = "ManagementUnit", IsClient = true)]
    ManagementUnitOpenModule,

    [SecurityOperation("Management Unit View", true, "{7C64E656-71C4-47CD-8683-B1680A954531}", DomainType = "ManagementUnit")]
    ManagementUnitView,

    [SecurityOperation("Management Unit Edit", true, "{7C64E656-71C4-47CD-8683-B1680A954532}", DomainType = "ManagementUnit")]
    ManagementUnitEdit,

    [SecurityOperation("Management Unit ChangeEdit", true, "{7C64E656-71C4-47CD-8683-B1680A954533}", "Edit Management Business Unit Change", DomainType = "ManagementUnitChange")]
    ManagementUnitChangeEdit,

    [SecurityOperation("Management Unit ChangeView", true, "{7C64E656-71C4-47CD-8683-B1680A954534}", "View Management Business Unit Change", DomainType = "ManagementUnitChange")]
    ManagementUnitChangeView,

    [SecurityOperation("Create ManagementStart", true, "{7C64E656-71C4-47CD-8683-B1680A954535}")]
    CreateManagementUnitStart,

    #endregion

    #region ManagementUnitAndBusinessUnitLink

    [SecurityOperation("ManagementUnitAndBusinessUnitLink", true, "00000000-71C4-47CD-8683-000000000020", DomainType = "ManagementUnitAndBusinessUnitLink")]
    ManagementUnitAndBusinessUnitLinkView,

    [SecurityOperation("ManagementUnitAndBusinessUnitLink", true, "00000000-71C4-47CD-8683-000000000021", DomainType = "ManagementUnitAndBusinessUnitLink")]
    ManagementUnitAndBusinessUnitLinkEdit,

    #endregion

    #region ManagementUnitAndHRDepartmentLink

    [SecurityOperation("ManagementUnitAndHRDepartmentLink View", true, "00000000-71C4-47CD-8683-000000000022", DomainType = "ManagementUnitAndHRDepartmentLink")]
    ManagementUnitAndHRDepartmentLinkView,

    [SecurityOperation("ManagementUnitAndHRDepartmentLink Edit", true, "00000000-71C4-47CD-8683-000000000023", DomainType = "ManagementUnitAndHRDepartmentLink")]
    ManagementUnitAndHRDepartmentLinkEdit,

    #endregion

    #region EmployeeSpecialization

    [SecurityOperation("EmployeeSpecialization ModuleOpen", false, "00000000-71C4-47CD-8683-000000000003", DomainType = "EmployeeSpecialization", IsClient = true)]
    EmployeeSpecializationOpenModule,

    [SecurityOperation("EmployeeSpecialization View", false, "00000000-71C4-47CD-8683-000000000004", DomainType = "EmployeeSpecialization")]
    EmployeeSpecializationView,

    #endregion

    #region EmployeeRole

    [SecurityOperation("EmployeeRole ModuleOpen", false, "00000000-71C4-47CD-8683-000000000006", DomainType = "EmployeeRole", IsClient = true)]
    EmployeeRoleOpenModule,

    [SecurityOperation("EmployeeRole View", false, "00000000-71C4-47CD-8683-000000000007", DomainType = "EmployeeRole")]
    EmployeeRoleView,


    #endregion

    #region EmployeeRoleDegree

    [SecurityOperation("EmployeeRoleDegree ModuleOpen", false, "00000000-71C4-47CD-8683-000000000009", DomainType = "EmployeeRoleDegree", IsClient = true)]
    EmployeeRoleDegreeOpenModule,

    [SecurityOperation("EmployeeRoleDegree View", false, "00000000-71C4-47CD-8683-000000000010", DomainType = "EmployeeRoleDegree")]
    EmployeeRoleDegreeView,

    #endregion

    #region HRDepartment

    [SecurityOperation("HRDepartmentOpenModule", false, "{00000000-71C4-47CD-8683-000000000000}", DomainType = "HRDepartment", IsClient = true)]
    HRDepartmentOpenModule,

    [SecurityOperation("HRDepartmentView", false, "{00000000-71C4-47CD-8683-000000000001}", DomainType = "HRDepartment")]
    HRDepartmentView,

    [SecurityOperation("HRDepartmentEdit", false, "{00000000-71C4-47CD-8683-000000000002}", DomainType = "HRDepartment")]
    HRDepartmentEdit,

    [SecurityOperation("HRDepartmentAdvancedEdit", false, "{F75385D0-BAF3-4689-B0CE-4E7998B9957C}", DomainType = "HRDepartment")]
    HRDepartmentAdvancedEdit,

    #endregion

    #region Location

    [SecurityOperation("LocationOpenModule", false, "FC5B0FCD-B38B-4CC7-AE57-B38DDFB1B748", DomainType = "Location")]
    LocationOpenModule,

    [SecurityOperation("LocationView", false, "E5377866-FF6D-4D05-912F-2D3C72F27FA7", DomainType = "Location")]
    LocationView,

    [SecurityOperation("LocationEdit", false, "034C4E00-9C62-422B-98B8-B119C1991596", DomainType = "Location")]
    LocationEdit,

    #endregion

    #region Country

    [SecurityOperation("Country", false, "{1040b424-576f-410a-b466-bc49cdc536f6}", DomainType = "Country", IsClient = true)]
    CountryOpenModule,

    [SecurityOperation("Country", false, "{D2171DC7-5D6E-4FF4-BF2C-CBC5B7ABFB51}", DomainType = "Country")]
    CountryView,

    [SecurityOperation("Country", false, "{EE81E81B-DBB6-47E7-8B13-9AC20CA1B730}", DomainType = "Country")]
    CountryEdit,

    #endregion

    #region CompanyLegalEntity

    [SecurityOperation("CompanyLegalEntity ModuleOpen", false, "00000000-71C4-47CD-8683-000000000014", DomainType = "CompanyLegalEntity", IsClient = true)]
    CompanyLegalEntityOpenModule,

    [SecurityOperation("CompanyLegalEntity View", false, "00000000-71C4-47CD-8683-000000000015", DomainType = "CompanyLegalEntity")]
    CompanyLegalEntityView,

    [SecurityOperation("CompanyLegalEntity Edit", false, "00000000-71C4-47CD-8683-000000000016", DomainType = "CompanyLegalEntity")]
    CompanyLegalEntityEdit,

    #endregion

    #region EmployeePosition

    [SecurityOperation("EmployeePosition ModuleOpen", false, "27B5C122-F507-46F9-A461-792BCA10AB18", DomainType = "EmployeePosition", IsClient = true)]
    EmployeePositionOpenModule,

    [SecurityOperation("EmployeePosition View", true, "C6FC2405-403C-4885-A2FD-1E53796D5FC3", DomainType = "EmployeePosition")]
    EmployeePositionView,

    [SecurityOperation("EmployeePosition Edit", true, "0AD8A590-DC02-4EE3-BC55-3B9E0655EB5C", DomainType = "EmployeePosition")]
    EmployeePositionEdit,

    #endregion


    [SecurityOperation("SearchNotification", false, "{E66C1E91-6290-4192-BDE7-074634562288}", nameof(SearchNotificationOperation), adminHasAccess: false)]
    SearchNotificationOperation,

    [SecurityOperation(SecurityOperationCode.AuthorizationImpersonate)]
    AuthorizationImpersonate,

    [SecurityOperation("EmployeePersonalCellPhoneView", false, "{EF42631D-0B49-4418-A3A7-4EE413ACEDF0}", "EmployeePersonalCellPhoneView", DomainType = "Employee")]
    EmployeePersonalCellPhoneView,

    [SecurityOperation("EmployeePersonalCellPhoneEdit", false, "{A30BB9DE-578F-485B-BFFF-1CE6256F34F6}", "EmployeePersonalCellPhoneEdit", DomainType = "Employee")]
    EmployeePersonalCellPhoneEdit,

    [SecurityOperation(SecurityOperationCode.SystemIntegration)]
    SystemIntegration,

    #region AuthWorkflow

    [SecurityOperation("TestWorkflow", false, "{939EC98C-131B-4E3E-B97C-9DF95620C758}", "Required operation for approve", adminHasAccess: false)]
    ApproveWorkflowOperation,

    [SampleSystemApproveOperation(ApproveWorkflowOperation)]
    [SecurityOperation("TestWorkflow", false, "{927E4AFC-8CC2-4EDA-B6EE-FE6B2C53D0BA}", "Operation testing workflow")]
    ApprovingWorkflowOperation

    #endregion
}
