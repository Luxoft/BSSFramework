using System.Collections.Generic;

using Framework.Configuration.SubscriptionModeling;
using Framework.Notification;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata;

/// <inheritdoc />
public abstract class GenerationLambdaBase<TDomainObject> :
        LambdaMetadata<ISampleSystemBLLContext, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>
        where TDomainObject : class
{
}
