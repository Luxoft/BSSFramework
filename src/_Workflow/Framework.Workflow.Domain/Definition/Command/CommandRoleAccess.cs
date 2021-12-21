using System;

using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Связь между ролью и командой
    /// </summary>
    public class CommandRoleAccess : AuditPersistentDomainObjectBase, IDetail<Command>, IWorkflowElement
    {
        private readonly Command command;

        private Role role;


        protected CommandRoleAccess()
        {

        }

        /// <summary>
        /// Конструктор создает связь между ролью и командой
        /// </summary>
        /// <param name="command">Команда</param>
        public CommandRoleAccess(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            this.command = command;
            this.command.AddDetail(this);
        }


        /// <summary>
        /// Команда, которая входит в роль
        /// </summary>
        public virtual Command Command
        {
            get { return this.command; }
        }

        /// <summary>
        /// Роль, в которую входит команда
        /// </summary>
        [WorkflowElementValidator]
        [Required]
        [UniqueElement]
        public virtual Role Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        #region IDetail<Command> Members

        Command IDetail<Command>.Master
        {
            get { return this.Command; }
        }

        #endregion

        #region IWorkflowElement Members

        Workflow IWorkflowElement.Workflow
        {
            get { return this.Command.Workflow; }
        }

        #endregion
    }
}