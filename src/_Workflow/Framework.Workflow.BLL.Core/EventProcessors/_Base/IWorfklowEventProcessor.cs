using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public interface IWorfklowEventProcessor<in TDomainObject>
        where TDomainObject : DomainObjectBase
    {
        Event GetEvent(TDomainObject domainObject);
    }
}