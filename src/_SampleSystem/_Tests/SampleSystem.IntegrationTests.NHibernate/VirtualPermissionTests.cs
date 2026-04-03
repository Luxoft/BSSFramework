using Framework.Application;
using Framework.Database;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class VirtualPermissionTests : TestBase
{
    private (string UserLogin, Guid BuId, Guid EmployeeId)[] Datas;


    [TestInitialize]
    public void SetUp() =>
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
    public  async Task VirtualPermission_EmployeeWithLink_ResolvedByAccessors()
    {
        // Arrange

        // Act
        var accessorList =
            await this.EvaluateAsync(
                DBSessionMode.Read,
                this.Datas[0].EmployeeId,
                async ctx =>
                {
                    var bu = ctx.Logics.BusinessUnit.GetById(this.Datas[0].BuId, true)!;

                    var accessorData = await ctx.SecurityService.GetSecurityProvider<BusinessUnit>(SampleSystemSecurityRole.SeManager)
                                                .GetAccessorDataAsync(bu);

                    return ctx.SecurityAccessorResolver.Resolve(accessorData);
                });

        // Assert
        accessorList.Should().Contain(this.Datas[0].UserLogin);
    }

    [TestMethod]
    public async Task VirtualPermission_EmployeeWithMyLink_AccessGranted()
    {
        // Arrange

        // Act
        var hasAccess =

            await this.EvaluateAsync(
                DBSessionMode.Read,
                this.Datas[1].EmployeeId,
                async ctx =>
                {
                    var bu = ctx.Logics.BusinessUnit.GetById(this.Datas[1].BuId)!;

                    return await ctx.SecurityService.GetSecurityProvider<BusinessUnit>(SampleSystemSecurityRole.SeManager)
                              .HasAccessAsync(bu);
                });

        // Assert
        hasAccess.Should().BeTrue();
    }

    [TestMethod]
    public async Task VirtualPermission_EmployeeWithNotMyLink_AccessDenied()
    {
        // Arrange

        // Act
        var hasAccess =

            await this.EvaluateAsync(
                DBSessionMode.Read,
                this.Datas[1].EmployeeId,
                async ctx =>
                {
                    var bu = ctx.Logics.BusinessUnit.GetById(this.Datas[0].BuId)!;

                    return await ctx.SecurityService.GetSecurityProvider<BusinessUnit>(SampleSystemSecurityRole.SeManager)
                              .HasAccessAsync(bu);
                });

        // Assert
        hasAccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task VirtualPermission_NoNameWithoutLink_AccessDenied()
    {
        // Arrange

        // Act
        var hasAccess =

            await this.EvaluateAsync(
                DBSessionMode.Read,
                "Noname",
                async ctx =>
                {
                    return await ctx.Authorization.SecuritySystem.HasAccessAsync(SampleSystemSecurityRole.SeManager);
                });

        // Assert
        hasAccess.Should().BeFalse();
    }
}
