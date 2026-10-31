using Framework.Database.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Message sent to the user
/// </summary>
[NotAuditedClass]
public class SentMessage : AuditPersistentDomainObjectBase
{
    private readonly string from = null!;
    private readonly string to = null!;
    private readonly string copy = null!;
    private readonly string subject = null!;
    private readonly string message = null!;
    private readonly string templateName = null!;
    private readonly string comment = null!;
    private readonly string contextObjectType = null!;
    private readonly Guid? contextObjectId;
    private readonly string replyTo = null!;

    protected SentMessage()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="from">Sender</param>
    /// <param name="to">Recipient</param>
    /// <param name="subject">Message subject template</param>
    /// <param name="message">Message</param>
    /// <param name="templateName">Template name</param>
    /// <param name="comment">Comment</param>
    /// <param name="copy">Additional recipients in copy of the letter</param>
    /// <param name="contextObjectType">Domain type the subscription is registered for</param>
    /// <param name="contextObjectId">ID of the domain type the subscription is registered for</param>
    /// <param name="replyTo">Reply-to recipients of the letter</param>
    public SentMessage(string @from, string to, string subject, string message, string templateName, string comment, string copy, string contextObjectType, Guid? contextObjectId, string replyTo)
    {
        this.from = from;
        this.contextObjectId = contextObjectId;
        this.contextObjectType = contextObjectType;
        this.comment = comment;
        this.templateName = templateName;
        this.message = message;
        this.subject = subject;
        this.to = to;
        this.copy = copy;
        this.replyTo = replyTo;
    }

    /// <summary>
    /// ID of the domain type the subscription is registered for
    /// </summary>
    public virtual Guid? ContextObjectId => this.contextObjectId;

    /// <summary>
    /// Domain type the subscription is registered for
    /// </summary>
    public virtual string ContextObjectType => this.contextObjectType;

    /// <summary>
    /// Additional recipient in copy of the letter
    /// </summary>
    [MaxLength]
    public virtual string Copy => this.copy;

    /// <summary>
    /// Notification recipient(s)
    /// </summary>
    [MaxLength]
    public virtual string To => this.to;

    /// <summary>
    /// Notification subject template
    /// </summary>
    [MaxLength(1000)]
    public virtual string Subject => this.subject;

    /// <summary>
    /// Notification text template
    /// </summary>
    [MaxLength]
    public virtual string Message => this.message;

    /// <summary>
    /// Notification template name
    /// </summary>
    public virtual string TemplateName => this.templateName;

    /// <summary>
    /// Comments
    /// </summary>
    public virtual string Comment => this.comment;

    /// <summary>
    /// Notification sender
    /// </summary>
    public virtual string From => this.from;

    /// <summary>
    /// ReplyTo of the notification
    /// </summary>
    [MaxLength]
    public virtual string ReplyTo => this.replyTo;
}
