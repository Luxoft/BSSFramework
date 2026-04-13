using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Database;
using Framework.Database.Domain;
using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using Microsoft.Extensions.Logging;

using SecuritySystem.Providers;

namespace Framework.Configuration.BLL;

public partial class DomainObjectModificationBLL(
    IConfigurationBLLContext context,
    ISecurityProvider<DomainObjectModification> securityProvider,
    ISubscriptionService subscriptionService,
    ILogger<DomainObjectModificationBLL> logger,
    IDomainObjectVersionsResolverFactory domainObjectVersionsResolverFactory)
    : SecurityDomainBLLBase<DomainObjectModification>(context, securityProvider)
{
    public ITryResult<int> Process(int limit = 1000)
    {
        this.Context.NamedLockService.LockAsync(ConfigurationNamedLock.ProcessModifications, LockRole.Update).GetAwaiter().GetResult();

        var modifications = this.Context.Logics.DomainObjectModification.GetUnsecureQueryable().Where(m => !m.Processed) // Add Order by time?
                                .Take(limit).ToList();

        logger.LogDebug("Found {Count} modifications", modifications.Count);

        var errors = new List<Exception>();

        foreach (var modification in modifications)
        {
            var info = new ObjectModificationInfo<Guid>(
                Identity: modification.DomainObjectId,
                ModificationType: modification.Type,
                Revision: modification.Revision,
                TypeInfo: new TypeNameIdentity { Name = modification.DomainType.Name, Namespace = modification.DomainType.Namespace });

            logger.LogDebug("Process modification {DomainObjectId}", modification.DomainObjectId);

            var versions = this.GetDomainObjectVersions(info);

            foreach (var tryResult in subscriptionService.Process(versions))
            {
                tryResult.Match(
                    _ => { },
                    ex =>
                    {
                        logger.LogError("Process modification {DomainObjectId} has {Error}", modification.DomainObjectId, ex.Message);
                        errors.Add(ex);
                    });
            }

            modification.Processed = true;

            this.Context.Logics.DomainObjectModification.Save(modification);
        }

        if (errors.Any())
        {
            return TryResult.CreateFault<int>(new AggregateException(errors));
        }
        else
        {
            return TryResult.Return(modifications.Count);
        }
    }

    private DomainObjectVersions GetDomainObjectVersions(ObjectModificationInfo<Guid> modificationInfo)
    {
        var domainObjectType = this.Context.TargetSystemTypeResolver.Resolve(modificationInfo.TypeInfo);

        return domainObjectVersionsResolverFactory.Create(domainObjectType).GetDomainObjectVersions(modificationInfo.Identity, modificationInfo.Revision);
    }

    /// <inheritdoc />
    public QueueProcessingState GetProcessingState() =>
        new()
        {
            UnprocessedCount = this.GetUnsecureQueryable().Count(mod => !mod.Processed),
            LastProcessedItemDateTime = this.GetUnsecureQueryable().Where(mod => mod.Processed).Max(mod => mod.ModifyDate)
        };
}
