using Framework.AutomationCore.RootServiceProviderContainer;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public partial class DataHelper(IServiceProvider rootServiceProvider) : IRootServiceProviderContainer<ISampleSystemBLLContext>
{
    private Guid GetGuid(Guid? id)
    {
        id = id ?? Guid.NewGuid();
        return (Guid)id;
    }

    public IServiceProvider RootServiceProvider { get; } = rootServiceProvider;
}
