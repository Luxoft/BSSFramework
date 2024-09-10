using Framework.DomainDriven;
using Framework.Restriction;

namespace SampleSystem.Domain;

public abstract class DomainObjectIntegrationSaveModel<TDomainObject> : DomainObjectBase, IDomainObjectIntegrationSaveModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Required]
    public virtual TDomainObject SavingObject { get; set; }
}
