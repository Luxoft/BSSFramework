using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityContextInfoService : ISecurityContextInfoService<Guid>
{
    private readonly IRepository<EntityType> entityTypeRepository;

    private readonly Lazy<IReadOnlyDictionary<string, Guid>> lazyEntityTypeDict;

    public SecurityContextInfoService(IRepositoryFactory<EntityType> entityTypeRepositoryFactory)
    {
        this.entityTypeRepository = entityTypeRepositoryFactory.Create();

        this.lazyEntityTypeDict =
            LazyHelper.Create(() => this.entityTypeRepository.GetQueryable().ToList().ToReadOnlyDictionaryI(v => v.Name, v => v.Id));
    }

    public SecurityContextInfo<Guid> GetSecurityContextInfo(Type type)
    {
        var typeId = this.lazyEntityTypeDict.Value[type.Name];

        return new SecurityContextInfo<Guid>(typeId, type.Name);
    }
}
