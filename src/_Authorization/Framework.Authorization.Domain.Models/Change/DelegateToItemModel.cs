using Framework.Persistent;

namespace Framework.Authorization.Domain;

public class DelegateToItemModel : DomainObjectBase
{
    [Restriction.Required]
    public Principal Principal { get; set; }

    [Restriction.Required]
    [DetailRole(true)]
    public Permission Permission { get; set; }
}
