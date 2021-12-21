using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;

using JetBrains.Annotations;

namespace Framework.Configuration.Generated.DTO
{
    public class SubscriptionContainerDTOMappingService : BLLContextContainer<IConfigurationBLLContext>, IMappingService<SubscriptionContainerStrictDTO, SubscriptionContainer>
    {
        private readonly bool _allowUpdate;


        public SubscriptionContainerDTOMappingService([NotNull] IConfigurationBLLContext context, bool allowUpdate)
            : base(context)
        {
            this._allowUpdate = allowUpdate;
        }


        public void Map(SubscriptionContainerStrictDTO source, SubscriptionContainer target)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));

            var internalMappingService = new SubscriptionContainerInternalDTOMappingService(this.Context);

            internalMappingService.MapSubscriptionContainer(source, target);

            if (!this._allowUpdate)
            {
                var updatesRequest = from domainObject in internalMappingService.UpdateObjects

                                     let changes = this.Context.TrackingService.GetChanges(domainObject)

                                     where changes.HasChange()

                                     select new
                                     {
                                         DomainObject = domainObject,
                                         Changes = changes
                                     };

                var updates = updatesRequest.ToList();

                if (updates.Any())
                {
                    throw new BusinessLogicException("Can't change objects:{0}{1}", Environment.NewLine,

                        updates.Join(Environment.NewLine,

                            pair =>
                            $"{pair.DomainObject.ToFormattedString()}: {pair.Changes.Join(", ", tp => $"{tp.PropertyName} (Old:\"{tp.PreviusValue.Maybe(v => v.ToFormattedString()) ?? "null"}\", New:\"{tp.CurrentValue.Maybe(v => v.ToFormattedString()) ?? "null"}\")")}"));
                }
            }
        }

        private class SubscriptionContainerInternalDTOMappingService : ConfigurationServerPrimitiveDTOMappingServiceBase
        {
            private static readonly HashSet<Type> GraphTypes = new[]
            {
                typeof (Subscription),
                typeof (SubBusinessRole),
                typeof (SubscriptionSecurityItem),

                typeof (SubscriptionLambda),

                typeof (MessageTemplate),

                typeof (AttachmentContainer),
                typeof (Attachment),
                typeof (AttachmentTag)
            }.ToHashSet();

            private static readonly HashSet<Type> UpdateTypes = new[]
            {
                typeof (SubscriptionLambda),

                typeof (MessageTemplate),

                typeof (AttachmentContainer),
                typeof (Attachment),
                typeof (AttachmentTag)
            }.ToHashSet();


            internal readonly HashSet<PersistentDomainObjectBase> UpdateObjects = new HashSet<PersistentDomainObjectBase>();

            private readonly Dictionary<Type, Dictionary<Guid, PersistentDomainObjectBase>> _localObjects =

                new Dictionary<Type, Dictionary<Guid, PersistentDomainObjectBase>>();


            public SubscriptionContainerInternalDTOMappingService(IConfigurationBLLContext context)
                : base(context)
            {
            }

            public override TDomainObject GetById<TDomainObject>(Guid ident, IdCheckMode checkMode = IdCheckMode.SkipEmpty, LockRole lockRole = LockRole.None)
            {
                return this.GetLocalObject<TDomainObject>(ident) ?? base.GetById<TDomainObject>(ident, checkMode, lockRole);
            }


            protected override TDomainObject GetByIdOrCreate<TDomainObject>(Guid ident, Func<TDomainObject> createFunc)
            {
                if (ident.IsDefault())
                {
                    throw new ArgumentOutOfRangeException(nameof(ident));
                }

                if (!GraphTypes.Contains(typeof(TDomainObject)))
                {
                    throw new ArgumentOutOfRangeException("TDomainObject");
                }

                return this.GetById<TDomainObject>(ident, IdCheckMode.DontCheck) ?? createFunc().Self(newObj =>
                {
                    newObj.Id = ident;

                    var dict = this._localObjects.GetValueOrCreate(typeof(TDomainObject), () => new Dictionary<Guid, PersistentDomainObjectBase>());

                    dict.Add(ident, newObj);
                });
            }

            protected override TDomainObject ToDomainObject<TMappingObject, TDomainObject>(TMappingObject mappingObject)
            {
                var domainObject = base.ToDomainObject<TMappingObject, TDomainObject>(mappingObject);

                this.TryUpdateObject(domainObject);

                return domainObject;
            }

            protected override TDomainObject ToDomainObject<TMappingObject, TDomainObject>(TMappingObject mappingObject, Func<TDomainObject> createFunc)
            {
                var domainObject = base.ToDomainObject(mappingObject, createFunc);

                this.TryUpdateObject(domainObject);

                return domainObject;
            }

            private void TryUpdateObject<TDomainObject>(TDomainObject domainObject)
                where TDomainObject : PersistentDomainObjectBase
            {
                if (UpdateTypes.Contains(typeof(TDomainObject)) && !this.HasLocalObject(domainObject))
                {
                    this.UpdateObjects.Add(domainObject);
                }
            }

            private TDomainObject GetLocalObject<TDomainObject>(Guid ident)
                where TDomainObject : PersistentDomainObjectBase
            {
                var localRequest = from dict in this._localObjects.GetMaybeValue(typeof(TDomainObject))

                                   from obj in dict.GetMaybeValue(ident)

                                   select (TDomainObject)obj;

                return localRequest.GetValueOrDefault();
            }

            private bool HasLocalObject<TDomainObject>(TDomainObject domainObject)
                where TDomainObject : PersistentDomainObjectBase
            {
                var localRequest = from dict in this._localObjects.GetMaybeValue(typeof(TDomainObject))

                                   select dict.ContainsValue(domainObject);

                return localRequest.GetValueOrDefault();
            }
        }
    }
}
