using System;
using System.Linq;
using System.Collections.Generic;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL;

public class AttachmentBLLContextModule : BLLContextContainer<IConfigurationBLLContext>, IAttachmentBLLContextModule
{
    private readonly Lazy<Dictionary<TargetSystem, ITargetSystemService>> lazyTargetSystemServiceCache;


    public AttachmentBLLContextModule([NotNull] IConfigurationBLLContext context,
                                      IEnumerable<ITargetSystemService> targetSystemServices) : base(context)
    {
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
}
