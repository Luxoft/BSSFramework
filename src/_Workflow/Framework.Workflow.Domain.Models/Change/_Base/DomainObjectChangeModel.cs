using Framework.DomainDriven;

namespace Framework.Workflow.Domain
{
    [DirectMode(DirectMode.In | DirectMode.Out)]
    public abstract class DomainObjectChangeModel<TDomainObject> : DomainObjectBase, IDomainObjectChangeModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {
        [Framework.Restriction.Required]
        public TDomainObject ChangingObject { get; set; }
    }
}