using Framework.DomainDriven.BLL;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public abstract class WorfklowEventProcessor<TDomainObject> : BLLContextContainer<IWorkflowBLLContext>, IWorfklowEventProcessor<TDomainObject>
        where TDomainObject : DomainObjectBase
    {
        protected WorfklowEventProcessor(IWorkflowBLLContext context)
            : base(context)
        {

        }


        public abstract Event GetEvent(TDomainObject domainObject);
    }
}