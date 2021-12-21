using Framework.Workflow.Domain;

namespace Framework.Workflow.BLL
{
    public interface IDomainObjectContract<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {

    }
}