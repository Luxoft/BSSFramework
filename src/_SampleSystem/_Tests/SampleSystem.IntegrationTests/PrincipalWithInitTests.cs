using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;
using Framework.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class PrincipalWithInitTests : TestBase
{
    private const string TestPrincipalName = "Duplicate Permission Tester";

    private Period testPeriod;

    [TestInitialize]
    public void SetUp()
    {
        this.testPeriod = this.TimeProvider.GetCurrentMonth();

        var buTypeId = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID);

        var luxoftBuId = this.DataHelper.SaveBusinessUnit(
                                                          id: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_ID,
                                                          name: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME,
                                                          type: buTypeId);

        var costBuId = this.DataHelper.SaveBusinessUnit(
                                                        id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID,
                                                        name: DefaultConstants.BUSINESS_UNIT_PARENT_CC_NAME,
                                                        type: buTypeId,
                                                        parent: luxoftBuId);

        var profitBuId = this.DataHelper.SaveBusinessUnit(
                                                          id: DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID,
                                                          name: DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME,
                                                          type: buTypeId,
                                                          parent: luxoftBuId);

        this.AuthHelper.SetUserRole(
            TestPrincipalName,
            new SampleSystemTestPermission(
                SecurityRole.Administrator,
                new BusinessUnitIdentityDTO(DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID)) { Period = this.testPeriod });
    }

    [TestMethod]
    public void CreateDuplicatePermission_ValidationError()
    {
        // Arrange
        var expectedErrorMessage = $"Principal \"{TestPrincipalName}\" has duplicate permissions (Role: {SecurityRole.Administrator} | Period: {this.testPeriod} | BusinessUnits: {DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME})";

        // Act
        Action call = () =>
                      {
                          this.Evaluate(
                                        DBSessionMode.Write,
                                        context =>
                                        {
                                            var authContext = context.Authorization;

                                            var principalBll = authContext.Logics.Principal;
                                            var principal = principalBll.GetByName(TestPrincipalName, true);

                                            var existsPermission = principal.Permissions.Single();

                                            var newPermission = new Permission(principal);
                                            newPermission.Role = existsPermission.Role;
                                            newPermission.Period = this.testPeriod;

                                            var restriction = existsPermission.Restrictions.Single();

                                            new PermissionRestriction(newPermission)
                                            {
                                                SecurityContextId = restriction.SecurityContextId,
                                                SecurityContextType = restriction.SecurityContextType
                                            };

                                            principalBll.Save(principal);
                                        });
                      };

        // Assert
        call.Should().Throw<ValidationException>().WithMessage(expectedErrorMessage);
    }
}
