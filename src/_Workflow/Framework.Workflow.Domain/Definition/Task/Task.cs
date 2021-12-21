using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

using JetBrains.Annotations;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Задача
    /// </summary>
    /// <remarks>
    /// Задача – это набор возможных команд, которые могут быть выполнены над объектом в конкретном состоянии
    /// </remarks>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
    public partial class Task : WorkflowItemBase,
        IDetail<State>,
        IMaster<Command>,
        IMaster<TaskMetadata>,
        IWorkflowElement,
        IRoleSource
    {
        private readonly ICollection<Command> commands = new List<Command>();

        private readonly ICollection<TaskMetadata> metadatas = new List<TaskMetadata>();

        private readonly State state;

        /// <summary>
        /// Конструктор создает задачу с ссылкой на состояние
        /// </summary>
        /// <param name="state">Состояние</param>
        public Task([NotNull] State state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            this.state = state;
            this.state.AddDetail(this);
        }

        protected Task()
        {
        }

        /// <summary>
        /// Установленное состояние для задачи
        /// </summary>
        public virtual State State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Коллекция команд, которые содержатся в текущей задаче
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<Command> Commands
        {
            get { return this.commands; }
        }

        /// <summary>
        /// Коллекция метаданных задачи
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<TaskMetadata> Metadatas
        {
            get { return this.metadatas; }
        }

        /// <summary>
        /// Вычисляемый воркфлоу, который содержит задачу
        /// </summary>
        [ExpandPath("State.Workflow")]
        public virtual Workflow Workflow
        {
            get { return this.State.Workflow; }
        }

        /// <summary>
        /// Метод возвращает коллекцию ролей, по которым доступна задача
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Role> GetUsingRoles()
        {
            return this.Commands.SelectMany(command => command.GetUsingRoles()).Distinct();
        }

        #region IDetail<State> Members

        State IDetail<State>.Master
        {
            get { return this.State; }
        }

        #endregion

        #region IMaster<Command> Members

        ICollection<Command> IMaster<Command>.Details
        {
            get { return (ICollection<Command>)this.Commands; }
        }

        #endregion

        #region IMaster<TaskMetadata> Members

        ICollection<TaskMetadata> IMaster<TaskMetadata>.Details
        {
            get { return (ICollection<TaskMetadata>)this.Metadatas; }
        }

        #endregion
    }
}
