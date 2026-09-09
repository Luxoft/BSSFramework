using FluentMigrator;

namespace SampleSystem.DbGenerate.Migrations;

[Migration(4)]
public class AddProjectionViews : Migration
{
    public override void Up()
    {
        this.Execute.Sql(@"
CREATE VIEW [app].[BusinessUnitIdentity]
AS
SELECT [Id]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestBusinessUnit]
AS
SELECT [Id], [Name], [periodendDate], [parentId]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestBusinessUnit_AutoProp_Parent]
AS
SELECT [Id], [periodstartDate], [periodendDate]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestEmployee]
AS
SELECT [Id], [nameEngfirstName], [Login], [coreBusinessUnitId], [positionId], [ppmId], [roleId]
FROM [app].[Employee]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestEmployee_AutoProp_Role]
AS
SELECT [Id], [Name]
FROM [app].[EmployeeRole]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestEmployee_AutoProp_Ppm]
AS
SELECT [Id], [nameNativemiddleName]
FROM [app].[Employee]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestEmployee_AutoProp_Position]
AS
SELECT [Id], [Name]
FROM [app].[EmployeePosition]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestEmployee_AutoProp_CoreBusinessUnit]
AS
SELECT [Id], [Name], [periodendDate]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestDepartment]
AS
SELECT [Id], [locationId]
FROM [app].[HRDepartment]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestDepartment_AutoProp_Location]
AS
SELECT [Id], [BinaryData]
FROM [app].[Location]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestLocation]
AS
SELECT [Id], [parentId]
FROM [app].[Location]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestLocationCollectionProperties]
AS
SELECT [Id]
FROM [app].[Location]");

        this.Execute.Sql(@"
CREATE VIEW [app].[HerBusinessUnit]
AS
SELECT [Id], [Name], [parentId]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[MiniBusinessUnitEmployeeRole]
AS
SELECT [Id], [businessUnitId], [employeeId]
FROM [app].[BusinessUnitEmployeeRole]");

        this.Execute.Sql(@"
CREATE VIEW [app].[VisualEmployee]
AS
SELECT [Id], [nameEngfirstName], [nameEnglastName]
FROM [app].[Employee]");

        this.Execute.Sql(@"
CREATE VIEW [app].[VisualProject]
AS
SELECT [Id], [businessUnitId]
FROM [app].[Project]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestBusinessUnitType]
AS
SELECT [Id], [Name]
FROM [app].[BusinessUnitType]");

        this.Execute.Sql(@"
CREATE VIEW [app].[CustomCompanyLegalEntity]
AS
SELECT [Id]
FROM [app].[CompanyLegalEntity]");

        this.Execute.Sql(@"
CREATE VIEW [app].[CustomTestObjForNested]
AS
SELECT [Id], [name], [periodstartDate]
FROM [app].[TestObjForNestedBase]");

        this.Execute.Sql(@"
CREATE VIEW [app].[EmployeeWithBuPeriod]
AS
SELECT [Id], [coreBusinessUnitId]
FROM [app].[Employee]");

        this.Execute.Sql(@"
CREATE VIEW [app].[EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit]
AS
SELECT [Id], [periodendDate], [periodstartDate]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[BusinessUnitProgramClass]
AS
SELECT [Id], [IsNewBusiness], [Name], [periodendDate], [businessUnitTypeId]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[BusinessUnitProgramClass_AutoProp_BusinessUnitType]
AS
SELECT [Id]
FROM [app].[BusinessUnitType]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestSecurityObjItemProjection]
AS
SELECT [Id], [Name]
FROM [app].[TestSecurityObjItem]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestCustomContextSecurityObjProjection]
AS
SELECT [Id]
FROM [app].[TestCustomContextSecurityObj]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestIMRequest]
AS
SELECT [Id]
FROM [app].[IMRequest]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestIMRequestDetail]
AS
SELECT [Id], [requestId]
FROM [app].[IMRequestDetail]");

        this.Execute.Sql(@"
CREATE VIEW [app].[SecurityBusinessUnit]
AS
SELECT [Id]
FROM [app].[BusinessUnit]");

        this.Execute.Sql(@"
CREATE VIEW [app].[SecurityHRDepartment]
AS
SELECT [Id], [locationId]
FROM [app].[HRDepartment]");

        this.Execute.Sql(@"
CREATE VIEW [app].[SecurityEmployee]
AS
SELECT [Id], [Login], [coreBusinessUnitId], [hRDepartmentId]
FROM [app].[Employee]");

        this.Execute.Sql(@"
CREATE VIEW [app].[SecurityLocation]
AS
SELECT [Id]
FROM [app].[Location]");

        this.Execute.Sql(@"
CREATE VIEW [app].[AnotherSqlParserTestObj]
AS
SELECT [Id], [Version], [Active], [CreateDate], [CreatedBy], [ModifiedBy], [ModifyDate]
FROM [app].[SqlParserTestObj]");

        this.Execute.Sql(@"
CREATE VIEW [app].[ConcreteClassA]
AS
SELECT [Id], [Value] AS [age], [Id] AS [parentId]
FROM [app].[ClassA]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestManualEmployeeProjection]
AS
SELECT [Id]
FROM [app].[Employee]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestLegacyEmployee]
AS
SELECT [Id], [roleId]
FROM [app].[Employee]");

        this.Execute.Sql(@"
CREATE VIEW [app].[TestLegacyEmployee_AutoProp_Role]
AS
SELECT [Id], [Name]
FROM [app].[EmployeeRole]");
    }

    public override void Down()
    {
        this.Execute.Sql("DROP VIEW [app].[TestLegacyEmployee_AutoProp_Role]");
        this.Execute.Sql("DROP VIEW [app].[TestLegacyEmployee]");
        this.Execute.Sql("DROP VIEW [app].[TestManualEmployeeProjection]");
        this.Execute.Sql("DROP VIEW [app].[ConcreteClassA]");
        this.Execute.Sql("DROP VIEW [app].[AnotherSqlParserTestObj]");
        this.Execute.Sql("DROP VIEW [app].[SecurityLocation]");
        this.Execute.Sql("DROP VIEW [app].[SecurityEmployee]");
        this.Execute.Sql("DROP VIEW [app].[SecurityHRDepartment]");
        this.Execute.Sql("DROP VIEW [app].[SecurityBusinessUnit]");
        this.Execute.Sql("DROP VIEW [app].[TestIMRequestDetail]");
        this.Execute.Sql("DROP VIEW [app].[TestIMRequest]");
        this.Execute.Sql("DROP VIEW [app].[TestCustomContextSecurityObjProjection]");
        this.Execute.Sql("DROP VIEW [app].[TestSecurityObjItemProjection]");
        this.Execute.Sql("DROP VIEW [app].[BusinessUnitProgramClass_AutoProp_BusinessUnitType]");
        this.Execute.Sql("DROP VIEW [app].[BusinessUnitProgramClass]");
        this.Execute.Sql("DROP VIEW [app].[EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit]");
        this.Execute.Sql("DROP VIEW [app].[EmployeeWithBuPeriod]");
        this.Execute.Sql("DROP VIEW [app].[CustomTestObjForNested]");
        this.Execute.Sql("DROP VIEW [app].[CustomCompanyLegalEntity]");
        this.Execute.Sql("DROP VIEW [app].[TestBusinessUnitType]");
        this.Execute.Sql("DROP VIEW [app].[VisualProject]");
        this.Execute.Sql("DROP VIEW [app].[VisualEmployee]");
        this.Execute.Sql("DROP VIEW [app].[MiniBusinessUnitEmployeeRole]");
        this.Execute.Sql("DROP VIEW [app].[HerBusinessUnit]");
        this.Execute.Sql("DROP VIEW [app].[TestLocationCollectionProperties]");
        this.Execute.Sql("DROP VIEW [app].[TestLocation]");
        this.Execute.Sql("DROP VIEW [app].[TestDepartment_AutoProp_Location]");
        this.Execute.Sql("DROP VIEW [app].[TestDepartment]");
        this.Execute.Sql("DROP VIEW [app].[TestEmployee_AutoProp_CoreBusinessUnit]");
        this.Execute.Sql("DROP VIEW [app].[TestEmployee_AutoProp_Position]");
        this.Execute.Sql("DROP VIEW [app].[TestEmployee_AutoProp_Ppm]");
        this.Execute.Sql("DROP VIEW [app].[TestEmployee_AutoProp_Role]");
        this.Execute.Sql("DROP VIEW [app].[TestEmployee]");
        this.Execute.Sql("DROP VIEW [app].[TestBusinessUnit_AutoProp_Parent]");
        this.Execute.Sql("DROP VIEW [app].[TestBusinessUnit]");
        this.Execute.Sql("DROP VIEW [app].[BusinessUnitIdentity]");
    }
}
