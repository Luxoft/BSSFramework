using Framework.BLL.Domain.DirectMode;
using Framework.Restriction;

namespace Framework.BLL.Domain.Models;

[DirectMode(DirectMode.DirectMode.In)]
public interface IDomainObjectIntegrationSaveModel<out TDomainObject>
{
    [Required]
    TDomainObject SavingObject { get; }
}
