using Framework.BLL.Domain.DirectMode;
using Framework.Restriction;

namespace Framework.BLL.Domain.Models;

/// <summary>
/// Common interface of the base model for changing a single object
/// </summary>
/// <typeparam name="TDomainObject">Type of the object being changed</typeparam>
[DirectMode(DirectMode.DirectMode.In | DirectMode.DirectMode.Out)]
public interface IDomainObjectChangeModel<out TDomainObject>
{
    [Required]
    TDomainObject ChangingObject { get; }
}
