namespace SampleSystem.Domain.Models.Filters._Base;

public abstract class DomainObjectContextFilterModel<TDomainObject> : DomainObjectBase
        where TDomainObject : PersistentDomainObjectBase;
