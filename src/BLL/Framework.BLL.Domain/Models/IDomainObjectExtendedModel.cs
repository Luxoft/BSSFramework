using Framework.BLL.Domain.DirectMode;
using Framework.Relations;
using Framework.Restriction;

namespace Framework.BLL.Domain.Models;

[DirectMode(DirectMode.DirectMode.Out | DirectMode.DirectMode.In)]
public interface IDomainObjectExtendedModel<out TDomainObject>
{
    [DetailRole(true)]
    [Required]
    TDomainObject ExtendedObject { get; }
}
