using FluentAssertions;

using Framework.DomainDriven;
using Framework.SecuritySystem.SecurityAccessor;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class VirtualPermissionTests : TestBase
{
    private readonly string testEmployeeLogin = "testEmployeeLogin";

    private Guid testBuId;

    private Guid testEmployeeId;


    [TestInitialize]
    public void SetUp()
    {
        this.testBuId = this.DataHelper.SaveBusinessUnit().Id;

        this.testEmployeeId = this.DataHelper.SaveEmployee(login: this.testEmployeeLogin).Id;

        this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var bu = context.Logics.BusinessUnit.GetById(this.testBuId, true);

                var employee = context.Logics.Employee.GetById(this.testEmployeeId, true);

                context.Logics
                       .Default
                       .Create<BusinessUnitEmployeeRole>()
                       .Save(new BusinessUnitEmployeeRole(bu) { Employee = employee, Role = BusinessUnitEmployeeRoleType.Manager });
            });
    }

    [TestMethod]
    public void VirtualPermission_EmployeeWithLink_HasAccessByVirtualPermission()
    {
        // Arrange

        // Act
        var accessToBuList =

            this.Evaluate(
                DBSessionMode.Read,
                this.testEmployeeLogin,
                ctx =>
                    ctx.Logics.BusinessUnitFactory.Create(SampleSystemSecurityRole.SeManager)
                       .GetSecureQueryable().Select(bu => bu.Id).ToList());

        // Assert
        accessToBuList.Should().ContainSingle();
        accessToBuList[0].Should().Be(this.testBuId);
    }

    [TestMethod]
    public void VirtualPermission_EmployeeWithLink_ResolvedByAccessors()
    {
        // Arrange

        // Act
        var accessorList =

            this.Evaluate(
                DBSessionMode.Read,
                this.testEmployeeLogin,
                ctx =>
                {
                    var bu = ctx.Logics.BusinessUnit.GetById(this.testBuId);

                    var accessorData = ctx.SecurityService.GetSecurityProvider<BusinessUnit>(SampleSystemSecurityRole.SeManager)
                                          .GetAccessorData(bu);

                    return ctx.ServiceProvider
                              .GetRequiredService<ISecurityAccessorDataEvaluator>()
                              .Evaluate(accessorData);
                });

        // Assert
        accessorList.Should().Contain(this.testEmployeeLogin);
    }
}
