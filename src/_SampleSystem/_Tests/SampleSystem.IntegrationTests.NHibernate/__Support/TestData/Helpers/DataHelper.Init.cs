using Automation.ServiceEnvironment;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public partial class DataHelper : RootServiceProviderContainer<ISampleSystemBLLContext>
{
    public DataHelper(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
    {
    }

    private Guid GetGuid(Guid? id)
    {
        id = id ?? Guid.NewGuid();
        return (Guid)id;
    }
}
