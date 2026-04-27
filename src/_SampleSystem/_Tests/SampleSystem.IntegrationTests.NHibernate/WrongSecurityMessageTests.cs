using Framework.Application;
using Framework.AutomationCore.Utils;
using Framework.Database;

using Anch.SecuritySystem;

using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

using Anch.SecuritySystem.AccessDenied;

namespace SampleSystem.IntegrationTests;

public class WrongSecurityMessageTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    private static readonly string TestPrincipalName = TextRandomizer.RandomString(10);

    private static readonly Guid TestPrincipalId = Guid.NewGuid();

    protected override async ValueTask InitializeAsync(CancellationToken ct) =>
        this.DataHelper.SaveEmployee(login: TestPrincipalName, id: TestPrincipalId);

    [Fact]
    public void UseWrongSecurityMode_ErrorMessageCorrected() => this.UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SecurityRule.Edit);

    [Fact]
    public void UseWrongSecurityOperation_ErrorMessageCorrected() => this.UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SampleSystemSecurityOperation.EmployeeEdit);

    [Fact]
    public void UseWrongSecurityRole_ErrorMessageCorrected() => this.UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SampleSystemSecurityRole.SeManager);

    private void UseSecurityRule_WithoutSecurity_ErrorMessageCorrected(SecurityRule securityRule)
    {
        //Arrange

        // Act
        var action = () => this.Evaluate(
                         DBSessionMode.Read,
                         TestPrincipalName,
                         ctx => ctx.Logics.EmployeeFactory.Create(securityRule).CheckAccess(ctx.CurrentEmployeeSource.CurrentUser));

        // Assert
        Assert.Equal(
            $"You have no permissions to access object with type = '{nameof(Employee)}' (id = '{TestPrincipalId}', securityRule = '{securityRule}')",
            Assert.Throws<AccessDeniedException>(action).Message);
    }
}
