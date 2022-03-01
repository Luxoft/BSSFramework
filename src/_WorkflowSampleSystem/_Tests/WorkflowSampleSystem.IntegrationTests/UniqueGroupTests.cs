using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Framework.DomainDriven;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class UniqueGroupTests : TestBase
    {
        [TestMethod]
        public void UniqueGroup_NonUniqueEntityCreated_ErrorUsesCustomName()
        {
            // Arrange
            var role = this.DataHelper.SaveEmployeeRole();
            var roleDegree = this.DataHelper.SaveEmployeeRoleDegree();

            this.DataHelper.SaveRoleRoleDegreeLink(role, roleDegree);

            // Act
            var action = new Action(() => this.DataHelper.SaveRoleRoleDegreeLink(role, roleDegree));

            // Assert
            action.Should().Throw<UniqueViolationConstraintDALException>().WithMessage("Role-Seniority link with same:'Role,Seniority' already exists");
        }
    }
}
