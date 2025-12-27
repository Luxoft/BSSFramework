using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.Main;
using SecuritySystem;
using SecuritySystem.Validation;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class RestrictionRoleTests : TestBase
{
    [TestMethod]
    public void GetRestrictionObjectsWithRestrictionRole_RestrictionApplied()
    {
        // Arrange
        var userId = this.AuthManager.For(Guid.NewGuid().ToString()).SetRole(SampleSystemSecurityRole.RestrictionRole);

        var testObjects = this.Evaluate(
            DBSessionMode.Write,
            ctx =>
            {
                var objList = new TestRestrictionObject[]
                              {
                                  new() { RestrictionHandler = true },
                                  new() { RestrictionHandler = false },
                                  new() { RestrictionHandler = true }
                              };

                ctx.Logics.Default.Create<TestRestrictionObject>().Save(objList);

                return objList.ToIdentityDTOList();
            });

        // Act
        var result = this.GetControllerEvaluator<TestRestrictionObjectController>(userId)
                         .Evaluate(c => c.GetSimpleTestRestrictionObjects())
                         .Select(v => v.Identity);

        // Assert
        result.Should().BeEquivalentTo([testObjects[0], testObjects[2]]);
    }

    [TestMethod]
    public void TryCreateEmptyPermission_PermissionCreated()
    {
        // Arrange

        // Act
        var action = () => this.AuthManager.For().SetRole(SampleSystemSecurityRole.RestrictionRole);

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryCreatePermissionWithCorrectSecurityContext_PermissionCreated()
    {
        // Arrange
        var businessUnit = this.DataHelper.SaveBusinessUnit();

        // Act
        var action = () =>
                         this.AuthManager.For().SetRole(
                             new SampleSystemTestPermission(
                                 SampleSystemSecurityRole.RestrictionRole,
                                 businessUnit: businessUnit));

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryCreatePermissionWithInvalidSecurityContext_ExceptionRaised()
    {
        // Arrange
        var location = this.DataHelper.SaveLocation();

        // Act
        var action = () =>
                         this.AuthManager.For().SetRole(
                             new SampleSystemTestPermission(
                                 SampleSystemSecurityRole.RestrictionRole,
                                 location: location));

        // Assert
        action.Should().Throw<SecuritySystemValidationException>()
              .And.Message.Should().Contain($"Invalid SecurityContextType: {nameof(Location)}");
    }

    [TestMethod]
    public void GetRestrictionFromHeaderObjectsWithConditionRule_RestrictionApplied()
    {
        // Arrange
        var testObjects = this.Evaluate(
            DBSessionMode.Write,
            ctx =>
            {
                var objList = new TestRestrictionObject[]
                              {
                                  new() { RestrictionHandler = true },
                                  new() { RestrictionHandler = false },
                                  new() { RestrictionHandler = true }
                              };

                ctx.Logics.Default.Create<TestRestrictionObject>().Save(objList);

                return objList.ToIdentityDTOList();
            });

        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            ctx =>
            {
                var bll = ctx.Logics.Default.Create<TestRestrictionObject>(SampleSystemSecurityRule.TestRestriction);

                return bll.GetFullList().ToIdentityDTOList();
            });

        // Assert
        result.Should().BeEquivalentTo([testObjects[0], testObjects[2]]);
    }

    [TestMethod]
    public void GetRestrictionObjectsWithConditionRule_RestrictionApplied()
    {
        // Arrange
        var testObjects = this.Evaluate(
            DBSessionMode.Write,
            ctx =>
            {
                var objList = new TestRestrictionObject[]
                              {
                                  new() { RestrictionHandler = true },
                                  new() { RestrictionHandler = false },
                                  new() { RestrictionHandler = true }
                              };

                ctx.Logics.Default.Create<TestRestrictionObject>().Save(objList);

                return objList.ToIdentityDTOList();
            });

        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            ctx =>
            {
                var bll = ctx.Logics.Default.Create<TestRestrictionObject>(SecurityRule.Disabled.And((TestRestrictionObject v) => v.RestrictionHandler));

                return bll.GetFullList().ToIdentityDTOList();
            });

        // Assert
        result.Should().BeEquivalentTo([testObjects[0], testObjects[2]]);
    }

    [TestMethod]
    public void TryCreatePermissionWithoutRequiredSecurityContext_ExceptionRaised()
    {
        // Arrange
        var location = this.DataHelper.SaveLocation();

        // Act
        var action = () => this.AuthManager.For().SetRole(
                         new SampleSystemTestPermission(SampleSystemSecurityRole.RequiredRestrictionRole, location: location));

        // Assert
        action.Should().Throw<SecuritySystemValidationException>()
              .And.Message.Should().Contain(
                  $"{nameof(Framework.Authorization.Domain.Permission)} must contain the required contexts: {nameof(BusinessUnit)}");
    }
}
