using FluentAssertions;

using Framework.DomainDriven;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

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
                                       var securityProvider = SecurityPath<Employee>.Create(x => x.Location)
                                                                                    .Or(_ => false)
                                                                                    .ToProvider(
                                                                                        SampleSystemSecurityOperation.EmployeeView,
                                                                                        context.SecurityExpressionBuilderFactory);

                                       var employeeBll = context.Logics.EmployeeFactory.Create(securityProvider);

                                       return employeeBll.GetById(employeeIdentity.Id).ToIdentityDTO();
                                   });

        // Assert
        result.Should().BeEquivalentTo(employeeIdentity);
    }
}
