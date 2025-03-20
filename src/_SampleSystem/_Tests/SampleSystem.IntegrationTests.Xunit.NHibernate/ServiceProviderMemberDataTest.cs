using Automation.Xunit.Sdk;

using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.Xunit.__Support;

namespace SampleSystem.IntegrationTests.Xunit;

public class ServiceProviderMemberDataTest(IServiceProvider serviceProvider) : TestBase(serviceProvider)
{
    [AutomationCoreTheory]
    [ServiceProviderMemberData(nameof(GetMemberData))]
    public void GetDataFromServiceProvider(FullSecurityRole role)
    {

        Assert.NotEmpty(role.Name);
    }

    protected IEnumerable<object> GetMemberData() =>
        serviceProvider.GetRequiredService<ISecurityRoleSource>().SecurityRoles.Select(x => new [] { x });
}
