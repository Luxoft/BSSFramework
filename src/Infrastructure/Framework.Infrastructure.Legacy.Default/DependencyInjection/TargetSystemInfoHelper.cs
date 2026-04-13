using Framework.Authorization.Domain;
using Framework.BLL.Domain.TargetSystem;
using Framework.Configuration.Domain;

namespace Framework.Infrastructure.DependencyInjection;

public static class PersistentTargetSystemInfoHelper
{
    public static PersistentTargetSystemInfo Authorization { get; } =
        new()
        {
            PersistentDomainObjectBaseType = typeof(Authorization.Domain.PersistentDomainObjectBase),
            BllContextType = typeof(Authorization.BLL.IAuthorizationBLLContext),
            Name = nameof(Authorization),
            Id = new("{f065289e-4dc5-48c9-be44-a2ee0131e631}"),
            IsMain = false,
            IsRevision = false,
            Domain = new(
            [
                new(typeof(BusinessRole), new("{3823172C-B703-46FD-A82F-B55833EBCD38}")),
                new(typeof(Permission), new("{5d774041-bc69-4841-b64e-a2ee0131e632}")),
                new(typeof(Principal), new("{fa27cd64-c5e6-4356-9efa-a35b00ff69dd}"))
            ])
        };

    public static PersistentTargetSystemInfo Configuration { get; } =

        new()
        {
            PersistentDomainObjectBaseType = typeof(Configuration.Domain.PersistentDomainObjectBase),
            BllContextType = typeof(Configuration.BLL.IConfigurationBLLContext),
            Name = nameof(Configuration),
            Id = new("{50465868-4B49-42CF-A702-A39400E6C317}"),
            IsMain = false,
            IsRevision = false,
            Domain = new(
            [
                new(typeof(SystemConstant), new("{42C47133-A8C5-4E8E-9D46-385038BFE2B9}")),
            ])
        };
}
