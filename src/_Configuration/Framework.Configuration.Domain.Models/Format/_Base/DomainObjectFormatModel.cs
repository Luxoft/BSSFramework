

namespace Framework.Configuration.Domain;

public abstract class DomainObjectFormatModel<TDomainObject> : DomainObjectBase
        where TDomainObject : PersistentDomainObjectBase
{
    [Restriction.Required]
    public TDomainObject FormatObject { get; set; }
}
