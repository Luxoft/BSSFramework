using System;
using System.Collections.Generic;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Runtime
{
    /// <summary>
    /// Экземпляр задачи, ожидающий выполнения
    /// </summary>
    [BLLViewRole, BLLSaveRole(AllowCreate = false)]
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLRole]
    public partial class TaskInstance : AuditPersistentDomainObjectBase,
        IDetail<StateInstance>,
        IMaster<ExecutedCommand>,
        //IMaster<TaskInstanceAssignee>,
        IDefinitionDomainObject<Definition.Task>
    {
        private readonly StateInstance state;

        private readonly Definition.Task definition;

        private readonly ICollection<ExecutedCommand> commands = new List<ExecutedCommand>();

        //private readonly ICollection<TaskInstanceAssignee> assignees = new List<TaskInstanceAssignee>();

        /// <summary>
        /// Конструктор создает экзмпляр задачи с ссылкой на definition и состояние
        /// </summary>
        /// <param name="state">Экземпляр состояния</param>
        /// <param name="definition">Definition задачи</param>
        public TaskInstance(StateInstance state, Definition.Task definition)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (definition == null) throw new ArgumentNullException(nameof(definition));

            this.state = state;
            this.state.AddDetail(this);

            this.definition = definition;
        }

        protected TaskInstance()
        {
        }

        /// <summary>
        /// Коллекция экземпляров выполненной команды
        /// </summary>
        [UniqueGroup]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual IEnumerable<ExecutedCommand> Commands
        {
            get { return this.commands; }
        }


        //[UniqueGroup]
        //public virtual IEnumerable<TaskInstanceAssignee> Assignees
        //{
        //    get { return this.assignees; }
        //}

        /// <summary>
        /// Состояние объекта с выполненной задачей
        /// </summary>
        public virtual StateInstance State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Вычисляемый definition состояния, к которому относится задача
        /// </summary>
        [ExpandPath("State.Definition")]
        public virtual Definition.StateBase StateDefinition
        {
            get { return this.State.Definition; }
        }

        /// <summary>
        /// Definition задачи
        /// </summary>
        [UniqueElement]
        public virtual Definition.Task Definition
        {
            get { return this.definition; }
        }

        /// <summary>
        /// Вычисляемый экземпляр воркфлоу через состояние
        /// </summary>
        [ExpandPath("State.Workflow")]
        public virtual WorkflowInstance Workflow
        {
            get { return this.State.Workflow; }
        }

        /// <summary>
        /// Вычисляемый через состояние признак того, что состояние задачи может быть изменено
        /// </summary>
        [FetchPath("State.Workflow.CurrentState")]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual bool IsAvailable
        {
            get { return this.State.IsAvailable; }
        }

        /// <summary>
        /// Вычисляемый через состояние признак текущей задачи
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual bool IsCurrent
        {
            get { return this.State.IsCurrent; }
        }

        #region IDetail<WorkflowInstance> Members

        StateInstance IDetail<StateInstance>.Master
        {
            get { return this.State; }
        }

        #endregion

        #region IMaster<ExecutedCommand> Members

        ICollection<ExecutedCommand> IMaster<ExecutedCommand>.Details
        {
            get { return (ICollection<ExecutedCommand>)this.Commands; }
        }

        #endregion

        //#region IMaster<TaskInstanceAssignee> Members

        //ICollection<TaskInstanceAssignee> IMaster<TaskInstanceAssignee>.Details
        //{
        //    get { return (ICollection<TaskInstanceAssignee>)this.Assignees; }
        //}

        //#endregion
    }
}
