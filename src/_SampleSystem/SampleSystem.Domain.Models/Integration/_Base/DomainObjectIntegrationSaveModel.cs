using Framework.DomainDriven;

namespace SampleSystem.Domain;

public abstract class DomainObjectIntegrationSaveModel<TDomainObject> : DomainObjectBase, IDomainObjectIntegrationSaveModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Framework.Restriction.Required]
    public virtual TDomainObject SavingObject { get; set; }
}
