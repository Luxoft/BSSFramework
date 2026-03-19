using CommonFramework;
using CommonFramework.IdentitySource;

using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public class DomainObjectSaveStrategy<TDomainObject>(IServiceProxyFactory serviceProxyFactory) : IDomainObjectSaveStrategy<TDomainObject>
{
    private readonly IDomainObjectSaveStrategy<TDomainObject> innerService = serviceProxyFactory.Create<IDomainObjectSaveStrategy<TDomainObject>>();

    public Task SaveAsync(ISession session, TDomainObject domainObject, CancellationToken cancellationToken) =>
        this.innerService.SaveAsync(session, domainObject, cancellationToken);
}

public class DomainObjectSaveStrategy<TDomainObject, TIdent>(IIdentityInfo<TDomainObject, TIdent> identityInfo) : IDomainObjectSaveStrategy<TDomainObject>
{
    public Task SaveAsync(ISession session, TDomainObject domainObject, CancellationToken cancellationToken)
    {
        if (!session.Contains(domainObject))
        {
            var id = identityInfo.Id.Getter(domainObject);

            if (!EqualityComparer<TIdent>.Default.Equals(id, default))
            {
                return session.SaveAsync(domainObject, id, cancellationToken);
            }
        }

        return session.SaveOrUpdateAsync(domainObject, cancellationToken);
    }
}
