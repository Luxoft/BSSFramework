using Framework.BLL.Domain.DirectMode;
using Framework.Restriction;

namespace Framework.BLL.Domain.Models;

/// <summary>
/// Common interface of the base model for changing a collection of objects
/// </summary>
/// <typeparam name="TDomainObject">Type of the object being changed</typeparam>
[DirectMode(DirectMode.DirectMode.In | DirectMode.DirectMode.Out)]
public interface IDomainObjectMassChangeModel<out TDomainObject>
{
    [Required]
    IEnumerable<TDomainObject> ChangingObjects { get; }
}
