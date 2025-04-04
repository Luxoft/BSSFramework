﻿using Framework.Core;
using Framework.DomainDriven;

using Framework.SecuritySystem;
using Framework.SecuritySystem.Services;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class ExtraQueryableSecurityPathTests : TestBase
{
    private EmployeeIdentityDTO TestEmployee;

    private EmployeeIdentityDTO TestEmp1;

    private EmployeeIdentityDTO TestEmp2;

    private EmployeeIdentityDTO TestEmp3;

    private BusinessUnitIdentityDTO bu1Ident;

    private BusinessUnitIdentityDTO bu2Ident;

    private LocationIdentityDTO loc1Ident;

    private LocationIdentityDTO loc2Ident;

    [TestInitialize]
    public void SetUp()
    {
        this.bu1Ident = this.DataHelper.SaveBusinessUnit();

        this.bu2Ident = this.DataHelper.SaveBusinessUnit();

        this.loc1Ident = this.DataHelper.SaveLocation(name: "Loc 1 (ExtraQueryableSecurityPathTests)");

        this.loc2Ident = this.DataHelper.SaveLocation(name: "Loc 2 (ExtraQueryableSecurityPathTests)");

        this.TestEmployee = this.DataHelper.SaveEmployee(login: "EQSP SecurityTester");

        this.AuthManager.For(this.TestEmployee.Id).SetRole(
            new SampleSystemTestPermission(SampleSystemSecurityRole.SeManager, this.bu2Ident, null, this.loc1Ident),
            new SampleSystemTestPermission(SampleSystemSecurityRole.SeManager, this.bu2Ident, null, this.loc2Ident));

        this.TestEmp1 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu1Ident, location: this.loc1Ident);

        this.TestEmp2 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu2Ident, location: this.loc1Ident);

        this.TestEmp3 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu2Ident, location: this.loc2Ident);
    }

    [TestMethod]
    public void TestExtraQueryableSecurityPath_LoadedWithExtraQueryableFilter()
    {
        // Arrange
        var createProviderFunc = FuncHelper.Create(
            (ISampleSystemBLLContext context) =>
            {
                var extraQueryableSecurity = context.Logics.Location.GetUnsecureQueryable();

                var extraSecurityPath = SecurityPath<Employee>.Create(e => e.CoreBusinessUnit, true)
                                                              .And(e => e.Location, true)
                                                              .And(
                                                                  e => extraQueryableSecurity.Where(
                                                                      l => l == e.Location && e.Location.Id == this.loc1Ident.Id), true);

                return context.ServiceProvider.GetRequiredService<IDomainSecurityProviderFactory<Employee>>().Create(
                    SampleSystemSecurityOperation.EmployeeView,
                    extraSecurityPath);
            });

        // Act
        var items = this.Evaluate(
            DBSessionMode.Read,
            this.TestEmployee.Id,
            context =>
            {
                var employees = context.Logics.EmployeeFactory.Create(createProviderFunc(context)).GetSecureQueryable().ToList();

                return employees.ToArray(e => e.Id);
            });

        // Assert
        items.Count().Should().Be(1);
        items[0].Should().Be(this.TestEmp2.Id);
    }
}
