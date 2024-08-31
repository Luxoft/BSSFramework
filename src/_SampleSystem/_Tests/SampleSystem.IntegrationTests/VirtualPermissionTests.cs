using FluentAssertions;

using Framework.DomainDriven;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class VirtualPermissionTests : TestBase
{
    [TestMethod]
    public void VirtualPermission_ObjectResolvedByVirtualPermission()
    {
        // Arrange

        var testBuId = this.DataHelper.SaveBusinessUnit().Id;

        var testEmployeeLogin = "testEmployeeLogin";

        var testEmployeeId = this.DataHelper.SaveEmployee(login: testEmployeeLogin).Id;

        this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var bu = context.Logics.BusinessUnit.GetById(testBuId, true);

                var employee = context.Logics.Employee.GetById(testEmployeeId, true);

                context.Logics
                       .Default
                       .Create<BusinessUnitEmployeeRole>()
                       .Save(new BusinessUnitEmployeeRole(bu) { Employee = employee, Role = BusinessUnitEmployeeRoleType.Manager });
                context.Logics.BusinessUnit.Save(bu);
            });

        // Act
        var accessToBuList =

            this.Evaluate(
                DBSessionMode.Read,
                testEmployeeLogin,
                ctx =>
                    ctx.Logics.BusinessUnitFactory.Create(SampleSystemSecurityRole.SeManager)
                       .GetSecureQueryable().Select(bu => bu.Id).ToList());

        // Assert
        accessToBuList.Should().ContainSingle();
        accessToBuList[0].Should().Be(testBuId);
    }
}
