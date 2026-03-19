using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public interface IDomainObjectSaveStrategy<in TDomainObject>
{
    Task SaveAsync(ISession session, TDomainObject domainObject, CancellationToken cancellationToken);
}
