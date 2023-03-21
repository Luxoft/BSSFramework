using Framework.DomainDriven;
using Framework.Persistent;

namespace Framework.Authorization.Domain;

public class DelegateToItemModel : DomainObjectBase
{
    [Framework.Restriction.Required]
    public Principal Principal { get; set; }

    [Framework.Restriction.Required]
    [DetailRole(true)]
    [AutoMapping(false)]
    public Permission Permission { get; set; }
}
