using Framework.DomainDriven;
using Framework.Persistent;

namespace Framework.Authorization.Domain;

public class DelegateToItemModel : DomainObjectBase
{
    [Restriction.Required]
    public Principal Principal { get; set; }

    [Restriction.Required]
    [DetailRole(true)]
    [AutoMapping(false)]
    public Permission Permission { get; set; }
}
