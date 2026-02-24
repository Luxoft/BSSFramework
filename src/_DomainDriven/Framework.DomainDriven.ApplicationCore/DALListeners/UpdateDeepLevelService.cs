using CommonFramework;

using Framework.Persistent;

using HierarchicalExpand;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class UpdateDeepLevelService<TDomainObject>(
    IDomainObjectExpanderFactory<TDomainObject> domainObjectExpanderFactory,
    HierarchicalInfo<TDomainObject> hierarchicalInfo,
    DeepLevelInfo<TDomainObject> deepLevelInfo) : IUpdateDeepLevelService<TDomainObject>
    where TDomainObject : class
{
    public async Task UpdateDeepLevels(IEnumerable<TDomainObject> domainObjects, CancellationToken cancellationToken)
    {
        foreach (var domainObject in await domainObjectExpanderFactory.Create().GetAllChildren(domainObjects, cancellationToken))
        {
            deepLevelInfo.Setter.Invoke(
                domainObject,
                domainObject.GetAllElements(v => hierarchicalInfo.ParentFunc(v), true).Count());
        }
    }
}
