using System.Net.Mail;

namespace Framework.Subscriptions.Metadata;

/// <inheritdoc />
public abstract class AttachmentLambdaBase<TDomainObject> :
    LambdaMetadata<TDomainObject, IEnumerable<Attachment>>
    where TDomainObject : class;
