using System;
using Automation.ServiceEnvironment;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
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

        public IDatabaseContext DatabaseContext => this.RootServiceProvider.GetRequiredService<IDatabaseContext>();

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
