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

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Inline;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests
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
            var result = this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Read,
                context =>
                {
                    var securityProvider = WorkflowSampleSystemSecurityPath<Employee>.Create(x => x.Location)
                                                                             .Or(_ => false)
                                                                             .ToProvider(WorkflowSampleSystemSecurityOperation.EmployeeView, context.SecurityExpressionBuilderFactory, context.AccessDeniedExceptionService);

                    var employeeBll = context.Logics.EmployeeFactory.Create(securityProvider);

                    return employeeBll.GetById(employeeIdentity.Id).ToIdentityDTO();
                });

            // Assert
            result.Should().BeEquivalentTo(employeeIdentity);
        }
    }
}
