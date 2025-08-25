using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;

using SecuritySystem;

using Microsoft.Extensions.Logging;

using SecuritySystem.Attributes;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationBusinessRoleInitializer(
    [DisabledSecurity] IRepository<BusinessRole> businessRoleRepository,
    ISecurityRoleSource securityRoleSource,
    ILogger<AuthorizationBusinessRoleInitializer> logger,
    InitializerSettings settings)
    : IAuthorizationBusinessRoleInitializer
{
    public async Task<MergeResult<BusinessRole, FullSecurityRole>> Init(CancellationToken cancellationToken)
    {
        return await this.Init(securityRoleSource.GetRealRoles(), cancellationToken);
    }

    public async Task<MergeResult<BusinessRole, FullSecurityRole>> Init(
        IEnumerable<FullSecurityRole> securityRoles,
        CancellationToken cancellationToken)
    {
        var dbRoles = await businessRoleRepository.GetQueryable().GenericToListAsync(cancellationToken);

        var mergeResult = dbRoles.GetMergeResult(securityRoles, br => br.Id, sr => sr.Id);

        if (mergeResult.RemovingItems.Any())
        {
            switch (settings.UnexpectedAuthElementMode)
            {
                case UnexpectedAuthElementMode.RaiseError:
                    throw new InvalidOperationException(
                        $"Unexpected roles in database: {mergeResult.RemovingItems.Join(", ")}");

                case UnexpectedAuthElementMode.Remove:
                {
                    foreach (var removingItem in mergeResult.RemovingItems)
                    {
                        logger.LogDebug("Role removed: {Name} {Id}", removingItem.Name, removingItem.Id);

                        await businessRoleRepository.RemoveAsync(removingItem, cancellationToken);
                    }

                    break;
                }
            }
        }

        foreach (var securityRole in mergeResult.AddingItems)
        {
            var businessRole = new BusinessRole { Name = GetActualName(securityRole), Description = securityRole.Information.Description };

            logger.LogDebug("Role created: {Name} {Id}", businessRole.Name, securityRole.Id);

            await businessRoleRepository.InsertAsync(businessRole, securityRole.Id, cancellationToken);
        }

        foreach (var (businessRole, securityRole) in mergeResult.CombineItems)
        {
            var newName = GetActualName(securityRole);
            var newDescription = securityRole.Information.Description;

            if (newName != businessRole.Name || newDescription != businessRole.Description)
            {
                businessRole.Name = newName;
                businessRole.Description = newDescription;

                logger.LogDebug("Role updated: {Name} {Description} {Id}", businessRole.Name, businessRole.Description, securityRole.Id);

                await businessRoleRepository.SaveAsync(businessRole, cancellationToken);
            }
        }

        return mergeResult;
    }

    private static string GetActualName(FullSecurityRole securityRole)
    {
        return securityRole.Information.CustomName ?? securityRole.Name;
    }

    async Task ISecurityInitializer.Init(CancellationToken cancellationToken) => await this.Init(cancellationToken);
}
