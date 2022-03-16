using System;

using Framework.Core;
using Framework.DomainDriven.BLL;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Generated.DTO;
using AttachmentsSampleSystem.ServiceEnvironment;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper
    {
        public AttachmentsSampleSystemServiceEnvironment Environment { get; set; }

        public string PrincipalName { private get; set; }

        public AuthHelper AuthHelper { private get; set; }

        public TResult EvaluateWrite<TResult>(Func<IAttachmentsSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, this.PrincipalName, func);
        }

        public void EvaluateRead(Action<IAttachmentsSampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, action);
        }

        public TResult EvaluateRead<TResult>(Func<IAttachmentsSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, this.PrincipalName, func);
        }

        public void EvaluateWrite(Action<IAttachmentsSampleSystemBLLContext> func)
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

        public AttachmentsSampleSystemServerPrimitiveDTOMappingService GetMappingService(IAttachmentsSampleSystemBLLContext context)
        {
            return new AttachmentsSampleSystemServerPrimitiveDTOMappingService(context);
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
