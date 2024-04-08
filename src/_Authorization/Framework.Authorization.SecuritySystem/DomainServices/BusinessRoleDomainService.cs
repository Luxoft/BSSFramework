using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem.DomainServices;

public class BusinessRoleDomainService : IBusinessRoleDomainService
{
    private readonly IRepository<BusinessRole> businessRoleRepository;

    public BusinessRoleDomainService([FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<BusinessRole> businessRoleRepository)
    {
        this.businessRoleRepository = businessRoleRepository;
    }

    public async Task<BusinessRole> GetAdminRole(CancellationToken cancellationToken = default)
    {
        return await this.GetRole(BusinessRole.AdminRoleName, cancellationToken);
    }

    public async Task<BusinessRole> GetRole(string name, CancellationToken cancellationToken = default)
    {
        return await this.businessRoleRepository.GetQueryable().Where(v => v.Name == name)
                   .SingleOrDefaultAsync(cancellationToken)
               ?? throw new Exception($"BusinessRole with name '{name}' not found");
    }

    public async Task<BusinessRole> GetOrCreateEmptyAdminRole(CancellationToken cancellationToken = default)
    {
        var businessRoleRepository = this.businessRoleRepository;

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
