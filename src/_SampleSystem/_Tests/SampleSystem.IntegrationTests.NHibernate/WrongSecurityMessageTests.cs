using Automation.Utils;

using Framework.DomainDriven;
using Framework.SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class WrongSecurityMessageTests : TestBase
{
    private static readonly string TestPrincipalName = TextRandomizer.RandomString(10);

    private static readonly Guid TestPrincipalId = Guid.NewGuid();

    [TestInitialize]
    public void SetUp()
    {
        this.DataHelper.SaveEmployee(login: TestPrincipalName, id: TestPrincipalId);
    }

    [TestMethod]
    public void UseWrongSecurityMode_ErrorMessageCorrected()
    {
        this.UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SecurityRule.Edit);
    }

    [TestMethod]
    public void UseWrongSecurityOperation_ErrorMessageCorrected()
    {
        this.UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SampleSystemSecurityOperation.EmployeeEdit);
    }

    [TestMethod]
    public void UseWrongSecurityRole_ErrorMessageCorrected()
    {
        this.UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SampleSystemSecurityRole.SeManager);
    }

    private void UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SecurityRule securityRule)
    {
        //Arrange

        // Act
        var action = () => this.Evaluate(
                         DBSessionMode.Read,
                         TestPrincipalName,
                         ctx => ctx.Logics.EmployeeFactory.Create(securityRule).CheckAccess(ctx.Logics.Employee.GetCurrent()));

        // Assert
        action.Should()
              .Throw<AccessDeniedException>()
              .WithMessage($"You have no permissions to access object with type = '{nameof(Employee)}' (id = '{TestPrincipalId}', securityRule = '{securityRule}')");
    }
}
