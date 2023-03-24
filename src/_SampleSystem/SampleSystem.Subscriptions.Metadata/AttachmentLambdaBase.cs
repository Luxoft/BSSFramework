using System.Collections.Generic;
using System.Net.Mail;

using Framework.Configuration.SubscriptionModeling;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata;

/// <inheritdoc />
public abstract class AttachmentLambdaBase<TDomainObject> :
        LambdaMetadata<ISampleSystemBLLContext, TDomainObject, IEnumerable<Attachment>>
        where TDomainObject : class
{
}
