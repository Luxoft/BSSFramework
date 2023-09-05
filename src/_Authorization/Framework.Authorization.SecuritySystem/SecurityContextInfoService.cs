using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityContextInfoService : ISecurityContextInfoService<Guid>
{
    private readonly IRepository<EntityType> entityTypeRepository;

    private readonly IReadOnlyDictionary<string, Guid> entityTypes;

    public SecurityContextInfoService(IRepositoryFactory<EntityType> entityTypeRepositoryFactory)
    {
        this.entityTypeRepository = entityTypeRepositoryFactory.Create();
    }

    public SecurityContextInfo<Guid> GetSecurityContextInfo(Type type)
    {

    }
}
