using System;
using Framework.Core;
using Framework.DomainDriven.BLL;
using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.WebApiCore;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper
    {
        public SampleSystemServiceEnvironment Environment { get; set; }

        public string PrincipalName { private get; set; }

        public AuthHelper AuthHelper { private get; set; }

        public TResult EvaluateWrite<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, this.PrincipalName, func);
        }

        public void EvaluateRead(Action<ISampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, action);
        }

        public TResult EvaluateRead<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, func);
        }

        public void EvaluateWrite(Action<ISampleSystemBLLContext> func)
        {
            this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Write,
                this.PrincipalName,
                context =>
                {
                    func(context);
                    return Ignore.Value;
                });
        }

        public SampleSystemServerPrimitiveDTOMappingService GetMappingService(ISampleSystemBLLContext context)
        {
            return new SampleSystemServerPrimitiveDTOMappingService(context);
        }

        ////private IDateTimeService DateTimeService
        ////{
        ////    get
        ////    {
        ////        return this.Environment.DateTimeService;
        ////    }
        ////}

        private Guid GetGuid(Guid? id)
        {
            id = id ?? Guid.NewGuid();
            return (Guid)id;
        }
    }
}
