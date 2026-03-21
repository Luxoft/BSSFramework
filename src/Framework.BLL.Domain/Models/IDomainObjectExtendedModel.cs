using System.ComponentModel.DataAnnotations;

using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.Persistent.Attributes;

namespace Framework.BLL.Domain.Models;

[DirectMode(DirectMode.DirectMode.Out | DirectMode.DirectMode.In)]
public interface IDomainObjectExtendedModel<out TDomainObject>
{
    [DetailRole(true)]
    [Required]
    TDomainObject ExtendedObject { get; }
}
