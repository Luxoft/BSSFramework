using Framework.Application.Domain;
using Framework.BLL;
using Framework.BLL.Services;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Core.TypeResolving;

namespace Framework.Infrastructure.DependencyInjection;

public interface ITargetSystemSetup
{
    bool RegisterBase { get; set; }

    bool RegisterAuthorization { get; set; }

    bool RegisterConfiguration { get; set; }

    ITargetSystemSetup AddTargetSystem<TBLLContext, TPersistentDomainObjectBase>(string name, Guid id, bool isMain, bool isRevision, IEnumerable<DomainTypeInfo> domainTypes)
        where TBLLContext : class, ITypeResolverContainer<string>,
        ISecurityServiceContainer<IRootSecurityService>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>;
}
