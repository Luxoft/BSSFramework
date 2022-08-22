using System;
using Automation;
using Automation.ServiceEnvironment;
using Automation.Utils.DatabaseUtils.Interfaces;
using Framework.DomainDriven;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper : IntegrationTestContextEvaluator<ISampleSystemBLLContext>
    {
        public DataHelper(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
        {
        }

        public AuthHelper AuthHelper => this.RootServiceProvider.GetRequiredService<AuthHelper>();

        public IDateTimeService DateTimeService => this.RootServiceProvider.GetRequiredService<IDateTimeService>();

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
