using Automation.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public class AuthHelper : AuthHelperBase
{
    public AuthHelper(IServiceProvider rootServiceProvider)
        : base(rootServiceProvider)
    {
    }
}
