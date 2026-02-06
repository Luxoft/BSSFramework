using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.Lock;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Configuration.BLL;

public partial class DomainObjectModificationBLL
{
    public ITryResult<int> Process(int limit = 1000)
    {
        this.Context.NamedLockService.LockAsync(ConfigurationNamedLock.ProcessModifications, LockRole.Update).GetAwaiter().GetResult();

        var modifications = this.Context.Logics.DomainObjectModification.GetUnsecureQueryable().Where(m => !m.Processed) // Add Order by time?
                                .Take(limit).ToList();

        var logger = this.Context.ServiceProvider.GetRequiredService<ILogger<DomainObjectModificationBLL>>();
        logger.LogDebug("Found {Count} modifications", modifications.Count);

        var errors = new List<Exception>();

        foreach (var modification in modifications)
        {
            var info = new ObjectModificationInfo<Guid>
                       {
                           Identity = modification.DomainObjectId,
                           ModificationType = modification.Type,
                           Revision = modification.Revision,
                           TypeInfo = new TypeInfoDescription(modification.DomainType)
                       };

            logger.LogDebug("Process modification {DomainObjectId}", modification.DomainObjectId);

            foreach (var tryResult in this.Context.Logics.Subscription.Process(info))
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

    /// <inheritdoc />
    public QueueProcessingState GetProcessingState() =>
        new()
        {
            UnprocessedCount = this.GetUnsecureQueryable().Count(mod => !mod.Processed),
            LastProcessedItemDateTime = this.GetUnsecureQueryable().Where(mod => mod.Processed).Max(mod => mod.ModifyDate)
        };
}
