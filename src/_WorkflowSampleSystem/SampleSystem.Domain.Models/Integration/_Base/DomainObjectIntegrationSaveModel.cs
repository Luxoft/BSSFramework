using Framework.DomainDriven;
using Framework.Persistent;

namespace SampleSystem.Domain
{
    public abstract class DomainObjectIntegrationSaveModel<TDomainObject> : DomainObjectBase, IDomainObjectIntegrationSaveModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {
        [Framework.Restriction.Required]
        public virtual TDomainObject SavingObject { get; set; }
    }
}