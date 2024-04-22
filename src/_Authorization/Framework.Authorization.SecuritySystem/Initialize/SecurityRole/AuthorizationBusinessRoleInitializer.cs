using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationBusinessRoleInitializer(
    [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<BusinessRole> businessRoleRepository,
    ISecurityRoleSource securityRoleSource,
    ILogger<AuthorizationBusinessRoleInitializer> logger,
    InitializerSettings settings)
    : IAuthorizationBusinessRoleInitializer
{
    public async Task Init(CancellationToken cancellationToken)
    {
        await this.Init(securityRoleSource.SecurityRoles, cancellationToken);
    }

    public async Task Init(IEnumerable<FullSecurityRole> securityRoles, CancellationToken cancellationToken)
    {
        var dbRoles = await businessRoleRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeRoleResult = dbRoles.GetMergeResult(GetOrderedRoles(securityRoles), br => br.Id, sr => sr.Id);

        var mappingDict = new Dictionary<SecurityRole, BusinessRole>();

        if (mergeRoleResult.RemovingItems.Any())
        {
            switch (settings.UnexpectedAuthElementMode)
            {
                case UnexpectedAuthElementMode.RaiseError:
                    throw new InvalidOperationException(
                        $"Unexpected roles in database: {mergeRoleResult.RemovingItems.Join(", ")}");

                case UnexpectedAuthElementMode.Remove:
                {
                    foreach (var removingItem in mergeRoleResult.RemovingItems)
                    {
                        logger.LogDebug("Remove Role: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);

                        await businessRoleRepository.RemoveAsync(removingItem, cancellationToken);
                    }

                    break;
                }
            }
        }

        foreach (var securityRole in mergeRoleResult.AddingItems)
        {
            var businessRole = new BusinessRole
            {
                Name = securityRole.Information.CustomName ?? securityRole.Name,
                Description = securityRole.Information.Description
            };

            logger.LogDebug("Create Role: {BusinessRole} {SecurityRole}", businessRole.Name, securityRole.Id);

            await businessRoleRepository.InsertAsync(businessRole, securityRole.Id, cancellationToken);

            mappingDict.Add(securityRole, businessRole);
        }

        foreach (var combinePair in mergeRoleResult.CombineItems)
        {
            var businessRole = combinePair.Item1;
            var securityRole = combinePair.Item2;

            businessRole.Description = securityRole.Information.Description;

            await businessRoleRepository.SaveAsync(businessRole, cancellationToken);

            mappingDict.Add(securityRole, businessRole);
        }
    }

    private static IEnumerable<FullSecurityRole> GetOrderedRoles(IEnumerable<FullSecurityRole> securityRoles)
    {
        var startParts = securityRoles.Partial(sr => sr.Information.Children.Any(), (rootRoles, leafRoles) => new { rootRoles, leafRoles });

        var orderedResult = startParts.leafRoles.ToList();

        var processed = startParts.leafRoles.ToHashSet();

        var unprocessed = new Queue<FullSecurityRole>(startParts.rootRoles);

        while (unprocessed.Any())
        {
            var currentRole = unprocessed.Dequeue();

            if (currentRole.Information.Children.All(processed.Contains))
            {
                processed.Add(currentRole);
                orderedResult.Add(currentRole);
            }
            else
            {
                unprocessed.Enqueue(currentRole);
            }
        }

        return orderedResult;
    }
}
