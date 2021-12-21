using System;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Метаданные команды
    /// </summary>
    /// <remarks>
    /// У одного типа команды одинаковый набор матаданных
    /// </remarks>
    public partial class CommandMetadata : ObjectMetadata, IWorkflowElement, IDetail<Command>
    {
        private readonly Command command;


        protected CommandMetadata()
        {
        }

        /// <summary>
        /// Конструктор создает метаданные с ссылкой на команду
        /// </summary>
        /// <param name="command">Команда</param>
        public CommandMetadata(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            this.command = command;
            this.command.AddDetail(this);
        }

        /// <summary>
        /// Команда, для которой созданы метаданные
        /// </summary>
        public virtual Command Command
        {
            get { return this.command; }
        }


        #region IWorkflowElement Members

        Workflow IWorkflowElement.Workflow
        {
            get { return this.Command.Workflow; }
        }

        #endregion

        #region IDetail<Command> Members

        Command IDetail<Command>.Master
        {
            get { return this.Command; }
        }

        #endregion
    }
}