using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain.IntergrationVersions;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Integration;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class IntegrationVersionTest : TestBase
{
    [TestMethod]
    public void SaveWithMoreVersion_IgnoreLessPolicy_ValueSaved()
    {
        //Arrange
        var integrationVersion = 10;

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer1()
                                                  {
                                                          Name = Guid.NewGuid().ToString(),
                                                          IntegrationVersion = integrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer1>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer1Controller = this.GetControllerEvaluator<IntegrationVersionContainer1Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        // Act
        var expectedName = Guid.NewGuid().ToString();
        var expectedIntegrationVersion = integrationVersion + 10;

        integrationVersionContainer1Controller.Evaluate(c => c.SaveIntegrationVersionContainer1(
                                                         new IntegrationVersionContainer1IntegrationRichDTO()
                                                         {
                                                                 Id = id,
                                                                 IntegrationVersion = expectedIntegrationVersion,
                                                                 Name = expectedName
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer1>().GetById(id));
        actual.Name.Should().Be(expectedName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion);
    }

    [TestMethod]
    public void SaveWithLessVersion_IgnoreLessPolicy_ValueIgnore()
    {
        //Arrange
        var expectedIntegrationVersion = 10;
        var expectedName = Guid.NewGuid().ToString();

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer1()
                                                  {
                                                          Name = expectedName,
                                                          IntegrationVersion = expectedIntegrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer1>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer1Controller = this.GetControllerEvaluator<IntegrationVersionContainer1Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        // Act

        integrationVersionContainer1Controller.Evaluate(c => c.SaveIntegrationVersionContainer1(
                                                         new IntegrationVersionContainer1IntegrationRichDTO()
                                                         {
                                                                 Id = id,
                                                                 IntegrationVersion = expectedIntegrationVersion - 10,
                                                                 Name = Guid.NewGuid().ToString()
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer1>().GetById(id));
        actual.Name.Should().Be(expectedName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion);
    }

    [TestMethod]
    public void SaveWithEqualVersion_IgnoreLessPolicy_ValueIgnored()
    {
        //Arrange
        var expectedIntegrationVersion = 10;
        var expectedName = Guid.NewGuid().ToString();

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer1()
                                                  {
                                                          Name = expectedName,
                                                          IntegrationVersion = expectedIntegrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer1>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer1Controller = this.GetControllerEvaluator<IntegrationVersionContainer1Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        // Act

        integrationVersionContainer1Controller.Evaluate(c => c.SaveIntegrationVersionContainer1(
                                                         new IntegrationVersionContainer1IntegrationRichDTO()
                                                         {
                                                                 Id = id,
                                                                 IntegrationVersion = expectedIntegrationVersion,
                                                                 Name = Guid.NewGuid().ToString()
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer1>().GetById(id));
        actual.Name.Should().Be(expectedName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion);
    }

    [TestMethod]
    public void SaveWithMoreVersion_IgnoreLessOrEqualPolicy_ValueSaved()
    {
        //Arrange
        var integrationVersion = 10;

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer2()
                                                  {
                                                          Name = Guid.NewGuid().ToString(),
                                                          IntegrationVersion = integrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer2>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer2Controller = this.GetControllerEvaluator<IntegrationVersionContainer2Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        // Act
        var expectedName = Guid.NewGuid().ToString();
        var expectedIntegrationVersion = integrationVersion + 10;

        integrationVersionContainer2Controller.Evaluate(c => c.SaveIntegrationVersionContainer2(
                                                         new IntegrationVersionContainer2IntegrationRichDTO()
                                                         {
                                                                 Id = id,
                                                                 IntegrationVersion = expectedIntegrationVersion,
                                                                 Name = expectedName
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer2>().GetById(id));
        actual.Name.Should().Be(expectedName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion);
    }

    [TestMethod]
    public void SaveWithLessVersion_IgnoreLessOrEqualPolicy_ValueIgnore()
    {
        //Arrange
        var expectedIntegrationVersion = 10;
        var expectedName = Guid.NewGuid().ToString();

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer2()
                                                  {
                                                          Name = expectedName,
                                                          IntegrationVersion = expectedIntegrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer2>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer2Controller = this.GetControllerEvaluator<IntegrationVersionContainer2Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        // Act

        integrationVersionContainer2Controller.Evaluate(c => c.SaveIntegrationVersionContainer2(
                                                         new IntegrationVersionContainer2IntegrationRichDTO()
                                                         {
                                                                 Id = id,
                                                                 IntegrationVersion = expectedIntegrationVersion - 10,
                                                                 Name = Guid.NewGuid().ToString()
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer2>().GetById(id));
        actual.Name.Should().Be(expectedName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion);
    }

    [TestMethod]
    public void SaveWithEqualVersion_IgnoreLessOrEqualPolicy_ValueIgnored()
    {
        //Arrange
        var expectedIntegrationVersion = 10;
        var expectedName = Guid.NewGuid().ToString();

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer2()
                                                  {
                                                          Name = expectedName,
                                                          IntegrationVersion = expectedIntegrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer2>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer2Controller = this.GetControllerEvaluator<IntegrationVersionContainer2Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        // Act

        var nextName = Guid.NewGuid().ToString();

        integrationVersionContainer2Controller.Evaluate(c => c.SaveIntegrationVersionContainer2(
                                                         new IntegrationVersionContainer2IntegrationRichDTO()
                                                         {
                                                                 Id = id,
                                                                 IntegrationVersion = expectedIntegrationVersion,
                                                                 Name = nextName
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer2>().GetById(id));
        actual.Name.Should().Be(nextName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion);
    }

    [TestMethod]
    public void SaveModelWithLessVersion_IgnoreLessOrEqualPolicy_ValueIgnore()
    {
        //Arrange
        var expectedIntegrationVersion = 10;
        var expectedName = Guid.NewGuid().ToString();

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer1()
                                                  {
                                                          Name = expectedName,
                                                          IntegrationVersion = expectedIntegrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer1>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer1Controller = this.GetControllerEvaluator<IntegrationVersionContainer1Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        var modelName = Guid.NewGuid().ToString();

        integrationVersionContainer1Controller.Evaluate(c => c.SaveIntegrationVersionContainer1ByCustom(
                                                         new IntegrationVersionContainer1CustomIntegrationSaveModelIntegrationRichDTO()
                                                         {
                                                                 SavingObject = new IntegrationVersionContainer1IntegrationSimpleDTO() { Id = id, IntegrationVersion = expectedIntegrationVersion - 1 },
                                                                 CustomName = modelName
                                                         }));



        // Act

        integrationVersionContainer1Controller.Evaluate(c => c.SaveIntegrationVersionContainer1ByCustom(
                                                         new IntegrationVersionContainer1CustomIntegrationSaveModelIntegrationRichDTO()
                                                         {
                                                                 SavingObject = new IntegrationVersionContainer1IntegrationSimpleDTO() { Id = id, IntegrationVersion = expectedIntegrationVersion - 1 },
                                                                 CustomName = Guid.NewGuid().ToString()
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer1>().GetById(id));
        actual.Name.Should().Be(expectedName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion);
    }

    [TestMethod]
    public void SaveModelWithEqualVersion_IgnoreLessOrEqualPolicy_ValueIgnored()
    {
        //Arrange
        var expectedIntegrationVersion = 10;
        var expectedName = Guid.NewGuid().ToString();

        var id = this.EvaluateWrite(context =>
                                    {
                                        var obj = new IntegrationVersionContainer1()
                                                  {
                                                          Name = expectedName,
                                                          IntegrationVersion = expectedIntegrationVersion
                                                  };

                                        context.Logics.Default.Create<IntegrationVersionContainer1>().Save(obj);

                                        return obj.Id;
                                    });

        var integrationVersionContainer1Controller = this.GetControllerEvaluator<IntegrationVersionContainer1Controller>();
        this.AuthHelper.SetCurrentUserRole(SecurityRole.SystemIntegration);

        var modelName = Guid.NewGuid().ToString();

        integrationVersionContainer1Controller.Evaluate(c => c.SaveIntegrationVersionContainer1ByCustom(
                                                         new IntegrationVersionContainer1CustomIntegrationSaveModelIntegrationRichDTO()
                                                         {
                                                                 SavingObject = new IntegrationVersionContainer1IntegrationSimpleDTO() { Id = id, IntegrationVersion = expectedIntegrationVersion + 1 },
                                                                 CustomName = modelName
                                                         }));



        // Act

        integrationVersionContainer1Controller.Evaluate(c => c.SaveIntegrationVersionContainer1ByCustom(
                                                         new IntegrationVersionContainer1CustomIntegrationSaveModelIntegrationRichDTO()
                                                         {
                                                                 SavingObject = new IntegrationVersionContainer1IntegrationSimpleDTO() { Id = id, IntegrationVersion = expectedIntegrationVersion + 1 },
                                                                 CustomName = Guid.NewGuid().ToString()
                                                         }));

        // Assert

        var actual = this.EvaluateRead(context => context.Logics.Default.Create<IntegrationVersionContainer1>().GetById(id));
        actual.Name.Should().Be(modelName);
        actual.IntegrationVersion.Should().Be(expectedIntegrationVersion + 1);
    }
}
