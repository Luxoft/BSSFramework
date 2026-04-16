using Framework.AutomationCore.RootServiceProviderContainer;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class NhibDateTimeTests : TestBase
{
    private DateTime prevDateTime;

    public NhibDateTimeTests() => this.prevDateTime = this.TimeProvider.GetLocalNow().DateTime;

    protected override void BeforeCleanup() => this.SetCurrentDateTime(this.prevDateTime);

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

        reloadedObj.CreateDate.Should().BeCloseTo(testDate, TimeSpan.FromSeconds(1));
    }
}
