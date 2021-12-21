using System;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Параметр команды
    /// </summary>
    /// <remarks>
    /// Параметр команды записывается в Execute Command и автоматически вычисляется на сервере
    /// </remarks>
    public partial class CommandParameter : Parameter, IDetail<Command>
    {
        private readonly Command command;

        private bool isReadOnly;

        /// <summary>
        /// Конструктор создает параметр команды с ссылкой на команду
        /// </summary>
        /// <param name="command">Команда</param>
        public CommandParameter (Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            this.command = command;
            this.command.AddDetail(this);
        }

        protected CommandParameter()
        {
        }

        /// <summary>
        /// Команда параметра
        /// </summary>
        public virtual Command Command
        {
            get { return this.command; }
        }

        /// <summary>
        /// Признак для параметров, которые не нужно заполнять при выполнении команды
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return this.isReadOnly; }
            protected internal set { this.isReadOnly = value; }
        }

        /// <summary>
        /// Вычисляемый через команду воркфлоу, к которому относится параметр
        /// </summary>
        [ExpandPath("Command.Workflow")]
        public override Workflow Workflow
        {
            get { return this.Command.Workflow; }
        }


        #region IDetail<Command> Members

        Command IDetail<Command>.Master
        {
            get { return this.Command; }
        }

        #endregion


        //public const string PotentialApproversParameterName = "PotentialApprovers";

        //public const string PotentialApproversParameterDescription = "Potential Command Approvers";
    }
}