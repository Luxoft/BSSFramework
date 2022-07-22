using System;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.OData;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class SecurityPathTests : TestBase
    {
        [TestMethod]
        public void SecurityPathWithContext_EmployeeWithoutContextRestrictions_EmployeeShouldHaveAccess()
        {
            // Arrange
            var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

            // Act
            var result = this.GetContextEvaluator().Evaluate(
                DBSessionMode.Read,
                context =>
                {
                    var securityProvider = SampleSystemSecurityPath<Employee>.Create(x => x.Location)
                                                                             .Or(_ => false)
                                                                             .ToProvider(SampleSystemSecurityOperation.EmployeeView, context.SecurityExpressionBuilderFactory, context.AccessDeniedExceptionService);

                    var employeeBll = context.Logics.EmployeeFactory.Create(securityProvider);

                    return employeeBll.GetById(employeeIdentity.Id).ToIdentityDTO();
                });

            // Assert
            result.Should().BeEquivalentTo(employeeIdentity);
        }
    }
}
