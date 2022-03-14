using System;
using System.Linq;
using System.Collections.Generic;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL;

public class AttachmentBLLContextModule : IAttachmentBLLContextModule
{
    private readonly Lazy<Dictionary<TargetSystem, ITargetSystemService>> lazyTargetSystemServiceCache;


    public AttachmentBLLContextModule([NotNull] Framework.Configuration.BLL.IConfigurationBLLContext configuration, IEnumerable<ITargetSystemService> targetSystemServices)
    {
        this.Configuration = configuration;
        this.lazyTargetSystemServiceCache = LazyHelper.Create(() => targetSystemServices.ToDictionary(s => s.TargetSystem));
    }


    public ITargetSystemService GetPersistentTargetSystemService(TargetSystem targetSystem)
    {
        if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

        return this.lazyTargetSystemServiceCache.Value[targetSystem];
    }

    public IEnumerable<ITargetSystemService> GetPersistentTargetSystemServices()
    {
        return this.lazyTargetSystemServiceCache.Value.Values;
    }

    public Framework.Configuration.BLL.IConfigurationBLLContext Configuration { get; }

    public IAuthorizationBLLContextBase Authorization => this.Configuration.Authorization;
}
