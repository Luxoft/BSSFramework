using System;

using Automation.ServiceEnvironment;

using Framework.DomainDriven;

using Microsoft.Extensions.DependencyInjection;
using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper : RootServiceProviderContainer<ISampleSystemBLLContext>
    {
        public DataHelper(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
        {
        }

        public AuthHelper AuthHelper => this.RootServiceProvider.GetRequiredService<AuthHelper>();

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
