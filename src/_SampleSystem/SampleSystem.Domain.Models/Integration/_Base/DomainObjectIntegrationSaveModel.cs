using Framework.BLL.Domain.Models;
using Framework.Restriction;

namespace SampleSystem.Domain.Models.Integration._Base;

public abstract class DomainObjectIntegrationSaveModel<TDomainObject> : DomainObjectBase, IDomainObjectIntegrationSaveModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Required]
    public virtual TDomainObject SavingObject { get; set; }
}
