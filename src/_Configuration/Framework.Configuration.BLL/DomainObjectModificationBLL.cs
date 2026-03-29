using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Database;
using Framework.Database.Domain;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Configuration.BLL;

public partial class DomainObjectModificationBLL
{
    public ITryResult<int> Process(int limit = 1000)
    {
        this.Context.NamedLockService.LockAsync(ConfigurationNamedLock.ProcessModifications, LockRole.Update).GetAwaiter().GetResult();

        var modifications = Queryable.Where<DomainObjectModification>(this.Context.Logics.DomainObjectModification.GetUnsecureQueryable(), m => !m.Processed) // Add Order by time?
                                     .Take(limit).ToList();

        var logger = ServiceProviderServiceExtensions.GetRequiredService<ILogger<DomainObjectModificationBLL>>(this.Context.ServiceProvider);
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
                TryResultExtensions.Match<Subscription>(
                    tryResult,
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
            UnprocessedCount = Queryable.Count<DomainObjectModification>(this.GetUnsecureQueryable(), mod => !mod.Processed),
            LastProcessedItemDateTime = Queryable.Where<DomainObjectModification>(this.GetUnsecureQueryable(), mod => mod.Processed).Max(mod => mod.ModifyDate)
        };
}
