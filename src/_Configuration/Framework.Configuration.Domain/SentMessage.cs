using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Сообщение, отправленное пользователю
/// </summary>
[NotAuditedClass]
public class SentMessage : AuditPersistentDomainObjectBase
{
    private readonly string from;
    private readonly string to;
    private readonly string copy;
    private readonly string subject;
    private readonly string message;
    private readonly string templateName;
    private readonly string comment;
    private readonly string contextObjectType;
    private readonly Guid? contextObjectId;
    private readonly string replyTo;

    protected SentMessage()
    {
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="from">Отправитель</param>
    /// <param name="to">Получатель</param>
    /// <param name="subject">Шаблон темы сообщения</param>
    /// <param name="message">Сообщение</param>
    /// <param name="templateName">Имя шаблона</param>
    /// <param name="comment">Комментарий</param>
    /// <param name="copy">Дополнительные получатели в копии письма</param>
    /// <param name="contextObjectType">Доменный тип, на который зарегистрирована подписка</param>
    /// <param name="contextObjectId">ID доменного типа, на который зарегистрирована подписка</param>
    public SentMessage(string @from, string to, string subject, string message, string templateName, string comment, string copy, string contextObjectType, Guid? contextObjectId, string replyTo)
    {
        this.from = from;
        this.contextObjectId = contextObjectId;
        this.contextObjectType = contextObjectType;
        this.copy = copy;
        this.comment = comment;
        this.templateName = templateName;
        this.message = message;
        this.subject = subject;
        this.to = to;
        this.replyTo = replyTo;
    }

    /// <summary>
    /// ID доменного типа, на который зарегистрирована подписка
    /// </summary>
    public virtual Guid? ContextObjectId
    {
        get { return this.contextObjectId; }
    }

    /// <summary>
    /// Доменный тип, на который зарегистрирована подписка
    /// </summary>
    public virtual string ContextObjectType
    {
        get { return this.contextObjectType; }
    }

    /// <summary>
    /// Дополнительный получатель в копии письма
    /// </summary>
    [MaxLength]
    public virtual string Copy
    {
        get { return this.copy; }
    }

    /// <summary>
    /// Получатель(и) нотификации
    /// </summary>
    [MaxLength]
    public virtual string To
    {
        get { return this.to; }
    }

    /// <summary>
    /// Шаблон темы нотификации
    /// </summary>
    [MaxLength(1000)]
    public virtual string Subject
    {
        get { return this.subject; }
    }

    /// <summary>
    /// Шаблон текста нотифкации
    /// </summary>
    [MaxLength]
    public virtual string Message
    {
        get { return this.message; }
    }

    /// <summary>
    /// Название шаблона нотификации
    /// </summary>
    public virtual string TemplateName
    {
        get { return this.templateName; }
    }

    /// <summary>
    /// Комментарии
    /// </summary>
    public virtual string Comment
    {
        get { return this.comment; }
    }

    /// <summary>
    /// Отправитель нотификации
    /// </summary>
    public virtual string From
    {
        get { return this.from; }
    }

    /// <summary>
    /// ReplyTo нотификации
    /// </summary>
    [MaxLength]
    public virtual string ReplyTo
    {
        get { return this.replyTo; }
    }

}
