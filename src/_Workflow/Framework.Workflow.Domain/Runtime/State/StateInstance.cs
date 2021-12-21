using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Экземпляр состояния объекта с выполненной задачей, содержащей выполненную команду и ее параметр
    /// </summary>
    /// <remarks>
    /// При переходе воркфлоу в новый состояние с задачей создается экземпляр здачи и экземпляр состояния
    /// </remarks>
    [BLLViewRole]
    [WorkflowViewDomainObject]
    public class StateInstance : AuditPersistentDomainObjectBase, IMaster<TaskInstance>,

        IDetail<WorkflowInstance>,
        IDefinitionDomainObject<Definition.StateBase>,
        IMaster<WorkflowInstance>,

        IRestrictedStateInstance//,
        //IPrincipalsContainer<TaskInstanceAssignee>
    {
        private readonly WorkflowInstance workflow;
        private readonly Definition.StateBase definition;

        private readonly ICollection<TaskInstance> tasks = new List<TaskInstance>();

        private readonly ICollection<WorkflowInstance> subWorkflows = new List<WorkflowInstance>();

        /// <summary>
        /// Конструктор создает состояние объекта с выполненной задачей и с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Экземпляр воркфлоу</param>
        /// <param name="definition">Definition состояние</param>
        public StateInstance(WorkflowInstance workflow, Definition.StateBase definition)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));
            if (definition == null) throw new ArgumentNullException(nameof(definition));

            this.workflow = workflow;
            this.definition = definition;

            this.workflow.AddDetail(this);
        }

        protected StateInstance()
        {
        }

        /// <summary>
        /// Экземпляр воркфлоу, к которому относится экземпляр состояния
        /// </summary>
        public virtual WorkflowInstance Workflow
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// Коллекция экземпляров задач, которые относятся к экземпляру состояния
        /// </summary>
        [UniqueGroup]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual IEnumerable<TaskInstance> Tasks
        {
            get { return this.tasks; }
        }

        //[UniqueGroup]
        /// <summary>
        /// Коллекция элементов дочерних воркфлоу
        /// </summary>
        [Framework.Validation.UniqueCollectionValidator]
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual IEnumerable<WorkflowInstance> SubWorkflows
        {
            get { return this.subWorkflows; }
        }

        /// <summary>
        /// Definition состояния
        /// </summary>
        public virtual Definition.StateBase Definition
        {
            get { return this.definition; }
        }

        /// <summary>
        /// Вычисляемый признак того, что состояние задачи может быть изменено
        /// </summary>
        [FetchPath("Workflow.CurrentState")]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual bool IsAvailable
        {
            get { return this.Workflow.IsAvailable && this.IsCurrent; }
        }

        /// <summary>
        /// Вычисляемый через воркфлоу признак текущего состояния
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual bool IsCurrent
        {
            get { return this.Workflow.CurrentState == this; }
        }

        #region IMaster<TransitionInstance> Members

        ICollection<TaskInstance> IMaster<TaskInstance>.Details
        {
            get { return this.tasks; }
        }

        #endregion

        #region IDetail<StateInstance> Members

        WorkflowInstance IDetail<WorkflowInstance>.Master
        {
            get { return this.Workflow; }
        }

        #endregion

        #region IMaster<WorkflowInstance> Members

        ICollection<WorkflowInstance> IMaster<WorkflowInstance>.Details
        {
            get { return (ICollection<WorkflowInstance>)this.SubWorkflows; }
        }

        #endregion

        #region IStateInstance Members

        string IRestrictedStateInstance.Name
        {
        	get { return this.Definition.Name; }
        }

        #endregion

        #region IStateInstance Members


        IRestrictedWofkflowInstanceCollection<IRestrictedWofkflowInstance> IRestrictedStateInstance.SubWorkflows
        {
            get { return new RestrictedWofkflowInstanceCollection(this.SubWorkflows); }
        }

        #endregion


        private class RestrictedWofkflowInstanceCollection : NamedCollection<WorkflowInstance>, IRestrictedWofkflowInstanceCollection<WorkflowInstance>
        {
            public RestrictedWofkflowInstanceCollection(IEnumerable<WorkflowInstance> list)
                : base(list, wfInstance => wfInstance.Name)
            {

            }

            public bool AnyAborted
            {
                get { return this.Any(sw => sw.IsAborted); }
            }

            public bool AllStates(string stateName)
            {
                return this.All(rw => this.NameComparer.Equals(rw.CurrentStateDefinition.Name, stateName));
            }

            public bool AnyStates(string stateName)
            {
                return this.Any(rw => this.NameComparer.Equals(rw.CurrentStateDefinition.Name, stateName));
            }
        }

        //IEnumerable<TaskInstanceAssignee> IPrincipalsContainer<TaskInstanceAssignee>.Principals
        //{
        //    get { return this.Tasks.SelectMany(task => task.Assignees).Distinct(v => v.Login); }
        //}
    }
}