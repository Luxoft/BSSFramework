using CommonFramework;

using Framework.Persistent;

using HierarchicalExpand;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class UpdateDeepLevelService<TDomainObject>(
    IDomainObjectExpander<TDomainObject> domainObjectExpander,
    HierarchicalInfo<TDomainObject> hierarchicalInfo,
    DeepLevelInfo<TDomainObject> deepLevelInfo) : IUpdateDeepLevelService<TDomainObject>
    where TDomainObject : class
{
    public async Task UpdateDeepLevels(IEnumerable<TDomainObject> domainObjects, CancellationToken cancellationToken)
    {
        foreach (var domainObject in await domainObjectExpander.GetAllChildren(domainObjects, cancellationToken))
        {
            deepLevelInfo.Setter.Invoke(
                domainObject,
                domainObject.GetAllElements(v => hierarchicalInfo.ParentFunc(v), true).Count());
        }
    }
}
