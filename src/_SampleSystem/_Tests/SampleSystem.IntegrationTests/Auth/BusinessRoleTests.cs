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
            var employeeController = this.GetController<EmployeeController>();
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var operation = this.GetAuthorizationController().GetSimpleOperationByName(
                SampleSystemSecurityOperationCode.BusinessUnitTypeModuleOpen.ToString());

            var oprManager = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName);
            oprManager.BusinessRoleOperationLinks.Add(new BusinessRoleOperationLinkRichDTO
            {
                BusinessRole = oprManager,
                Operation = operation,
                IsDenormalized = false
            });

            // Act
            this.GetAuthorizationController().SaveBusinessRole(oprManager.ToStrict());

            // Assert
            var roleOperationLink = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName)
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
            var employeeController = this.GetController<EmployeeController>();
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var subRole = this.GetAuthorizationController().GetSimpleBusinessRoleByName("SecretariatNotification");

            var oprManager = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName);
            oprManager.SubBusinessRoleLinks.Add(new SubBusinessRoleLinkRichDTO
            {
                BusinessRole = oprManager,
                SubBusinessRole = subRole,
            });

            // Act
            this.GetAuthorizationController().SaveBusinessRole(oprManager.ToStrict());

            // Assert
            var subRoleLink = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName).SubBusinessRoleLinks
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
            var employeeController = this.GetController<EmployeeController>();
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var businessRoleStrict = new BusinessRoleStrictDTO
            {
                Name = RoleName
            };

            // Act
            var businessRoleIdentity = this.GetAuthorizationController().SaveBusinessRole(businessRoleStrict);

            // Assert
            var businessRoleSimple = this.GetAuthorizationController().GetSimpleBusinessRole(businessRoleIdentity);

            businessRoleSimple.Name.Should().Be(RoleName);
            businessRoleSimple.Active.Should().BeTrue();
            businessRoleSimple.CreatedBy.Should().Be(currentUser.Login.ToString());
            businessRoleSimple.ModifiedBy.Should().Be(currentUser.Login.ToString());
        }

        [TestMethod]
        public void SaveBusinessRole_CheckBusinessRoleChanges()
        {
            // Arrange
            var employeeController = this.GetController<EmployeeController>();
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var businessRoleIdentity = this.GetAuthorizationController().GetSimpleBusinessRoleByName(RoleName).Identity;
            var businessRoleStrict = this.GetAuthorizationController().GetFullBusinessRole(businessRoleIdentity).ToStrict();
            businessRoleStrict.Description = NewDescription;

            // Act
            this.GetAuthorizationController().SaveBusinessRole(businessRoleStrict);

            // Assert
            var businessRoleSiple = this.GetAuthorizationController().GetSimpleBusinessRole(businessRoleIdentity);

            businessRoleSiple.Name.Should().Be(RoleName);
            businessRoleSiple.Description.Should().Be(NewDescription);
            businessRoleSiple.Active.Should().BeTrue();
            businessRoleSiple.ModifiedBy.Should().Be(currentUser.Login.ToString());
        }

        [TestMethod]
        public void SaveBusinessRole_CheckOperationRemoval()
        {
            // Arrange
            var operationIdentity = this.GetAuthorizationController().GetSimpleOperationByName(
                SampleSystemSecurityOperationCode.EmployeeView.ToString()).Identity;

            var oprManager = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName);

            var operationLinkIdentity = oprManager.BusinessRoleOperationLinks
                                                  .Single(x => x.Operation.Identity == operationIdentity).Identity;

            oprManager.BusinessRoleOperationLinks = oprManager.BusinessRoleOperationLinks.Where(
                x => x.Operation.Identity != operationIdentity).ToList();

            // Act
            this.GetAuthorizationController().SaveBusinessRole(oprManager.ToStrict());

            // Assert
            oprManager = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName);
            oprManager.BusinessRoleOperationLinks.Should().NotContain(x => x.Identity == operationLinkIdentity);
        }

        [TestMethod]
        public void RemoveBusinessRole_CheckRemoval()
        {
            // Arrange
            var businessRoleStrict = new BusinessRoleStrictDTO { Name = RoleName };

            var businessRoleIdentity = this.GetAuthorizationController().SaveBusinessRole(businessRoleStrict);

            // Act
            this.GetAuthorizationController().RemoveBusinessRole(businessRoleIdentity);
            Action call = () => this.GetAuthorizationController().GetSimpleBusinessRole(businessRoleIdentity);

            // Assert
            call.Should().Throw<Exception>().WithMessage("BusinessRole with id = * not found");
        }

        [TestMethod]
        public void SaveBusinessRole_CheckSubBusinessRoleRemoval()
        {
            // Arrange
            var subRole = this.GetAuthorizationController().GetSimpleBusinessRoleByName("SecretariatNotification");

            var oprManager = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName);
            oprManager.SubBusinessRoleLinks.Add(new SubBusinessRoleLinkRichDTO
            {
                BusinessRole = oprManager,
                SubBusinessRole = subRole,
            });

            this.GetAuthorizationController().SaveBusinessRole(oprManager.ToStrict());
            oprManager = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName);

            var subRoleLinkIdentity = oprManager.SubBusinessRoleLinks
                .Single(x => x.SubBusinessRole.Identity == subRole.Identity).Identity;

            oprManager.SubBusinessRoleLinks = oprManager.SubBusinessRoleLinks
                .Where(x => x.SubBusinessRole.Identity != subRole.Identity).ToList();

            // Act
            this.GetAuthorizationController().SaveBusinessRole(oprManager.ToStrict());

            // Assert
            oprManager = this.GetAuthorizationController().GetRichBusinessRoleByName(RoleName);
            oprManager.SubBusinessRoleLinks.Should().NotContain(x => x.Identity == subRoleLinkIdentity);
        }

        [TestMethod]
        public void RemoveBusinessRoleWithOperations_CheckException()
        {
            // Arrange
            var businessRoleIdentity = this.GetAuthorizationController().GetSimpleBusinessRoleByName("SecretariatNotification").Identity;

            // Act
            Action call = () => this.GetAuthorizationController().RemoveBusinessRole(businessRoleIdentity);

            // Assert
            call.Should().Throw<Exception>().WithMessage("Removing business role \"SecretariatNotification\" must be empty");
        }
    }
}
