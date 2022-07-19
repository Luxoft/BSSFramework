using System;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper : IRootServiceProviderContainer
    {
        public DataHelper(IServiceProvider rootServiceProvider)
        {
            this.RootServiceProvider = rootServiceProvider;
        }

        public IServiceProvider RootServiceProvider { get; }


        public AuthHelper AuthHelper { private get; set; }

        public SampleSystemServerPrimitiveDTOMappingService GetMappingService(ISampleSystemBLLContext context)
        {
            return new SampleSystemServerPrimitiveDTOMappingService(context);
        }

        private Guid GetGuid(Guid? id)
        {
            id = id ?? Guid.NewGuid();
            return (Guid)id;
        }
    }
}
