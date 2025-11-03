using CommonFramework;

using Framework.Persistent;

using SecuritySystem.HierarchicalExpand;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class UpdateDeepLevelService<TDomainObject>(
    IDomainObjectExpander<TDomainObject> domainObjectExpander,
    HierarchicalInfo<TDomainObject> hierarchicalInfo) : IUpdateDeepLevelService<TDomainObject>
    where TDomainObject : class, IHierarchicalLevelObjectDenormalized
{
    public async Task UpdateDeepLevels(IEnumerable<TDomainObject> domainObjects, CancellationToken cancellationToken)
    {
        foreach (var domainObject in await domainObjectExpander.GetAllChildren(domainObjects, cancellationToken))
        {
            domainObject.SetDeepLevel(domainObject.GetAllElements(v => hierarchicalInfo.ParentFunc(v), true).Count());
        }
    }
}
