﻿using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

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

        result.Should().BeEquivalentTo([testObjects[0], testObjects[2]]);
    }
}
