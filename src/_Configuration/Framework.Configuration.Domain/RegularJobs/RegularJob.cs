using System;

using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Задача, которую необходимо выполнять на регулярной основе
    /// </summary>
    /// <remarks>
    /// Ожидает вызова от BizTalk
    /// </remarks>
    [UniqueGroup]
    [ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.RegularJobView)]
    [ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.RegularJobEdit)]
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [DomainType("F9BB09D3-C238-4F0D-B325-A5B40147AF63")]
    public class RegularJob : BaseDirectory, ILambdaObject, IVersionObject<long>
    {
        private ShedulerTime shedulerTime;

        private string function;

        private ExecuteRegularJobResult executionResult = new ExecuteRegularJobResult();

        private DateTime? lastPulseTime;

        private DateTime expectedNextStartTime;

        private bool wrapUpSession;

        private RegularJobState state;

        private long version;

        /// <summary>
        /// Лямбда-выражение, описывающее функцию, которую необходимо выполнить
        /// </summary>
        /// <remarks>
        /// Если WrapUpSession = true, то в качестве входного параметра ожидается экземпляр типа IBLLContext =>{ }
        /// В случае wrapUpSesssion = false, в качестве входного параметра ожидается экземпляр типа Environment =>{ }
        /// </remarks>
        [MaxLength]
        public virtual string Function
        {
            get { return this.function; }
            set { this.function = value; }
        }

        /// <summary>
        /// Результат выполненной регулярной задачи
        /// </summary>
        public virtual ExecuteRegularJobResult ExecutionResult
        {
            get { return this.executionResult ?? (this.executionResult = ExecuteRegularJobResult.CreateInitial()); }
            internal protected set { this.executionResult = value; }
        }

        [NotAuditedProperty]
        /// <summary>
        /// Дата последнего оповещения регулярной задачи о смене времени
        /// </summary>
        public virtual DateTime? LastPulseTime
        {
            get { return this.lastPulseTime; }
            protected internal set { this.lastPulseTime = value; }
        }

        /// <summary>
        /// Дата следующего запуска регулярной задачи
        /// </summary>
        public virtual DateTime ExpectedNextStartTime
        {
            get { return this.expectedNextStartTime; }
            set { this.expectedNextStartTime = value; }
        }

        /// <summary>
        /// Расписание запуска регулярной задачи
        /// </summary>
        public virtual ShedulerTime ShedulerTime
        {
            get { return this.shedulerTime ?? (this.shedulerTime = new ShedulerTime()); }
            set { this.shedulerTime = value; }
        }

        /// <summary>
        /// Признак того, что регулярную задачу нужно выполнять обернутую в сессию
        /// </summary>
        public virtual bool WrapUpSession
        {
            get { return this.wrapUpSession; }
            set { this.wrapUpSession = value; }
        }

        /// <summary>
        /// Состояние регулярной задачи
        /// </summary>
        public virtual RegularJobState State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        string IValueObject<string>.Value
        {
            get { return this.Function; }
        }

        [Version]
        public virtual long Version
        {
            get => this.version;
            set => this.version = value;
        }

        [CustomSerialization(CustomSerializationMode.Normal)]
        public override bool Active
        {
            get => base.Active;
            set => base.Active = value;
        }
    }
}
