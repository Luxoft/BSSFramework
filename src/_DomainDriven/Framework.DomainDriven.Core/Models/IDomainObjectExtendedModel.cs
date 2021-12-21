using Framework.Persistent;
using Framework.Restriction;

namespace Framework.DomainDriven
{
    [DirectMode(DirectMode.Out | DirectMode.In)]
    public interface IDomainObjectExtendedModel<out TDomainObject>
    {
        [DetailRole(true)]
        [Required]
        TDomainObject ExtendedObject { get; }
    }
}