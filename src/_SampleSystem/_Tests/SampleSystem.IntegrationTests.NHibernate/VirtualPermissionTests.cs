using Framework.Application;
using Framework.Database;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Enums;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

public class VirtualPermissionTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    private (string UserLogin, Guid BuId, Guid EmployeeId)[] Datas;


    protected override async ValueTask InitializeAsync(CancellationToken ct) =>
        this.Datas = new[] { "testEmployeeLogin", "otherTestEmployeeLogin" }
                     .Select(
                         userLogin =>
                         {
                             var buId = this.DataManager.SaveBusinessUnit().Id;

                             var employeeId = this.DataManager.SaveEmployee(login: userLogin).Id;

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

    [Fact]
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
        Assert.Equal(this.Datas[0].BuId, Assert.Single(accessToBuList));
    }

    [Fact]
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
        Assert.Contains(this.Datas[0].UserLogin, accessorList);
    }

    [Fact]
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
        Assert.True(hasAccess);
    }

    [Fact]
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
        Assert.False(hasAccess);
    }

    [Fact]
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
        Assert.False(hasAccess);
    }
}
