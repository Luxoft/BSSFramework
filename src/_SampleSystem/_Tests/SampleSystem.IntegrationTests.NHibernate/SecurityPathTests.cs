using Framework.Application;
using Framework.Database;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Services;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

public class SecurityPathTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void SecurityPathWithContext_EmployeeWithoutContextRestrictions_EmployeeShouldHaveAccess()
    {
        // Arrange
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

        // Act
        var result = this.Evaluate(
                                   DBSessionMode.Read,
                                   context =>
                                   {
                                       var securityPath = SecurityPath<Employee>.Create(x => x.Location).Or(_ => false);

                                       var securityProvider = context.ServiceProvider.GetRequiredService<IDomainSecurityProviderFactory<Employee>>().Create(
                                           SampleSystemSecurityOperation.EmployeeView,
                                           securityPath);

                                       var employeeBll = context.Logics.EmployeeFactory.Create(securityProvider);

                                       return employeeBll.GetById(employeeIdentity.Id).ToIdentityDTO();
                                   });

        // Assert
        Assert.Equal(employeeIdentity, result);
    }
}
