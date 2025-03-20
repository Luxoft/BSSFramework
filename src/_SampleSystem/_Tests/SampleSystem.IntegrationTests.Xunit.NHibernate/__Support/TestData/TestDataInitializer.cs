using Automation.Extensions;
using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.SecuritySystem;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.Xunit.__Support.TestData;

public class TestDataInitializer
{
    private readonly IServiceProvider serviceProvider;

    private readonly DataHelper dataHelper;

    public TestDataInitializer(
        IConfiguration configuration,
        IDatabaseContext databaseContext)
    {
        var generatorConfig = configuration.BuildFromRootWithConnectionStrings(databaseContext);

        this.serviceProvider = new EnvironmentInitialization()
            .ConfigureTestEnvironment(
                new ServiceCollection()
                    .AddSingleton(databaseContext)
                    .AddSingleton(generatorConfig),
                generatorConfig);

        this.dataHelper = this.serviceProvider.GetRequiredService<DataHelper>();
    }

    public async Task InitializeAsync(CancellationToken cancellationToken) =>
        await this.serviceProvider
                  .GetRequiredService<IIntegrationTestUserAuthenticationService>()
                  .WithImpersonateAsync(
                      nameof(TestDataInitializer),
                      async () => await this.InitializeAsyncInternal(cancellationToken));

    public async Task InitializeAsyncInternal(CancellationToken cancellationToken)
    {
        await this.serviceProvider.GetRequiredService<SampleSystemInitializer>().InitializeAsync(cancellationToken);
        var authManager = this.serviceProvider.GetRequiredService<RootAuthManager>();

        await authManager.For(nameof(TestDataInitializer)).SetAdminRoleAsync(cancellationToken);
        await authManager.For(DefaultConstants.NOTIFICATION_ADMIN).SetRoleAsync([SecurityRole.SystemIntegration], cancellationToken);
        await authManager.For(DefaultConstants.INTEGRATION_BUS).SetRoleAsync([SecurityRole.SystemIntegration], cancellationToken);

        this.FillMainData();

        var integrationTestUserName = this.serviceProvider.GetRequiredService<IOptions<AutomationFrameworkSettings>>()
                                          .Value.IntegrationTestUserName;

        this.dataHelper.SaveEmployee(
            id: DefaultConstants.EMPLOYEE_MY_ID,
            nameEng: new Fio { FirstName = DefaultConstants.EMPLOYEE_MY_NAME, LastName = DefaultConstants.EMPLOYEE_MY_NAME },
            login: integrationTestUserName);

        await authManager.For(integrationTestUserName).SetAdminRoleAsync(cancellationToken);
    }

    private void FillMainData()
    {
        this.dataHelper.SaveCountry(
            id: DefaultConstants.COUNTRY_RUSSIA_ID,
            name: DefaultConstants.COUNTRY_RUSSIA_NAME,
            nativeName: DefaultConstants.COUNTRY_RUSSIA_NATIVE_NAME,
            code: DefaultConstants.COUNTRY_RUSSIA_CODE,
            culture: "ru-RU");

        this.dataHelper.SaveLocation(id: DefaultConstants.LOCATION_PARENT_ID, name: DefaultConstants.LOCATION_PARENT_NAME);

        var company = this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_NAME,
            isAdministrative: false,
            projectStartAllowed: false,
            canBeLinkedToDepartment: false,
            canBeResourcePool: false,
            needVertical: false);
        var lob = this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_LOB_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_LOB_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);

        var businessSegment = this.dataHelper.SaveBusinessUnitType(
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

        var account = this.dataHelper.SaveBusinessUnitType(
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

        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_COMMERCIAL_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_COMMERCIAL_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_SERVICES_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_SERVICES_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_DIVISION_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_DIVISION_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false,
            possibleParents: [company, account]);

        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_NEW_BUSINESS_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_NEW_BUSINESS_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: true,
            possibleParents: [company, account, businessSegment]);

        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_SERVICE_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_SERVICE_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_ADMINISTRATIVE_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_ADMINISTRATIVE_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        var seDivision = this.dataHelper.SaveBusinessUnitType(
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
        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_SUB_DIVISION_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_SE_SE_SUB_DIVISION_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);
        this.dataHelper.SaveBusinessUnitType(
            DefaultConstants.BUSINESS_UNIT_TYPE_PRACTICE_ID,
            DefaultConstants.BUSINESS_UNIT_TYPE_PRACTICE_NAME,
            projectStartAllowed: true,
            canBeLinkedToDepartment: true,
            canBeResourcePool: true,
            needVertical: false);

        this.dataHelper.SaveBusinessUnitType(
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

        this.dataHelper.SaveEmployeePosition(
            DefaultConstants.EMPLOYEE_POSITION_TESTER_ID,
            DefaultConstants.EMPLOYEE_POSITION_TESTER_NAME);

        this.dataHelper.SaveEmployeeRegistrationType(
            DefaultConstants.EMPLOYEE_REGISTRATION_TYPE_STAFF_ID,
            DefaultConstants.EMPLOYEE_REGISTRATION_TYPE_STAFF_NAME);

        this.dataHelper.SaveEmployeeRole(
            DefaultConstants.EMPLOYEE_ROLE_TESTER_ID,
            DefaultConstants.EMPLOYEE_ROLE_TESTER_NAME);

        this.dataHelper.SaveEmployeeRoleDegree(
            DefaultConstants.EMPLOYEE_ROLE_DEGREE_REGULAR_ID,
            DefaultConstants.EMPLOYEE_ROLE_DEGREE_REGULAR_NAME);

        this.dataHelper.SaveCompanyLegalEntity(
            DefaultConstants.COMPANY_LEGAL_ENTITY_ID,
            DefaultConstants.COMPANY_LEGAL_ENTITY_NAME);

        this.dataHelper.SaveEmployee(DefaultConstants.HRDepartment_DEFAULT_HEAD_EMPLOYEE_ID, login: "Default_HRDepartment_HEAD", isObjectRequired: false);

        this.dataHelper.SaveHRDepartment(
            DefaultConstants.HRDEPARTMENT_PARENT_ID,
            DefaultConstants.HRDEPARTMENT_PARENT_NAME);
    }
}
