using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

namespace Framework.DomainDriven.Setup;

public interface ITargetSystemRootSettings
{
    bool RegisterBase { get; set; }

    bool RegisterAuthorization { get; set; }

    bool RegisterConfiguration { get; set; }

    ITargetSystemRootSettings AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(string name, Guid id, bool isMain, bool isRevision, IEnumerable<DomainTypeInfo> domainTypes)
        where TBLLContext : class, ITypeResolverContainer<string>,
        ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>;
}
