using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.DAL.Revisions;

using Serilog;

namespace Framework.Configuration.BLL;

public partial class DomainObjectModificationBLL
{
    public ITryResult<int> Process(int limit = 1000)
    {
        this.Context.Logics.NamedLock.Lock(NamedLockOperation.ProcessModifications, LockRole.Update);

        var modifications = this.Context.Logics.DomainObjectModification.GetUnsecureQueryable().Where(m => !m.Processed) // Add Order by time?
                                .Take(limit).ToList();

        Log.Verbose("Found {Count} modifications", modifications.Count);

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

            Log.Verbose("Process modification {DomainObjectId}", modification.DomainObjectId);

            foreach (var tryResult in new SubscriptionBLL(this.Context).Process(info))
            {
                tryResult.Match(_ => { }, ex =>
                                          {
                                              Log.Error("Process modification {DomainObjectId} has {Error}", modification.DomainObjectId, ex.Message);
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
    public QueueProcessingState GetProcessingState()
    {
        return new QueueProcessingState
               {
                       UnprocessedCount = this.GetUnsecureQueryable().Count(mod => !mod.Processed),

                       LastProcessedItemDateTime = this.GetUnsecureQueryable().Where(mod => mod.Processed).Max(mod => mod.ModifyDate)
               };
    }
}
