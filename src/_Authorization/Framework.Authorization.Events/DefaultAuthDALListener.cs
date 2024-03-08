using Framework.Authorization.Domain;
using Framework.Events;

namespace Framework.Authorization.Events;

public class DefaultAuthDALListener : DependencyDetailEventDALListener<PersistentDomainObjectBase>
{
    public DefaultAuthDALListener(IEventDTOMessageSender<PersistentDomainObjectBase> messageSender)
            : base(messageSender, DefaultEventTypes, DefaultDependencyEvents)
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
