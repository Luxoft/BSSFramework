using Bss.Testing.Xunit.Sdk;

using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.Xunit.NHibernate.__Support;

namespace SampleSystem.IntegrationTests.Xunit.NHibernate;

public class ServiceProviderMemberDataTest(IServiceProvider serviceProvider) : TestBase(serviceProvider)
{
    [BssTheory]
    [ServiceProviderMemberData(nameof(GetMemberData))]
    public void GetDataFromServiceProvider(FullSecurityRole role) => role.Name.Should().NotBeNull();

    protected IEnumerable<object> GetMemberData() =>
        this.ServiceProvider.GetRequiredService<ISecurityRoleSource>().SecurityRoles.Select(x => new [] { x });
}
