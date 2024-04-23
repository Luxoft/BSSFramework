using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationSecurityContextInitializer(
    [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<SecurityContextType> securityContextTypeRepository,
    IEnumerable<ISecurityContextInfo<Guid>> securityContextInfoList,
    ILogger<AuthorizationSecurityContextInitializer> logger,
    InitializerSettings settings)
    : IAuthorizationSecurityContextInitializer
{
    private readonly List<ISecurityContextInfo<Guid>> securityContextInfoList = securityContextInfoList.ToList();

    public async Task Init(CancellationToken cancellationToken)
    {
        var dbSecurityContextTypes = await securityContextTypeRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeResult = dbSecurityContextTypes.GetMergeResult(this.securityContextInfoList, et => et.Id, sc => sc.Id);

        if (mergeResult.RemovingItems.Any())
        {
            switch (settings.UnexpectedAuthElementMode)
            {
                case UnexpectedAuthElementMode.RaiseError:
                    throw new InvalidOperationException(
                        $"Unexpected entity type in database: {mergeResult.RemovingItems.Join(", ")}");

                case UnexpectedAuthElementMode.Remove:
                {
                    foreach (var removingItem in mergeResult.RemovingItems)
                    {
                        logger.LogDebug("Remove SecurityContextType: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);

                        await securityContextTypeRepository.RemoveAsync(removingItem, cancellationToken);
                    }

                    break;
                }
            }
        }

        foreach (var securityContextInfo in mergeResult.AddingItems)
        {
            var securityContextType = new SecurityContextType
            {
                Name = securityContextInfo.Name
            };

            logger.LogDebug("Create SecurityContextType: {Name} {Id}", securityContextType.Name, securityContextType.Id);

            await securityContextTypeRepository.InsertAsync(securityContextType, securityContextInfo.Id, cancellationToken);
        }
    }
}
