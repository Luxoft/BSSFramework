using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Сообщение об ошибке
/// </summary>
/// <remarks>
/// Механизм Framework-а сохраняет все ошибки по всем системам, записывает их в базу и высылает на почту
/// </remarks>
[BLLViewRole, BLLSaveRole(CustomImplementation = true)]
[ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.ExceptionMessageView)]
[NotAuditedClass]
public class ExceptionMessage : AuditPersistentDomainObjectBase
{
    private ExceptionMessage innerException;

    private string messageType;
    private string message;
    private string stackTrace;

    private bool isRoot;
    private bool isClient;

    #region Constructor

    public ExceptionMessage()
    {

    }

    public ExceptionMessage(ExceptionMessage ownerExceptionMessage)
    {

    }

    #endregion
    /// <summary>
    /// Внутрення ошибка нижнего уровня
    /// </summary>
    [DetailRole(true)]
    public virtual ExceptionMessage InnerException
    {
        get { return this.innerException; }
        set { this.innerException = value; }
    }

    /// <summary>
    /// Признак корневой ошибки
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual bool IsRoot
    {
        get { return this.isRoot; }
        set { this.isRoot = value; }
    }

    /// <summary>
    /// Признак возникновения ошибки на клиентском фреймворке (Silverlight)
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual bool IsClient
    {
        get { return this.isClient; }
        set { this.isClient = value; }
    }

    /// <summary>
    /// Тип ошибки
    /// </summary>
    public virtual string MessageType
    {
        get { return this.messageType.TrimNull(); }
        set { this.messageType = value.TrimNull(); }
    }

    /// <summary>
    /// Текст сообщения об ошибке
    /// </summary>
    [MaxLength]
    public virtual string Message
    {
        get { return this.message.TrimNull(); }
        set { this.message = value.TrimNull(); }
    }

    /// <summary>
    /// Полный StackTrace ошибки
    /// </summary>
    [MaxLength]
    public virtual string StackTrace
    {
        get { return this.stackTrace.TrimNull(); }
        set { this.stackTrace = value.TrimNull(); }
    }
}
