using Framework.AutomationCore.RootServiceProviderContainer;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class NhibDateTimeTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void CreateObject_CreatedDateOverride()
    {
        // Arrange
        var testDate = new DateTime(2000, 5, 5);
        this.SetCurrentDateTime(testDate);

        var example1Controller = this.GetControllerEvaluator<Example1Controller>();

        // Act
        var objIdentity = example1Controller.Evaluate(c => c.SaveExample1(new Example1StrictDTO()));

        // Assert
        var reloadedObj = example1Controller.Evaluate(c => c.GetSimpleExample1(objIdentity));

        Assert.InRange((reloadedObj.CreateDate! - testDate).Value.Duration(), TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
}
