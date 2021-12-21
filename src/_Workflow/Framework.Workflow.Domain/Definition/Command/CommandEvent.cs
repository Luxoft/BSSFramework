using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Событие выполнения команды
    /// </summary>
    public class CommandEvent : Event<Command>
    {
        protected CommandEvent()
        {

        }

        /// <summary>
        /// Конструктор создает событие выполнения команды с ссылкой на команду
        /// </summary>
        /// <param name="command">Команда</param>
        public CommandEvent(Command command)
            : this (command, false)
        {

        }

        /// <summary>
        /// Конструктор создает CommandEvent с ссылкой на команду и пересобирает имя команды
        /// </summary>
        /// <param name="command">Команда</param>
        /// <param name="autoRecalName">Признак того, что имя должно быть пересобрано</param>
        public CommandEvent(Command command, bool autoRecalName)
            : base(command)
        {
            command.AddDetail(this);

            if (autoRecalName)
            {
                this.RecalcName();
            }
        }

        /// <summary>
        /// Название события
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public override string Name
        {
            get { return base.Name; }
        }

        /// <summary>
        /// Вычисляемое через элемент воркфлоу состояние команды
        /// </summary>
        [ExpandPath("Owner.State")]
        public override StateBase SourceState
        {
            get { return this.Owner.State; }
        }
    }
}