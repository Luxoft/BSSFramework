using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Generated.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests.Auth
{
    [TestClass]
    public class BusinessRoleTests : TestBase
    {
        private const string NewDescription = "test_description";
        private const string RoleName = "SE Manager";

        [TestMethod]
        public void SaveBusinessRole_CheckOperationAddition()
        {
            // Arrange
            var authController = this.GetAuthControllerEvaluator();
            var employeeController = this.MainWebApi.Employee;
            var currentUser = employeeController.Evaluate(c=> c.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin())));

            var operation = authController.Evaluate(c => c.GetSimpleOperationByName(
                SampleSystemSecurityOperationCode.BusinessUnitTypeModuleOpen.ToString()));

            var oprManager = authController.Evaluate(c => c.GetRichBusinessRoleByName(RoleName));
            oprManager.BusinessRoleOperationLinks.Add(new BusinessRoleOperationLinkRichDTO
            {
                BusinessRole = oprManager,
                Operation = operation,
                IsDenormalized = false
            });

            // Act
            this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(oprManager.ToStrict()));

            // Assert
            var roleOperationLink = authController.Evaluate(c => c.GetRichBusinessRoleByName(RoleName))
                                                  .BusinessRoleOperationLinks.First(x => x.Operation.Identity == operation.Identity);

            roleOperationLink.BusinessRole.Identity.Should().Be(oprManager.Identity);
            roleOperationLink.CreatedBy.Should().Be(currentUser.Login.ToString());
            roleOperationLink.ModifiedBy.Should().Be(currentUser.Login.ToString());
            roleOperationLink.Active.Should().BeTrue();
        }

        [TestMethod]
        public void SaveBusinessRole_CheckSubBusinessRoleAddition()
        {
            // Arrange
            var employeeController = this.MainWebApi.Employee;
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var subRole = this.GetAuthControllerEvaluator().GetSimpleBusinessRoleByName("SecretariatNotification");

            var oprManager = this.GetAuthControllerEvaluator().GetRichBusinessRoleByName(RoleName);
            oprManager.SubBusinessRoleLinks.Add(new SubBusinessRoleLinkRichDTO
            {
                BusinessRole = oprManager,
                SubBusinessRole = subRole,
            });

            // Act
            this.GetAuthControllerEvaluator().SaveBusinessRole(oprManager.ToStrict());

            // Assert
            var subRoleLink = this.GetAuthControllerEvaluator().GetRichBusinessRoleByName(RoleName).SubBusinessRoleLinks
                                  .First(x => x.SubBusinessRole.Identity == subRole.Identity);

            subRoleLink.BusinessRole.Identity.Should().Be(oprManager.Identity);
            subRoleLink.CreatedBy.Should().Be(currentUser.Login.ToString());
            subRoleLink.ModifiedBy.Should().Be(currentUser.Login.ToString());
            subRoleLink.Active.Should().BeTrue();
        }

        [TestMethod]
        public void SaveBusinessRole_CheckCreation()
        {
            // Arrange
            var employeeController = this.MainWebApi.Employee;
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var businessRoleStrict = new BusinessRoleStrictDTO
            {
                Name = RoleName
            };

            // Act
            var businessRoleIdentity = this.GetAuthControllerEvaluator().SaveBusinessRole(businessRoleStrict);

            // Assert
            var businessRoleSimple = this.GetAuthControllerEvaluator().GetSimpleBusinessRole(businessRoleIdentity);

            businessRoleSimple.Name.Should().Be(RoleName);
            businessRoleSimple.Active.Should().BeTrue();
            businessRoleSimple.CreatedBy.Should().Be(currentUser.Login.ToString());
            businessRoleSimple.ModifiedBy.Should().Be(currentUser.Login.ToString());
        }

        [TestMethod]
        public void SaveBusinessRole_CheckBusinessRoleChanges()
        {
            // Arrange
            var employeeController = this.MainWebApi.Employee;
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var businessRoleIdentity = this.GetAuthControllerEvaluator().GetSimpleBusinessRoleByName(RoleName).Identity;
            var businessRoleStrict = this.GetAuthControllerEvaluator().GetFullBusinessRole(businessRoleIdentity).ToStrict();
            businessRoleStrict.Description = NewDescription;

            // Act
            this.GetAuthControllerEvaluator().SaveBusinessRole(businessRoleStrict);

            // Assert
            var businessRoleSiple = this.GetAuthControllerEvaluator().GetSimpleBusinessRole(businessRoleIdentity);

            businessRoleSiple.Name.Should().Be(RoleName);
            businessRoleSiple.Description.Should().Be(NewDescription);
            businessRoleSiple.Active.Should().BeTrue();
            businessRoleSiple.ModifiedBy.Should().Be(currentUser.Login.ToString());
        }

        [TestMethod]
        public void SaveBusinessRole_CheckOperationRemoval()
        {
            // Arrange
            var operationIdentity = this.GetAuthControllerEvaluator().GetSimpleOperationByName(
                SampleSystemSecurityOperationCode.EmployeeView.ToString()).Identity;

            var oprManager = this.GetAuthControllerEvaluator().GetRichBusinessRoleByName(RoleName);

            var operationLinkIdentity = oprManager.BusinessRoleOperationLinks
                                                  .Single(x => x.Operation.Identity == operationIdentity).Identity;

            oprManager.BusinessRoleOperationLinks = oprManager.BusinessRoleOperationLinks.Where(
                x => x.Operation.Identity != operationIdentity).ToList();

            // Act
            this.GetAuthControllerEvaluator().SaveBusinessRole(oprManager.ToStrict());

            // Assert
            oprManager = this.GetAuthControllerEvaluator().GetRichBusinessRoleByName(RoleName);
            oprManager.BusinessRoleOperationLinks.Should().NotContain(x => x.Identity == operationLinkIdentity);
        }

        [TestMethod]
        public void RemoveBusinessRole_CheckRemoval()
        {
            // Arrange
            var businessRoleStrict = new BusinessRoleStrictDTO { Name = RoleName };

            var businessRoleIdentity = this.GetAuthControllerEvaluator().SaveBusinessRole(businessRoleStrict);

            // Act
            this.GetAuthControllerEvaluator().RemoveBusinessRole(businessRoleIdentity);
            Action call = () => this.GetAuthControllerEvaluator().GetSimpleBusinessRole(businessRoleIdentity);

            // Assert
            call.Should().Throw<Exception>().WithMessage("BusinessRole with id = * not found");
        }

        [TestMethod]
        public void SaveBusinessRole_CheckSubBusinessRoleRemoval()
        {
            // Arrange
            var subRole = this.GetAuthControllerEvaluator().GetSimpleBusinessRoleByName("SecretariatNotification");

            var oprManager = this.GetAuthControllerEvaluator().GetRichBusinessRoleByName(RoleName);
            oprManager.SubBusinessRoleLinks.Add(new SubBusinessRoleLinkRichDTO
            {
                BusinessRole = oprManager,
                SubBusinessRole = subRole,
            });

            this.GetAuthControllerEvaluator().SaveBusinessRole(oprManager.ToStrict());
            oprManager = this.GetAuthControllerEvaluator().GetRichBusinessRoleByName(RoleName);

            var subRoleLinkIdentity = oprManager.SubBusinessRoleLinks
                .Single(x => x.SubBusinessRole.Identity == subRole.Identity).Identity;

            oprManager.SubBusinessRoleLinks = oprManager.SubBusinessRoleLinks
                .Where(x => x.SubBusinessRole.Identity != subRole.Identity).ToList();

            // Act
            this.GetAuthControllerEvaluator().SaveBusinessRole(oprManager.ToStrict());

            // Assert
            oprManager = this.GetAuthControllerEvaluator().GetRichBusinessRoleByName(RoleName);
            oprManager.SubBusinessRoleLinks.Should().NotContain(x => x.Identity == subRoleLinkIdentity);
        }

        [TestMethod]
        public void RemoveBusinessRoleWithOperations_CheckException()
        {
            // Arrange
            var businessRoleIdentity = this.GetAuthControllerEvaluator().GetSimpleBusinessRoleByName("SecretariatNotification").Identity;

            // Act
            Action call = () => this.GetAuthControllerEvaluator().RemoveBusinessRole(businessRoleIdentity);

            // Assert
            call.Should().Throw<Exception>().WithMessage("Removing business role \"SecretariatNotification\" must be empty");
        }
    }
}
