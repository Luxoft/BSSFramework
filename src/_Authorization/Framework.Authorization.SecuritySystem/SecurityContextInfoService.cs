using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityContextInfoService : ISecurityContextInfoService<Guid>
{
    private readonly IRealTypeResolver realTypeResolver;

    private readonly IRepository<EntityType> entityTypeRepository;

    private readonly Lazy<IReadOnlyDictionary<string, Guid>> lazyEntityTypeDict;

    public SecurityContextInfoService(IRepositoryFactory<EntityType> entityTypeRepositoryFactory, IRealTypeResolver realTypeResolver)
    {
        this.realTypeResolver = realTypeResolver;
        this.entityTypeRepository = entityTypeRepositoryFactory.Create();

        this.lazyEntityTypeDict =
            LazyHelper.Create(() => this.entityTypeRepository.GetQueryable().ToList().ToReadOnlyDictionaryI(v => v.Name, v => v.Id));
    }

    public SecurityContextInfo<Guid> GetSecurityContextInfo(Type type)
    {
        var realType = this.realTypeResolver.Resolve(type);

        var realTypeId = this.lazyEntityTypeDict.Value[realType.Name];

        return new SecurityContextInfo<Guid>(realTypeId, realType.Name);
    }
}
