﻿using Framework.Authorization.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.Setup;

public static class TargetSystemInfoHelper
{
    public static TargetSystemInfo<object> Base { get; } = new(
        PersistentHelper.BaseTargetSystemName,
        PersistentHelper.BaseTargetSystemId,
        false,
        false,
        PersistentHelper.BaseTypes.Select(pair => new DomainTypeInfo(pair.Key, pair.Value)).ToList());

    public static TargetSystemInfo<Authorization.Domain.PersistentDomainObjectBase> Authorization { get; } = new(
        nameof(Authorization),
        new Guid("{f065289e-4dc5-48c9-be44-a2ee0131e631}"),
        false,
        true,
        [
            new (typeof(BusinessRole), new Guid("{3823172C-B703-46FD-A82F-B55833EBCD38}")),
            new (typeof(Permission), new Guid("{5d774041-bc69-4841-b64e-a2ee0131e632}")),
            new (typeof(Principal), new Guid("{fa27cd64-c5e6-4356-9efa-a35b00ff69dd}"))
        ]);

    public static TargetSystemInfo<Configuration.Domain.PersistentDomainObjectBase> Configuration { get; } = new(
        nameof(Configuration),
        new Guid("{50465868-4B49-42CF-A702-A39400E6C317}"),
        false,
        true,
        [
            new (typeof(SystemConstant), new Guid("{42C47133-A8C5-4E8E-9D46-385038BFE2B9}")),
        ]);
}
