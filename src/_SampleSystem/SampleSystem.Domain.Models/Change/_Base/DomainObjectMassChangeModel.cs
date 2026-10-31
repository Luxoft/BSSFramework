using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.Models;
using Framework.Restriction;

namespace SampleSystem.Domain.Models.Change._Base;

/// <summary>
/// Common type for the base model for changing a collection of objects
/// </summary>
/// <typeparam name="TDomainObject">Type of the object being changed</typeparam>
[DirectMode(DirectMode.In | DirectMode.Out)]
public abstract class DomainObjectMassChangeModel<TDomainObject> : DomainObjectBase, IDomainObjectMassChangeModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Required]
    public List<TDomainObject> ChangingObjects { get; set; } = null!;

    IEnumerable<TDomainObject> IDomainObjectMassChangeModel<TDomainObject>.ChangingObjects => this.ChangingObjects;
}
