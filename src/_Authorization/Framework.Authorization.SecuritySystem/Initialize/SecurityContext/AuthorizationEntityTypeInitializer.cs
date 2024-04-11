using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;
using Serilog;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationEntityTypeInitializer : IAuthorizationEntityTypeInitializer
{
    private readonly IRepository<SecurityContextType> entityTypeRepository;

    private readonly List<ISecurityContextInfo<Guid>> securityContextInfoList;

    private readonly ILogger logger;

    private readonly InitializeSettings settings;

    public AuthorizationEntityTypeInitializer(
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<SecurityContextType> entityTypeRepository,
        IEnumerable<ISecurityContextInfo<Guid>> securityContextInfoList,
        ILogger logger,
        InitializeSettings settings)
    {
        this.securityContextInfoList = securityContextInfoList.ToList();
        this.logger = logger.ForContext<AuthorizationEntityTypeInitializer>();
        this.entityTypeRepository = entityTypeRepository;
        this.settings = settings;
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        var dbEntityTypes = await this.entityTypeRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeResult = dbEntityTypes.GetMergeResult(this.securityContextInfoList, et => et.Id, sc => sc.Id);

        if (mergeResult.RemovingItems.Any())
        {
            switch (this.settings.UnexpectedAuthElementMode)
            {
                case UnexpectedAuthElementMode.RaiseError:
                    throw new InvalidOperationException(
                        $"Unexpected entity type in database: {mergeResult.RemovingItems.Join(", ")}");

                case UnexpectedAuthElementMode.Remove:
                {
                    foreach (var removingItem in mergeResult.RemovingItems)
                    {
                        this.logger.Verbose("Remove EntityType: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);

                        await this.entityTypeRepository.RemoveAsync(removingItem, cancellationToken);
                    }

                    break;
                }
            }
        }

        foreach (var securityContextInfo in mergeResult.AddingItems)
        {
            var entityTpe = new SecurityContextType(true, securityContextInfo.Type.IsHierarchical())
            {
                Name = securityContextInfo.Name
            };

            this.logger.Verbose("Create EntityType: {0} {1}", entityTpe.Name, entityTpe.Id);

            await this.entityTypeRepository.InsertAsync(entityTpe, securityContextInfo.Id, cancellationToken);
        }
    }
}
