using Framework.Application;
using Framework.BLL;
using Framework.Database;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DTO;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.Main;
using Anch.SecuritySystem;
using Anch.SecuritySystem.Validation;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class RestrictionRoleTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
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
        Assert.Equal(new[] { testObjects[0], testObjects[2] }, result);
    }

    [Fact]
    public void TryCreateEmptyPermission_PermissionCreated()
    {
        // Arrange

        // Act
        var action = () => this.AuthManager.For().SetRole(SampleSystemSecurityRole.RestrictionRole);

        // Assert
        action();
    }

    [Fact]
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
        action();
    }

    [Fact]
    public void TryCreatePermissionWithInvalidSecurityContext_ExceptionRaised()
    {
        // Arrange
        var location = this.DataHelper.SaveLocation();

        // Act
        var ex = Record.Exception(() =>
            this.AuthManager.For().SetRole(
                new SampleSystemTestPermission(
                    SampleSystemSecurityRole.RestrictionRole,
                    location: location)));

        // Assert
        var validationException = Assert.IsType<SecuritySystemValidationException>(ex);
        Assert.Contains($"Invalid SecurityContextType: {nameof(Location)}", validationException.Message);
    }

    [Fact]
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
        Assert.Equal(new[] { testObjects[0], testObjects[2] }, result);
    }

    [Fact]
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
        Assert.Equal(new[] { testObjects[0], testObjects[2] }, result);
    }

    [Fact]
    public void TryCreatePermissionWithoutRequiredSecurityContext_ExceptionRaised()
    {
        // Arrange
        var location = this.DataHelper.SaveLocation();

        // Act
        var ex = Record.Exception(() => this.AuthManager.For().SetRole(
            new SampleSystemTestPermission(SampleSystemSecurityRole.RequiredRestrictionRole, location: location)));

        // Assert
        var validationException = Assert.IsType<SecuritySystemValidationException>(ex);
        Assert.Contains(
            $"{nameof(Framework.Authorization.Domain.Permission)} must contain the required contexts: {nameof(BusinessUnit)}",
            validationException.Message);
    }
}
