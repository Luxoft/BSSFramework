using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.Persistent;
using Framework.Restriction;

using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.Persistent;
using Framework.Core;
using Framework.Restriction;

namespace Framework.Configuration.Domain
{

    /// <summary>
    /// Шаблон сообщения
    /// </summary>
    /// <remarks>
    /// Для каждой подписки задаётся шаблон письма, отправляемого пользователю в нотификации
    /// Отосланные письма логируются в таблице SentMessage
    /// </remarks>
    [DomainType("0376E081-21CC-47B3-BF36-A3B700FF8487")]
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.MessageTemplateView)]
    [ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.MessageTemplateEdit)]
    [UniqueGroup]
    [NotAuditedClass]
    public class MessageTemplate : AuditPersistentDomainObjectBase,
        IMessageTemplate,
        IDomainTypeElement<DomainType>,
        ITargetSystemElement<TargetSystem>
    {
        private DomainType domainType;


        private string message;

        private string subject;

        private string code;

        private string comment;

        private TemplateRenderer renderer;

        public MessageTemplate()
        {
        }

        /// <summary>
        /// Доменный тип, к оторому относится шаблон сообщения
        /// </summary>
        public virtual DomainType DomainType
        {
            get { return this.domainType; }
            set { this.domainType = value; }
        }

        /// <summary>
        /// Виртуальное свойство на целевую систему, в рамках которой отправляются нотификации пользователю
        /// </summary>
        [ExpandPath("DomainType.TargetSystem")]
        public virtual TargetSystem TargetSystem
        {
            get { return this.DomainType.Maybe(v => v.TargetSystem); }
        }

        /// <summary>
        /// Шаблон тела сообщения, отправляемого пользователю в нотификации
        /// </summary>
        [MaxLength]
        public virtual string Message
        {
            get { return this.message.TrimNull(); }
            set { this.message = value.TrimNull(); }
        }

        /// <summary>
        /// Шаблон темы сообщения
        /// </summary>
        [MaxLength(500)]
        public virtual string Subject
        {
            get { return this.subject.TrimNull(); }
            set { this.subject = value.TrimNull(); }
        }

        /// <summary>
        /// Уникальный код шаблона
        /// </summary>
        [Required]
        [UniqueElement]
        [VisualIdentity]
        public virtual string Code
        {
            get { return this.code.TrimNull(); }
            set { this.code = value.TrimNull(); }
        }

        /// <summary>
        /// Комментарии
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment.TrimNull(); }
            set { this.comment = value.TrimNull(); }
        }

        /// <summary>
        /// Способ рендеринга шаблона
        /// </summary>
        public virtual TemplateRenderer Renderer
        {
            get { return this.renderer; }
            set { this.renderer = value; }
        }

        public override string ToString()
        {
            return this.Code;
        }
    }

    /// <summary>
    /// Перечисление способов рендеринга
    /// </summary>
    public enum TemplateRenderer
        {
        Spring,

        Razor
    }
}