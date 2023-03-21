namespace SampleSystem.Domain;

public abstract class DomainObjectFormatModel<TDomainObject> : DomainObjectBase
        where TDomainObject : PersistentDomainObjectBase
{
    [Framework.Restriction.Required]
    public TDomainObject FormatObject { get; set; }
}
