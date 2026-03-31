using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.Models;
using Framework.Restriction;

namespace SampleSystem.Domain;

[DirectMode(DirectMode.In | DirectMode.Out)]
public abstract class DomainObjectChangeModel<TDomainObject> : DomainObjectBase, IDomainObjectChangeModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Required]
    public TDomainObject ChangingObject { get; set; }
}
