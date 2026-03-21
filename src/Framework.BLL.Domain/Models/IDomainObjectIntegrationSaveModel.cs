using System.ComponentModel.DataAnnotations;

using Framework.BLL.Domain.DirectMode;

namespace Framework.BLL.Domain.Models;

[DirectMode(DirectMode.DirectMode.In)]
public interface IDomainObjectIntegrationSaveModel<out TDomainObject>
{
    [Required]
    TDomainObject SavingObject { get; }
}
