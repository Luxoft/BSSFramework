using System.Net.Mail;

using CommonFramework;

using Framework.Core;

namespace Framework.Notification;

public class MessageTemplateNotification
{
    private readonly IEnumerable<string> copyReceivers = new List<string>();

    private readonly IEnumerable<string> replyTo = new List<string>();

    private readonly string messageTemplateCode;

    public MessageTemplateNotification(
            string messageTemplateCode,
            object contextObject,
            Type contextObjectType,
            IEnumerable<string> receivers,
            IEnumerable<string> copyReceivers,
            IEnumerable<Attachment> attachments,
            ISubscription subscription = null,
            bool sendWithEmptyListOfRecipients = false,
            Type sourceContextObjectType = null)
            : this(messageTemplateCode, contextObject, contextObjectType, receivers, copyReceivers, null, attachments, subscription, sendWithEmptyListOfRecipients, sourceContextObjectType)
    {
    }

    public MessageTemplateNotification(
            string messageTemplateCode,
            object contextObject,
            Type contextObjectType,
            IEnumerable<string> receivers,
            IEnumerable<string> copyReceivers,
            IEnumerable<string> replyToReceivers,
            IEnumerable<Attachment> attachments,
            ISubscription subscription = null,
            bool sendWithEmptyListOfRecipients = false,
            Type sourceContextObjectType = null)
            : this(messageTemplateCode, contextObject, contextObjectType, receivers, attachments, subscription, sendWithEmptyListOfRecipients, sourceContextObjectType)
    {
        if (copyReceivers == null)
        {
            throw new ArgumentNullException(nameof(copyReceivers));
        }

        this.copyReceivers = copyReceivers.ToList();
        this.replyTo = replyToReceivers.EmptyIfNull().ToList();
    }

    public MessageTemplateNotification(
            string messageTemplateCode,
            object contextObject,
            Type contextObjectType,
            IEnumerable<string> receivers,
            IEnumerable<Attachment> attachments,
            ISubscription subscription = null,
            bool sendWithEmptyListOfRecipients = false,
            Type sourceContextObjectType = null)
    {
        if (receivers == null)
        {
            throw new ArgumentNullException(nameof(receivers));
        }

        this.messageTemplateCode = messageTemplateCode ?? throw new ArgumentNullException(nameof(messageTemplateCode));
        this.ContextObject = contextObject ?? throw new ArgumentNullException(nameof(contextObject));
        this.ContextObjectType = contextObjectType ?? throw new ArgumentNullException(nameof(contextObjectType));
        this.Receivers = receivers.ToList();
        this.Subscription = subscription;
        this.SendWithEmptyListOfRecipients = sendWithEmptyListOfRecipients;
        this.Attachments = attachments;
        this.SourceContextObjectType = sourceContextObjectType ?? contextObjectType;
    }

    public Type ContextObjectType { get; private set; }

    /// <summary>
    /// Тип доменного объекта нотификации, на который подписан ConditionLambda.
    /// В общем случае совпадает с ContextObjectType, но может быть переопределен при формировании получателей
    /// </summary>
    public Type SourceContextObjectType { get; private set; }

    /// <summary>
    /// Получает код шаблона уведомления.
    /// </summary>
    /// <value>
    /// Код шаблона уведомления.
    /// </value>
    public string MessageTemplateCode => !string.IsNullOrEmpty(this.messageTemplateCode)
                                                 ? this.messageTemplateCode
                                                 : this.RazorMessageTemplateType.FullName;

    public object ContextObject { get; private set; }

    public IEnumerable<string> Receivers { get; private set; }

    public IEnumerable<Attachment> Attachments { get; private set; }

    public IEnumerable<string> CopyReceivers => this.copyReceivers;

    public IEnumerable<string> ReplyTo => this.replyTo;

    public ISubscription Subscription { get; private set; }

    public bool SendWithEmptyListOfRecipients { get; private set; }

    /// <summary>
    /// Задаёт или возвращает тип, реализующий Razor разметку шаблона сообщения.
    /// </summary>
    /// <value>
    /// тип, реализующий Razor разметку шаблона сообщения.
    /// </value>
    public Type RazorMessageTemplateType { get; set; }

    /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return this.MessageTemplateCode;
    }
}
