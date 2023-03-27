using System.Linq.Expressions;
using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class PrincipalWithInitTests : TestBase
{
    private const string TestPrincipalName = "Duplicate Permission Tester";

    private static readonly Period TestPeriod = Framework.DomainDriven.DateTimeService.Default.CurrentMonth;

    [TestInitialize]
    public void SetUp()
    {
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

        this.Evaluate(
                      DBSessionMode.Write,
                      context =>
                      {
                          var authContext = context.Authorization;

                          var principalBll = authContext.Logics.Principal;
                          var principal = principalBll.GetByNameOrCreate(TestPrincipalName, true);

                          var entityType = authContext.Logics.EntityType.GetByName(nameof(BusinessUnit));

                          Expression<Func<PermissionFilterEntity, bool>> entitySearchFilter =
                                  entity =>
                                          entity.EntityType == entityType
                                          && entity.EntityId == DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID;

                          var filterEntity = authContext.Logics.PermissionFilterEntity.GetObjectBy(entitySearchFilter) ?? new PermissionFilterEntity
                                                 {
                                                         EntityType = entityType,
                                                         EntityId = DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID
                                                 }.Self(bu => authContext.Logics.PermissionFilterEntity.Save(bu));

                          var permission = new Permission(principal);
                          permission.Role = authContext.Logics.BusinessRole.GetOrCreateAdminRole();
                          permission.Period = TestPeriod;

                          new PermissionFilterItem(permission) { Entity = filterEntity };

                          principalBll.Save(principal);
                      });
    }

    [TestMethod]
    public void CreateDuplicatePermission_ValidationError()
    {
        // Arrange
        var expectedErrorMessage = $"Principal \"{TestPrincipalName}\" has duplicate permissions (Role: {BusinessRole.AdminRoleName} | Period: {TestPeriod} | BusinessUnits: {DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME})";

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
                                            newPermission.Period = TestPeriod;

                                            new PermissionFilterItem(newPermission) { Entity = existsPermission.FilterItems.Single().Entity };

                                            principalBll.Save(principal);
                                        });
                      };

        // Assert
        call.Should().Throw<ValidationException>().WithMessage(expectedErrorMessage);
    }
}
