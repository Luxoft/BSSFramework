using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem.DomainServices;

public class BusinessRoleDomainService : IBusinessRoleDomainService
{
    private readonly IRepositoryFactory<BusinessRole> businessRoleRepositoryFactory;

    public BusinessRoleDomainService(IRepositoryFactory<BusinessRole> businessRoleRepositoryFactory)
    {
        this.businessRoleRepositoryFactory = businessRoleRepositoryFactory;
    }

    public async Task<BusinessRole> GetAdminRole(CancellationToken cancellationToken = default)
    {
        return await this.GetRole(BusinessRole.AdminRoleName, cancellationToken);
    }

    public async Task<BusinessRole> GetRole(string name, CancellationToken cancellationToken = default)
    {
        return await this.businessRoleRepositoryFactory.Create().GetQueryable().Where(v => v.Name == name)
                   .SingleOrDefaultAsync(cancellationToken)
               ?? throw new Exception($"BusinessRole with name '{name}' not found");
    }

    public async Task<BusinessRole> GetOrCreateEmptyAdminRole(CancellationToken cancellationToken = default)
    {
        var businessRoleRepository = this.businessRoleRepositoryFactory.Create();

        var businessRole = await businessRoleRepository.GetQueryable().Where(v => v.Name == BusinessRole.AdminRoleName)
                                                 .SingleOrDefaultAsync(cancellationToken);

        if (businessRole == null)
        {
            businessRole = new BusinessRole { Name = BusinessRole.AdminRoleName };

            await businessRoleRepository.SaveAsync(businessRole, cancellationToken);
        }

        return businessRole;
    }
}
