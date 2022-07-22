using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;

using Framework.Core;
using Framework.Events;

namespace Framework.Authorization.Events
{
    public class DefaultAuthDALListener : DependencyDetailEventDALListener<IAuthorizationBLLContext, PersistentDomainObjectBase>
    {
        public DefaultAuthDALListener(IAuthorizationBLLContext context, IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender)
                : base(context, messageSender, DefaultEventTypes, DefaultDependencyEvents)
        {
        }

        public static readonly IReadOnlyCollection<TypeEvent> DefaultEventTypes = new[]
        {
                TypeEvent.Save<Principal>(),
                TypeEvent.SaveAndRemove<Permission>(),
                TypeEvent.SaveAndRemove<BusinessRole>()
        };


        public static readonly IReadOnlyCollection<TypeEventDependency> DefaultDependencyEvents = new[]
        {
                TypeEventDependency.FromSaveAndRemove<PermissionFilterItem, Permission>(z => z.Permission),
                TypeEventDependency.FromSaveAndRemove<Permission, Principal>(z => z.Principal)
        };
    }
}
