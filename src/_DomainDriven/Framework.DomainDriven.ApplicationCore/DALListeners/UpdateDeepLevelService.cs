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
        foreach (var domainObject in await domainObjectExpanderFactory
                                           .Create()
                                           .GetAllChildren(domainObjects.Where(this.DeepLevelChanged), cancellationToken))
        {
            deepLevelInfo.DeepLevel.Setter.Invoke(
                domainObject,
                domainObject.GetAllElements(v => hierarchicalInfo.ParentFunc(v), true).Count());
        }
    }

    private bool DeepLevelChanged(TDomainObject domainObject) =>
        deepLevelInfo.DeepLevel.Getter(domainObject) != this.GetActualDeepLevel(domainObject);

    private int GetActualDeepLevel(TDomainObject domainObject) =>
        domainObject.GetAllElements(v => hierarchicalInfo.ParentFunc(v), true).Count();
}
