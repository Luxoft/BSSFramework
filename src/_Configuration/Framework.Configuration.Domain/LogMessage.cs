using Framework.DomainDriven.Attributes;
using Framework.Persistent;
using Framework.Core;
using Framework.Restriction;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    [NotAuditedClass]

    /// <summary>
    /// Сообщение об обращении пользователя через фасад
    /// </summary>
    /// <remarks>
    /// Логируется каждое обращение пользователя
    /// </remarks>
    public class LogMessage : AuditPersistentDomainObjectBase
    {
        private Period period;

        private string action;
        private string userName;
        private string inputMessage;
        private string outputMessage;

        /// <summary>
        /// Период обработки запроса
        /// </summary>
        public virtual Period Period
        {
            get { return this.period; }
            set { this.period = value; }
        }

        /// <summary>
        /// Фасадный метод
        /// </summary>
        [MaxLength(512)]
        public virtual string Action
        {
            get { return this.action.TrimNull(); }
            set { this.action = value.TrimNull(); }
        }

        /// <summary>
        /// Имя пользователя, который вызвал фасадный метод
        /// </summary>
        [MaxLength(512)]
        public virtual string UserName
        {
            get { return this.userName.TrimNull(); }
            set { this.userName = value.TrimNull(); }
        }

        /// <summary>
        /// Входные параметры метода
        /// </summary>
        [MaxLength]
        public virtual string InputMessage
        {
            get { return this.inputMessage.TrimNull(); }
            set { this.inputMessage = value.TrimNull(); }
        }

        /// <summary>
        /// Выходные параметры метода
        /// </summary>
        [MaxLength]
        public virtual string OutputMessage
        {
            get { return this.outputMessage.TrimNull(); }
            set { this.outputMessage = value.TrimNull(); }
        }
    }
}