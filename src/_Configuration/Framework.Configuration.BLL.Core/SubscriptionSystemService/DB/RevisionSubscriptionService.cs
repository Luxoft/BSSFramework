using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Notification;
using Framework.Persistent;

using JetBrains.Annotations;

using Serilog;

namespace Framework.Configuration.BLL.DBSubscription
{
    public class RevisionSubscriptionService<TBLLContext, TPersistentDomainObjectBase> : SubscriptionSystemService<TBLLContext>, IRevisionSubscriptionSystemService

        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        public RevisionSubscriptionService(IConfigurationBLLContext context, ITargetSystemService<TBLLContext> targetSystemService) : base(context, targetSystemService)
        {

        }

        public RevisionSubscriptionService(IConfigurationBLLContext context, ITargetSystemService<TBLLContext> targetSystemService, ILogger logger, IMessageSender<MessageTemplateNotification> subscriptionSender)
            : base(context, targetSystemService, logger, subscriptionSender)
        {

        }

        public ITryResult<Subscription> Process(Subscription subscription, long? revision, Guid domainObjectId)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));
            if (domainObjectId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainObjectId));

            try
            {
                var domainType = subscription.DomainType.Pipe(t => this.TargetSystemService.TypeResolver.Resolve(t, true));

                Func<Subscription, long?, Guid, ITryResult<Subscription>> del = this.TypedProcess<TPersistentDomainObjectBase>;

                return del.CreateGenericMethod(domainType).Invoke<ITryResult<Subscription>>(this, new object[]
                {
                    subscription,
                    revision,
                    domainObjectId
                });
            }
            catch (Exception ex)
            {
                return TryResult.CreateFault<Subscription>(ex);
            }
        }

        private ITryResult<Subscription> TypedProcess<TDomainObject>(Subscription subscription, long? tryRevision, Guid domainObjectId)

            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var domainObjectBLL = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

            if (tryRevision == null)
            {
                var objectsRevisions = domainObjectBLL.GetObjectRevisions(domainObjectId).RevisionInfos.OrderByDescending(info => info.Date).ToList();

                var revision = objectsRevisions.First(() => new ArgumentException(
                                                                $"Object with id:{domainObjectId} are not found. (Type:{typeof(TDomainObject).Name})")).RevisionNumber;

                var domainObjectExists = domainObjectBLL.GetUnsecureQueryable().Any(obj => obj.Id == domainObjectId);

                var next = !domainObjectExists
                         ? null
                         : domainObjectBLL.GetObjectByRevision(domainObjectId, revision)
                                          .FromMaybe(() => new ArgumentException(
                                                               $"Object with id:{domainObjectId} are not found. (Type:{typeof(TDomainObject).Name} Revision:{revision})"));

                var prev = domainObjectBLL.GetPreviousRevision(domainObjectId, revision).MaybeNullable(

                    previousRevision => domainObjectBLL.GetObjectByRevision(domainObjectId, previousRevision));

                return this.TryProcessSubscription(prev, next, subscription);
            }
            else
            {
                var revision = tryRevision.Value;

                var next = domainObjectBLL.GetObjectByRevision(domainObjectId, revision)
                                          .FromMaybe(() => new ArgumentException(
                                                               $"Object with id:{domainObjectId} are not found. (Type:{typeof(TDomainObject).Name} Revision:{revision})"));

                var prev = domainObjectBLL.GetPreviousRevision(domainObjectId, revision).MaybeNullable(

                            previousRevision => domainObjectBLL.GetObjectByRevision(domainObjectId, previousRevision));

                return this.TryProcessSubscription(prev, next, subscription);
            }
        }

        public IList<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo)
        {
            if (changedObjectInfo == null) throw new ArgumentNullException(nameof(changedObjectInfo));

            var domainType = this.Context.Logics.DomainType.GetByDomainType(changedObjectInfo.TypeInfo)
                                 .Pipe(t => this.TargetSystemService.TypeResolver.Resolve(t, true));

            Func<Guid, long, ModificationType, IList<ITryResult<Subscription>>> del = this.TypedProcess<TPersistentDomainObjectBase>;

            return del.CreateGenericMethod(domainType)
                      .Invoke<IList<ITryResult<Subscription>>>(this, new object[]
            {
                changedObjectInfo.Identity,
                changedObjectInfo.Revision,
                changedObjectInfo.ModificationType
            });
        }

        public IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes)
        {
            if (changes == null) throw new ArgumentNullException(nameof(changes));

            foreach (var itemGroup in changes.GetSubset(typeof(TPersistentDomainObjectBase)).GroupByType())
            {
                var objectType = itemGroup.Key;

                if (this.Context.Logics.Subscription.HasActiveSubscriptions(objectType))
                {
                    var currentRevision = this.GetCurrentRevision(objectType);

                    var items = itemGroup.Value.ToChangeTypeDict();

                    if (0 == currentRevision)
                    {
                        throw new System.ArgumentException("CurrentRevision is 0. ModifiedObjects: " + items.Join(", ", x => $"[Type: '{objectType.Name}', Id: {((TPersistentDomainObjectBase)x.Key).Id}"));
                    }

                    foreach (var pair in items)
                    {
                        var @object = (TPersistentDomainObjectBase)pair.Key;

                        yield return new ObjectModificationInfo<Guid>
                        {
                            Identity = @object.Id,
                            Revision = currentRevision,
                            ModificationType = pair.Value.ToModificationType(),
                            TypeInfo = new TypeInfoDescription(objectType)
                        };
                    }
                }
            }
        }

        private long GetCurrentRevision(Type domainObjectType)
        {
            return new Func<long>(this.GetCurrentRevision<TPersistentDomainObjectBase>).CreateGenericMethod(domainObjectType).Invoke<long>(this);
        }

        private long GetCurrentRevision<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return this.TargetSystemService.TargetSystemContext.Logics.Default.Create<TDomainObject>().GetCurrentRevision();
        }


        [UsedImplicitly(ImplicitUseKindFlags.Default)]
        private IList<ITryResult<Subscription>> TypedProcess<TDomainObject>(Guid domainObjectId, long revision, ModificationType modificationType)

            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var domainObjectBLL = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

            var next = modificationType == ModificationType.Remove
                        ? null
                        : domainObjectBLL.GetObjectByRevision(domainObjectId, revision)
                                         .FromMaybe(() => new ArgumentException($"Object with id:{domainObjectId} are not found. (Type:{typeof(TDomainObject).Name} Revision:{revision})"));

            var prev = domainObjectBLL.GetPreviousRevision(domainObjectId, revision).MaybeNullable(

                previousRevision => domainObjectBLL.GetObjectByRevision(domainObjectId, previousRevision));

            return this.ProcessChangedObject(prev, next);
        }
    }
}
