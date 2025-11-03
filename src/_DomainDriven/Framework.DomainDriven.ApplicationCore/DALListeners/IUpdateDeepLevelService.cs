namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public interface IUpdateDeepLevelService<in TDomainObject>
{
    Task UpdateDeepLevels(IEnumerable<TDomainObject> domainObjects, CancellationToken cancellationToken);
}
