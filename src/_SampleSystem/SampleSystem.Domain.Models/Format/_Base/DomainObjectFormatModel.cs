using Framework.Restriction;

namespace SampleSystem.Domain;

public abstract class DomainObjectFormatModel<TDomainObject> : DomainObjectBase
        where TDomainObject : PersistentDomainObjectBase
{
    [Required]
    public TDomainObject FormatObject { get; set; }
}
