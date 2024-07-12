using FluentAssertions;

using FluentValidation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using SampleSystem.Generated.DTO;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class TestRestrictionRoleTests : TestBase
{
    [TestMethod]
    public void GetRestrictionObjectsWithRestrictionRole_RestrictionApplied()
    {
        // Arrange
        var testRestrictionPrincipal = Guid.NewGuid().ToString();

        this.AuthHelper.SetUserRole(testRestrictionPrincipal, SampleSystemSecurityRole.RestrictionRole);

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
        var result = this.GetControllerEvaluator<TestRestrictionObjectController>(testRestrictionPrincipal)
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
        var action = () => this.AuthHelper.SetCurrentUserRole(SampleSystemSecurityRole.RestrictionRole);

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
                         this.AuthHelper.SetCurrentUserRole(
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
                         this.AuthHelper.SetCurrentUserRole(
                             new SampleSystemTestPermission(
                                 SampleSystemSecurityRole.RestrictionRole,
                                 location: location));

        // Assert
        action.Should().Throw<ValidationException>()
              .And.Message.Should().Contain($"Invalid SecurityContextType: {nameof(Location)}.");
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
                var bll = ctx.Logics.Default.Create<TestRestrictionObject>(SampleSystemSecurityRule.TestRestriction);

                return bll.GetFullList().ToIdentityDTOList();
            });

        // Assert
        result.Should().BeEquivalentTo([testObjects[0], testObjects[2]]);
    }
}
