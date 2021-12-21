using System;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Экземпляр параметра выполненной команды
    /// </summary>
    public class ExecutedCommandParameter : ParameterInstance<Definition.CommandParameter>, IDetail<ExecutedCommand>
    {
        private readonly ExecutedCommand command;

       /// <summary>
       /// Конструктор создает параметр выполненной команды с ссылкой на команду
       /// </summary>
       /// <param name="command">Экземпляр команды</param>
        public ExecutedCommandParameter(ExecutedCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            this.command = command;
            this.command.AddDetail(this);
        }

        protected ExecutedCommandParameter()
        {
        }

        /// <summary>
        /// Экземпляр команды
        /// </summary>
        public virtual ExecutedCommand Command
        {
            get { return this.command; }
        }

        #region IDetail<ExecutedCommand> Members

        ExecutedCommand IDetail<ExecutedCommand>.Master
        {
            get { return this.Command; }
        }

        #endregion

        #region IWorkflowInstanceElement Members

        [ExpandPath("Command.Task.State.Workflow")]
        public override WorkflowInstance WorkflowInstance
        {
            get { return this.Command.Task.State.Workflow; }
        }

        #endregion
    }
}