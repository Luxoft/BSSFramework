using System;

using Automation.ServiceEnvironment;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public partial class DataHelper : RootServiceProviderContainer<ISampleSystemBLLContext>
{
    public DataHelper(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
    {
    }

    public AuthHelper AuthHelper => this.RootServiceProvider.GetRequiredService<AuthHelper>();

    private Guid GetGuid(Guid? id)
    {
        id = id ?? Guid.NewGuid();
        return (Guid)id;
    }
}
