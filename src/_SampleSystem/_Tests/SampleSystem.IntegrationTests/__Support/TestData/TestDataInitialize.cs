using System;
using System.Collections.Generic;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.__Support.Utils;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support.TestData
{
    public class TestDataInitialize
    {
        private readonly AuthHelper AuthHelper;
        private readonly DataHelper DataHelper;
        private readonly IServiceProvider RootServiceProvider;
        public TestDataInitialize(IDatabaseContext databaseContext)
        {
            this.RootServiceProvider = SampleSystemTestRootServiceProvider.Create(databaseContext);
            this.AuthHelper = this.RootServiceProvider.GetRequiredService<AuthHelper>();
            this.DataHelper = this.RootServiceProvider.GetRequiredService<DataHelper>();
        }

        public void TestData()
        {
            this.RootServiceProvider.GetRequiredService<SampleSystemInitializer>().Initialize();

            this.AuthHelper.AddCurrentUserToAdmin();

            this.AuthHelper.SetUserRole(DefaultConstants.NOTIFICATION_ADMIN, new SampleSystemPermission(BusinessRole.SystemIntegration));
            this.AuthHelper.SetUserRole(DefaultConstants.INTEGRATION_USER, new SampleSystemPermission(BusinessRole.SystemIntegration));

            this.DataHelper.SaveCountry(
                id: DefaultConstants.COUNTRY_RUSSIA_ID,
                name: DefaultConstants.COUNTRY_RUSSIA_NAME,
                nativeName: DefaultConstants.COUNTRY_RUSSIA_NATIVE_NAME,
                code: DefaultConstants.COUNTRY_RUSSIA_CODE,
                culture: "ru-RU");

            this.DataHelper.SaveLocation(id: DefaultConstants.LOCATION_PARENT_ID, name: DefaultConstants.LOCATION_PARENT_NAME);

            var company = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID, DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_NAME, isAdministrative: false, projectStartAllowed: false, canBeLinkedToDepartment: false, canBeResourcePool: false, needVertical: false);
            var lob = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_LOB_ID, DefaultConstants.BUSINESS_UNIT_TYPE_LOB_NAME, projectStartAllowed: true, canBeLinkedToDepartment: true, canBeResourcePool: true, needVertical: false);

            var businessSegment = this.DataHelper.SaveBusinessUnitType(
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
                possibleParents: new List<BusinessUnitTypeIdentityDTO> { company });

            var account = this.DataHelper.SaveBusinessUnitType(
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
                possibleFinancialProjectTypes: new List<FinancialProjectType> { FinancialProjectType.Commercial, FinancialProjectType.Administrative, FinancialProjectType.Investment, FinancialProjectType.RnD },
                possibleParents: new List<BusinessUnitTypeIdentityDTO> { businessSegment, lob, company },
                transferTo: new List<BusinessUnitTypeIdentityDTO> { businessSegment, lob, company });

            this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_COMMERCIAL_ID, DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_COMMERCIAL_NAME, projectStartAllowed: true, canBeLinkedToDepartment: true, canBeResourcePool: true, needVertical: false);
            this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_SERVICES_ID, DefaultConstants.BUSINESS_UNIT_TYPE_ADMIN_SERVICES_NAME, projectStartAllowed: true, canBeLinkedToDepartment: true, canBeResourcePool: true, needVertical: false);
            this.DataHelper.SaveBusinessUnitType(
                DefaultConstants.BUSINESS_UNIT_TYPE_DIVISION_ID,
                DefaultConstants.BUSINESS_UNIT_TYPE_DIVISION_NAME,
                projectStartAllowed: true,
                canBeLinkedToDepartment: true,
                canBeResourcePool: true,
                needVertical: false,
                possibleParents: new List<BusinessUnitTypeIdentityDTO> { company, account });

            this.DataHelper.SaveBusinessUnitType(
                DefaultConstants.BUSINESS_UNIT_TYPE_NEW_BUSINESS_ID,
                DefaultConstants.BUSINESS_UNIT_TYPE_NEW_BUSINESS_NAME,
                projectStartAllowed: true,
                canBeLinkedToDepartment: true,
                canBeResourcePool: true,
                needVertical: true,
                possibleParents: new List<BusinessUnitTypeIdentityDTO> { company, account, businessSegment });

            this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_SERVICE_ID, DefaultConstants.BUSINESS_UNIT_TYPE_SERVICE_NAME, projectStartAllowed: true, canBeLinkedToDepartment: true, canBeResourcePool: true, needVertical: false);
            this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_SE_ADMINISTRATIVE_ID, DefaultConstants.BUSINESS_UNIT_TYPE_SE_ADMINISTRATIVE_NAME, projectStartAllowed: true, canBeLinkedToDepartment: true, canBeResourcePool: true, needVertical: false);
            var seDivision = this.DataHelper.SaveBusinessUnitType(
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
               possibleParents: new List<BusinessUnitTypeIdentityDTO> { account },
               transferTo: new List<BusinessUnitTypeIdentityDTO> { account });
            this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_SE_SUB_DIVISION_ID, DefaultConstants.BUSINESS_UNIT_TYPE_SE_SE_SUB_DIVISION_NAME, projectStartAllowed: true, canBeLinkedToDepartment: true, canBeResourcePool: true, needVertical: false);
            this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_PRACTICE_ID, DefaultConstants.BUSINESS_UNIT_TYPE_PRACTICE_NAME, projectStartAllowed: true, canBeLinkedToDepartment: true, canBeResourcePool: true, needVertical: false);

            this.DataHelper.SaveBusinessUnitType(
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
                possibleParents: new List<BusinessUnitTypeIdentityDTO> { account, seDivision, businessSegment },
                possibleFinancialProjectTypes: new List<FinancialProjectType> { FinancialProjectType.Commercial, FinancialProjectType.Administrative, FinancialProjectType.Investment, FinancialProjectType.RnD },
                transferTo: new List<BusinessUnitTypeIdentityDTO> { account, seDivision });

            this.DataHelper.SaveEmployeePosition(
                DefaultConstants.EMPLOYEE_POSITION_TESTER_ID,
                DefaultConstants.EMPLOYEE_POSITION_TESTER_NAME);

            this.DataHelper.SaveEmployeeRegistrationType(
                DefaultConstants.EMPLOYEE_REGISTRATION_TYPE_STAFF_ID,
                DefaultConstants.EMPLOYEE_REGISTRATION_TYPE_STAFF_NAME);

            this.DataHelper.SaveEmployeeRole(
                DefaultConstants.EMPLOYEE_ROLE_TESTER_ID,
                DefaultConstants.EMPLOYEE_ROLE_TESTER_NAME);

            this.DataHelper.SaveEmployeeRoleDegree(
                DefaultConstants.EMPLOYEE_ROLE_DEGREE_REGULAR_ID,
                DefaultConstants.EMPLOYEE_ROLE_DEGREE_REGULAR_NAME);

            this.DataHelper.SaveEmployee(
                id: DefaultConstants.EMPLOYEE_MY_ID,
                nameEng:
                    new Fio
                    {
                        FirstName = DefaultConstants.EMPLOYEE_MY_NAME,
                        LastName = DefaultConstants.EMPLOYEE_MY_NAME
                    },
                login: DefaultConstants.EMPLOYEE_MY_LOGIN,
                isObjectRequired: false);

            this.DataHelper.SaveCompanyLegalEntity(
                DefaultConstants.COMPANY_LEGAL_ENTITY_ID,
                DefaultConstants.COMPANY_LEGAL_ENTITY_NAME);

            this.DataHelper.SaveHRDepartment(
                DefaultConstants.HRDEPARTMENT_PARENT_ID,
                DefaultConstants.HRDEPARTMENT_PARENT_NAME);
        }
    }
}
