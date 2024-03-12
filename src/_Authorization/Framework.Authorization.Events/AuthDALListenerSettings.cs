using Framework.Authorization.Domain;
using Framework.Events;

namespace Framework.Authorization.Events;

public static class AuthDALListenerSettings
{
    public static readonly EventDALListenerSettings<PersistentDomainObjectBase> DefaultSettings =
        new()
        {
            TypeEvents =
            [
                TypeEvent.Save<Principal>(),
                TypeEvent.SaveAndRemove<Permission>(),
                TypeEvent.SaveAndRemove<BusinessRole>()
            ],
            Dependencies = new[]
                           {
                               TypeEventDependency.FromSaveAndRemove<PermissionFilterItem, Permission>(z => z.Permission),
                               TypeEventDependency.FromSaveAndRemove<Permission, Principal>(z => z.Principal)
                           }
        };
}
