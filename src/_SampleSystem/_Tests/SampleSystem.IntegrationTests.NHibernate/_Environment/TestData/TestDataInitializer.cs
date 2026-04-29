using Anch.Core;
using Anch.SecuritySystem;
using Anch.SecuritySystem.Services;
using Anch.SecuritySystem.Testing;

using Framework.AutomationCore.Settings;

using Microsoft.Extensions.Options;

using SampleSystem.Domain.Enums;
using SampleSystem.Domain.Inline;
using SampleSystem.IntegrationTests._Environment.TestData.Helpers;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.IntegrationTests._Environment.TestData;

public class TestDataInitializer(
    SampleSystemInitializer mainInitializer,
    RootAuthManager authManager,
    DataManager dataManager,
    IOptions<AutomationFrameworkSettings> settings,
    IRootImpersonateService rootImpersonateService) : IInitializer
{
    public async Task Initialize(CancellationToken cancellationToken) =>
        await rootImpersonateService
            .WithImpersonateAsync(nameof(TestDataInitializer), async () => await this.InitializeInternal(cancellationToken));

    public async Task InitializeInternal(CancellationToken cancellationToken)
    {
        await mainInitializer.InitializeAsync(cancellationToken);

        await authManager.For(nameof(TestDataInitializer)).SetAdminRoleAsync(cancellationToken);
        await authManager.For(DefaultConstants.NOTIFICATION_ADMIN).SetRoleAsync(SecurityRole.SystemIntegration, cancellationToken);
        await authManager.For(DefaultConstants.INTEGRATION_BUS).SetRoleAsync(SecurityRole.SystemIntegration, cancellationToken);

        this.FillMainData();

        var integrationTestUserName = settings.Value.IntegrationTestUserName;

        dataManager.SaveEmployee(
            id: DefaultConstants.EMPLOYEE_MY_ID,
            nameEng: new Fio { FirstName = DefaultConstants.EMPLOYEE_MY_NAME, LastName = DefaultConstants.EMPLOYEE_MY_NAME },
            login: integrationTestUserName);

        await authManager.For(integrationTestUserName).SetAdminRoleAsync(cancellationToken);

        foreach (var localAdmin in settings.Value.LocalAdmins)
        {
            dataManager.SaveEmployee(login: localAdmin);

            await authManager.For(localAdmin).SetRoleAsync(SecurityRole.Administrator, cancellationToken);
        }
    }

    private void FillMainData()
    {
        dataManager.SaveCountry(
            id: DefaultConstants.COUNTRY_RUSSIA_ID,
            name: DefaultConstants.COUNTRY_RUSSIA_NAME,
            nativeName: DefaultConstants.COUNTRY_RUSSIA_NATIVE_NAME,
            code: DefaultConstants.COUNTRY_RUSSIA_CODE,
            culture: "ru-RU");

        dataManager.SaveLocation(id: DefaultConstants.LOCATION_PARENT_ID, name: DefaultConstants.LOCATION_PARENT_NAME);

        var company = dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_NAME,
            isAdministrative: false,
            projectStartAllowed: false,
            canBeLinkedToDepartment: false,
            canBeResourcePool: false,
            needVertical: false);
        var lob = dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_LOB_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_LOB_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);

        var businessSegment = dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_BUSINESS_SEGMENT_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_BUSINESS_SEGMENT_NAME,
            possibleStartDate: PossibleStartDate.FinYearStart,
            isAdministrative: false,
            canBeResourcePool: true,
            projectStartAllowed: true,
            billProjectAreNotAllowed: false,
            startBOConfirm: true,
            possibleTransferDate: PossibleStartDate.FinYearStart,
            transferBOConfirm: true,
            needVertical: false,
            canBeIsCommission: false,
            canBeNewBusiness: false,
            possibleParents: [company]);

        var account = dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_ACCOUNT_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_ACCOUNT_NAME,
            possibleStartDate: PossibleStartDate.AnyDay,
            isAdministrative: false,
            canBeResourcePool: true,
            projectStartAllowed: true,
            billProjectAreNotAllowed: false,
            startBOConfirm: true,
            possibleTransferDate: PossibleStartDate.AnyDay,
            transferBOConfirm: true,
            needVertical: true,
            canBeIsCommission: false,
            canBeNewBusiness: true,
            possibleFinancialProjectTypes:
            [
                FinancialProjectType.Commercial, FinancialProjectType.Administrative,
                FinancialProjectType.Investment, FinancialProjectType.RnD
            ],
            possibleParents: [businessSegment, lob, company],
            transferTo: [businessSegment, lob, company]);

        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_COMMERCIAL_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_COMMERCIAL_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_SERVICES_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_SERVICES_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_DIVISION_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_DIVISION_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false,
            possibleParents: [company, account]);

        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_NEW_BUSINESS_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_NEW_BUSINESS_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: true,
            possibleParents: [company, account, businessSegment]);

        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_SERVICE_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_SERVICE_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_ADMINISTRATIVE_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_ADMINISTRATIVE_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        var seDivision = dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_DIVISION_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_DIVISION_NAME,
            possibleStartDate: PossibleStartDate.AnyDay,
            isAdministrative: false,
            canBeResourcePool: false,
            projectStartAllowed: false,
            billProjectAreNotAllowed: false,
            startBOConfirm: true,
            possibleTransferDate: PossibleStartDate.FinYearStart,
            transferBOConfirm: true,
            needVertical: false,
            canBeIsCommission: false,
            canBeNewBusiness: false,
            possibleParents: [account],
            transferTo: [account]);
        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_SUB_DIVISION_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_SE_SUB_DIVISION_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_PRACTICE_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_PRACTICE_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);

        dataManager.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_PROGRAM_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_PROGRAM_NAME,
            currentTypeInPossibleParents: true,
            currentTypeInTransferTo: true,
            possibleStartDate: PossibleStartDate.AnyDay,
            isAdministrative: false,
            canBeResourcePool: true,
            projectStartAllowed: true,
            billProjectAreNotAllowed: false,
            startBOConfirm: true,
            possibleTransferDate: PossibleStartDate.AnyDay,
            transferBOConfirm: true,
            needVertical: false,
            canBeIsCommission: true,
            canBeNewBusiness: false,
            possibleParents: [account, seDivision, businessSegment],
            possibleFinancialProjectTypes:
            [
                FinancialProjectType.Commercial, FinancialProjectType.Administrative,
                FinancialProjectType.Investment, FinancialProjectType.RnD
            ],
            transferTo: [account, seDivision]);

        dataManager.SaveEmployeePosition(
            DefaultConstants.EMPLOYEE_POSITION_TESTER_ID,
            DefaultConstants.EMPLOYEE_POSITION_TESTER_NAME);

        dataManager.SaveEmployeeRegistrationType(
            DefaultConstants.EMPLOYEE_REGISTRATION_TYPE_STAFF_ID,
            DefaultConstants.EMPLOYEE_REGISTRATION_TYPE_STAFF_NAME);

        dataManager.SaveEmployeeRole(
            DefaultConstants.EMPLOYEE_ROLE_TESTER_ID,
            DefaultConstants.EMPLOYEE_ROLE_TESTER_NAME);

        dataManager.SaveEmployeeRoleDegree(
            DefaultConstants.EMPLOYEE_ROLE_DEGREE_REGULAR_ID,
            DefaultConstants.EMPLOYEE_ROLE_DEGREE_REGULAR_NAME);

        dataManager.SaveCompanyLegalEntity(
            DefaultConstants.COMPANY_LEGAL_ENTITY_ID,
            DefaultConstants.COMPANY_LEGAL_ENTITY_NAME);

        dataManager.SaveEmployee(DefaultConstants.HRDepartment_DEFAULT_HEAD_EMPLOYEE_ID, login: "Default_HRDepartment_HEAD", isObjectRequired: false);

        dataManager.SaveHRDepartment(
            DefaultConstants.HRDEPARTMENT_PARENT_ID,
            DefaultConstants.HRDEPARTMENT_PARENT_NAME);
    }
}
