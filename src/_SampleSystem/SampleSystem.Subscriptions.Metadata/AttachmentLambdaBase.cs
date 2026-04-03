using System.Net.Mail;

using Framework.Subscriptions;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata;

/// <inheritdoc />
public abstract class AttachmentLambdaBase<TDomainObject> :
        LambdaMetadata<ISampleSystemBLLContext, TDomainObject, IEnumerable<Attachment>>
        where TDomainObject : class;
