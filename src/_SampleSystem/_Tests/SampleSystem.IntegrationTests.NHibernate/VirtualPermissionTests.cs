using Framework.DomainDriven;
using Framework.SecuritySystem.SecurityAccessor;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class VirtualPermissionTests : TestBase
{
    private (string UserLogin, Guid BuId, Guid EmployeeId)[] Datas;


    [TestInitialize]
    public void SetUp()
    {
        this.Datas = new[] { "testEmployeeLogin", "otherTestEmployeeLogin" }
                     .Select(
                         userLogin =>
                         {
                             var buId = this.DataHelper.SaveBusinessUnit().Id;

                             var employeeId = this.DataHelper.SaveEmployee(login: userLogin).Id;

                             this.Evaluate(
                                 DBSessionMode.Write,
                                 context =>
                                 {
                                     var bu = context.Logics.BusinessUnit.GetById(buId, true);

                                     var employee = context.Logics.Employee.GetById(employeeId, true);

                                     context.Logics
                                            .Default
                                            .Create<BusinessUnitEmployeeRole>()
                                            .Save(
                                                new BusinessUnitEmployeeRole(bu)
                                                {
                                                    Employee = employee,
                                                    Role = BusinessUnitEmployeeRoleType.Manager
                                                });
                                 });

                             return (userLogin, buId, employeeId);
                         })
                     .ToArray();


    }

    [TestMethod]
    public void VirtualPermission_EmployeeWithLink_HasAccessByVirtualPermission()
    {
        // Arrange

        // Act
        var accessToBuList =

            this.Evaluate(
                DBSessionMode.Read,
                this.Datas[0].EmployeeId,
                ctx =>
                    ctx.Logics.BusinessUnitFactory.Create(SampleSystemSecurityRole.SeManager)
                       .GetSecureQueryable().Select(bu => bu.Id).ToList());

        // Assert
        accessToBuList.Should().ContainSingle();
        accessToBuList[0].Should().Be(this.Datas[0].BuId);
    }

    [TestMethod]
    public void VirtualPermission_EmployeeWithLink_ResolvedByAccessors()
    {
        // Arrange

        // Act
        var accessorList =

            this.Evaluate(
                DBSessionMode.Read,
                this.Datas[0].EmployeeId,
                ctx =>
                {
                    var bu = ctx.Logics.BusinessUnit.GetById(this.Datas[0].BuId);

                    var accessorData = ctx.SecurityService.GetSecurityProvider<BusinessUnit>(SampleSystemSecurityRole.SeManager)
                                          .GetAccessorData(bu);

                    return ctx.ServiceProvider
                              .GetRequiredService<ISecurityAccessorResolver>()
                              .Resolve(accessorData);
                });

        // Assert
        accessorList.Should().Contain(this.Datas[0].UserLogin);
    }

    [TestMethod]
    public void VirtualPermission_EmployeeWithMyLink_AccessGranted()
    {
        // Arrange

        // Act
        var hasAccess =

            this.Evaluate(
                DBSessionMode.Read,
                this.Datas[1].EmployeeId,
                ctx =>
                {
                    var bu = ctx.Logics.BusinessUnit.GetById(this.Datas[1].BuId);

                    return ctx.SecurityService.GetSecurityProvider<BusinessUnit>(SampleSystemSecurityRole.SeManager)
                              .HasAccess(bu);
                });

        // Assert
        hasAccess.Should().Be(true);
    }

    [TestMethod]
    public void VirtualPermission_EmployeeWithNotMyLink_AccessDenied()
    {
        // Arrange

        // Act
        var hasAccess =

            this.Evaluate(
                DBSessionMode.Read,
                this.Datas[1].EmployeeId,
                ctx =>
                {
                    var bu = ctx.Logics.BusinessUnit.GetById(this.Datas[0].BuId);

                    return ctx.SecurityService.GetSecurityProvider<BusinessUnit>(SampleSystemSecurityRole.SeManager)
                              .HasAccess(bu);
                });

        // Assert
        hasAccess.Should().Be(false);
    }

    [TestMethod]
    public void VirtualPermission_NoNameWithoutLink_AccessDenied()
    {
        // Arrange

        // Act
        var hasAccess =

            this.Evaluate(
                DBSessionMode.Read,
                "Noname",
                ctx =>
                {
                    return ctx.Authorization.SecuritySystem.HasAccess(SampleSystemSecurityRole.SeManager);
                });

        // Assert
        hasAccess.Should().Be(false);
    }
}
