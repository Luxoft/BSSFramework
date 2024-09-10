using Framework.DomainDriven;

namespace Framework.Authorization.Domain;

[DirectMode(DirectMode.In | DirectMode.Out)]
public abstract class DomainObjectChangeModel<TDomainObject> : DomainObjectBase, IDomainObjectChangeModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Restriction.Required]
    public TDomainObject ChangingObject { get; set; }
}
