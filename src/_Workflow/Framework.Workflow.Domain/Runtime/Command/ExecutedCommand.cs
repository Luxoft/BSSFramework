using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Runtime
{
    /// <summary>
    /// Экземпляр выполненной команды
    /// </summary>
    [WorkflowViewDomainObject]
    [BLLRole]
    public class ExecutedCommand : AuditPersistentDomainObjectBase,
        IMaster<ExecutedCommandParameter>,
        IDetail<TaskInstance>,

        IParametersContainer<ExecutedCommandParameter>,
        IDefinitionDomainObject<Definition.Command>
    {
        private readonly ICollection<ExecutedCommandParameter> parameters = new List<ExecutedCommandParameter>();


        private readonly TaskInstance task;

        private Definition.Command definition;


        protected ExecutedCommand()
        {

        }

        /// <summary>
        /// Конструктор создает экземпляр выполненной команды с ссылкой на задачу
        /// </summary>
        /// <param name="task">Задача</param>
        public ExecutedCommand(TaskInstance task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            this.task = task;
            this.task.AddDetail(this);
        }

        /// <summary>
        /// Экземпляр задачи
        /// </summary>
        public virtual TaskInstance Task
        {
            get { return this.task; }
        }

        /// <summary>
        /// Коллекция зкземпляров параметров выполненной команды
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        [UniqueGroup]
        public virtual IEnumerable<ExecutedCommandParameter> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Definition команды
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual Definition.Command Definition
        {
            get { return this.definition; }
            set { this.SetValueSafe(v => v.definition, value); }
        }


        #region IMaster<ExecutedCommandParameter> Members

        ICollection<ExecutedCommandParameter> IMaster<ExecutedCommandParameter>.Details
        {
            get { return this.parameters; }
        }

        #endregion

        #region IDetail<TaskInstance> Members

        TaskInstance IDetail<TaskInstance>.Master
        {
            get {return this.task; }
        }

        #endregion
    }
}
