using Framework.Restriction;

namespace Framework.DomainDriven;

[DirectMode(DirectMode.In)]
public interface IDomainObjectIntegrationSaveModel<out TDomainObject>
{
    [Required]
    TDomainObject SavingObject { get; }
}
