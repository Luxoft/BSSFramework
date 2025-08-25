using Framework.DomainDriven;
using SecuritySystem;
using SecuritySystem.Services;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class SecurityPathTests : TestBase
{
    [TestMethod]
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
        result.Should().BeEquivalentTo(employeeIdentity);
    }
}
