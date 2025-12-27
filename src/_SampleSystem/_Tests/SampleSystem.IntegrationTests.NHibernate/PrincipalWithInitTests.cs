using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

using SecuritySystem;
using SecuritySystem.Validation;

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

        this.AuthManager.For(TestPrincipalName).SetRole(
            new SampleSystemTestPermission(
            SampleSystemSecurityRole.TestPerformance,
            new BusinessUnitIdentityDTO(DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID)) { Period = new PermissionPeriod(this.testPeriod.StartDate, this.testPeriod.EndDate) });
    }

    [TestMethod]
    public void CreateDuplicatePermission_ValidationError()
    {
        // Arrange
        var expectedErrorMessage = $"Principal \"{TestPrincipalName}\" has duplicate permissions";

        // Act
        var call = () =>
                   {
                       this.Evaluate(
                           DBSessionMode.Write,
                           context =>
                           {
                               var authContext = context.Authorization;

                               var principalBll = authContext.Logics.Principal;
                               var principal = principalBll.GetByName(TestPrincipalName, true);

                               var existsPermission = principal.Permissions.Single();

                               var newPermission = new Permission(principal)
                               {
                                   Role = existsPermission.Role,
                                   Period = this.testPeriod
                               };

                               foreach (var restriction in existsPermission.Restrictions)
                               {
                                   new PermissionRestriction(newPermission)
                                   {
                                       SecurityContextId = restriction.SecurityContextId,
                                       SecurityContextType = restriction.SecurityContextType
                                   };
                               }

                               try
                               {
                                   principalBll.Save(principal);
                               }
                               catch (Exception e)
                               {
                                   var m = e.Message;
                                   Console.WriteLine(e);
                                   throw;
                               }

                           });
                   };

        // Assert
        call.Should().Throw<SecuritySystemValidationException>().And.Message.Should().Contain(expectedErrorMessage);
    }
}
