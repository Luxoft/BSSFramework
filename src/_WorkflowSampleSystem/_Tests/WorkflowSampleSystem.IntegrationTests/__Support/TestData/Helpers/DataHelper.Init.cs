using System;

using Framework.Core;
using Framework.DomainDriven.BLL;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.ServiceEnvironment;

namespace WorkflowSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper
    {
        public WorkflowSampleSystemServiceEnvironment Environment { get; set; }

        public string PrincipalName { private get; set; }

        public AuthHelper AuthHelper { private get; set; }

        public TResult EvaluateWrite<TResult>(Func<IWorkflowSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, this.PrincipalName, func);
        }

        public void EvaluateRead(Action<IWorkflowSampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, action);
        }

        public TResult EvaluateRead<TResult>(Func<IWorkflowSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, func);
        }

        public void EvaluateWrite(Action<IWorkflowSampleSystemBLLContext> func)
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

        public WorkflowSampleSystemServerPrimitiveDTOMappingService GetMappingService(IWorkflowSampleSystemBLLContext context)
        {
            return new WorkflowSampleSystemServerPrimitiveDTOMappingService(context);
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
