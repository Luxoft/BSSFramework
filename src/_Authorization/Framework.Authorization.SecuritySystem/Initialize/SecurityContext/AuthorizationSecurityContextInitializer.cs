using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;
using Serilog;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationSecurityContextInitializer : IAuthorizationSecurityContextInitializer
{
    private readonly IRepository<SecurityContextType> securityContextTypeRepository;

    private readonly List<ISecurityContextInfo<Guid>> securityContextInfoList;

    private readonly ILogger logger;

    private readonly InitializeSettings settings;

    public AuthorizationSecurityContextInitializer(
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<SecurityContextType> securityContextTypeRepository,
        IEnumerable<ISecurityContextInfo<Guid>> securityContextInfoList,
        ILogger logger,
        InitializeSettings settings)
    {
        this.securityContextInfoList = securityContextInfoList.ToList();
        this.logger = logger.ForContext<AuthorizationSecurityContextInitializer>();
        this.securityContextTypeRepository = securityContextTypeRepository;
        this.settings = settings;
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        var dbSecurityContextTypes = await this.securityContextTypeRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeResult = dbSecurityContextTypes.GetMergeResult(this.securityContextInfoList, et => et.Id, sc => sc.Id);

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
                        this.logger.Verbose("Remove SecurityContextType: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);

                        await this.securityContextTypeRepository.RemoveAsync(removingItem, cancellationToken);
                    }

                    break;
                }
            }
        }

        foreach (var securityContextInfo in mergeResult.AddingItems)
        {
            var entityTpe = new SecurityContextType
            {
                Name = securityContextInfo.Name
            };

            this.logger.Verbose("Create SecurityContextType: {0} {1}", entityTpe.Name, entityTpe.Id);

            await this.securityContextTypeRepository.InsertAsync(entityTpe, securityContextInfo.Id, cancellationToken);
        }
    }
}
