using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.AutomationCore.TestingProvider;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests._Environment.TestData.Helpers;

public partial class DataManager(IServiceProvider rootServiceProvider, IMsSqlServerSource sqlServerSource) : IRootServiceProviderContainer<ISampleSystemBLLContext>
{
    private Guid GetGuid(Guid? id)
    {
        id = id ?? Guid.NewGuid();
        return (Guid)id;
    }

    public IServiceProvider RootServiceProvider { get; } = rootServiceProvider;
}
