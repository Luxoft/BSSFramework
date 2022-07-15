using System;

using Framework.Core;
using Framework.DomainDriven.BLL;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper : IRootServiceProviderContainer
    {
        public SampleSystemTestServiceEnvironment Environment { get; set; }


        public AuthHelper AuthHelper { private get; set; }

        public TResult EvaluateWrite<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, func);
        }

        public void EvaluateRead(Action<ISampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, action);
        }

        public TResult EvaluateRead<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, func);
        }

        public void EvaluateWrite(Action<ISampleSystemBLLContext> func)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context => { func(context); return Ignore.Value; });
        }

        public SampleSystemServerPrimitiveDTOMappingService GetMappingService(ISampleSystemBLLContext context)
        {
            return new SampleSystemServerPrimitiveDTOMappingService(context);
        }

        private Guid GetGuid(Guid? id)
        {
            id = id ?? Guid.NewGuid();
            return (Guid)id;
        }
        IServiceProvider IRootServiceProviderContainer.RootServiceProvider => this.Environment.RootServiceProvider;
    }
}
