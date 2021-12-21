using System;
using System.Collections.Generic;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL
{
    public interface IRevisionSubscriptionSystemService : ISubscriptionSystemService
    {
        ITryResult<Subscription> Process(Subscription subscription, long? revision, Guid domainObjectId);

        IList<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo);

        IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications([NotNull] DALChanges changes);
    }
}
