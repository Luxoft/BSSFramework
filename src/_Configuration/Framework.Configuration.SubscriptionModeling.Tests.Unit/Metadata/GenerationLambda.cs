using System;
using System.Collections.Generic;
using Framework.Configuration.Core;
using Framework.Notification;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class GenerationLambda :
        LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>>
{
    public GenerationLambda()
    {
        this.Lambda = (context, versions) => new List<NotificationMessageGenerationInfo>();
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
    }

    internal void SetFunc(
            Func<object, DomainObjectVersions<object>, IEnumerable<NotificationMessageGenerationInfo>> func)
    {
        this.Lambda = func;
    }
}
