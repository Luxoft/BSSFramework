using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Exceptions;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Runtime
{
    /// <summary>
    /// Экземпляр, созданный на основе доменного типа для всего жизненного цикла воркфлоу
    /// </summary>
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole(CountType = CountType.Both)]
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    public partial class WorkflowInstance : WorkflowItemBase,
        IMaster<WorkflowInstanceParameter>,
        IMaster<StateInstance>,
        IMaster<TransitionInstance>,
        IMaster<WorkflowInstanceWatcher>,

        IParametersContainer<WorkflowInstanceParameter>,
        IDefinitionDomainObject<Definition.Workflow>,

        IDefaultHierarchicalPersistentDomainObjectBase<WorkflowInstance>,

        IDetail<StateInstance>,

        IRestrictedWofkflowInstance,

        IPrincipalsContainer<WorkflowInstanceWatcher>
    {
        private readonly ICollection<WorkflowInstanceParameter> parameters = new List<WorkflowInstanceParameter>();

        private readonly ICollection<WorkflowInstanceWatcher> watchers = new List<WorkflowInstanceWatcher>();

        private readonly ICollection<StateInstance> states = new List<StateInstance>();

        private readonly ICollection<TransitionInstance> transitions = new List<TransitionInstance>();


        private readonly StateInstance ownerState;

        private readonly Definition.Workflow definition;


        private StateInstance currentState;

        private bool isFinished;

        private bool isAborted;

        private Guid domainObjectId;


        protected WorkflowInstance()
        {

        }

        /// <summary>
        /// Конструктор создает экземпляр воркфлоу с ссылкой на воркфлоу и экземпляр состояния родительского воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        /// <param name="ownerWorkflowState">Экземпляр состояния родительского воркфлоу</param>
        public WorkflowInstance(Workflow.Domain.Definition.Workflow workflow, StateInstance ownerWorkflowState)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.definition = workflow;

            if (ownerWorkflowState != null)
            {
                var owner = ownerWorkflowState.Workflow;

                if (owner.Maybe(o => o.Definition) != workflow.Owner)
                {
                    throw new BusinessLogicException("Different owner workflow");
                }

                this.ownerState = ownerWorkflowState;
                this.ownerState.AddDetail(this);
            }
        }


        /// <summary>
        /// Коллекция параметров экземпляра воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        [UniqueGroup]
        public virtual IEnumerable<WorkflowInstanceParameter> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Коллекция пользователей, которые имеют права на просмотр объектов, минуя систему прав
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<WorkflowInstanceWatcher> Watchers
        {
            get { return this.watchers; }
        }

        /// <summary>
        /// Экземпляр параметра воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [Required]
        public virtual WorkflowInstanceParameter DomainObjectParameter
        {
            get
            {
                return this.Definition.DomainObjectParameter.Maybe(domainObjectParameter =>

                    this.Parameters.SingleOrDefault(p => p.Definition == domainObjectParameter));
            }
        }

        /// <summary>
        /// Коллекция экземпляров состояний объекта с выполненной задачей
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual IEnumerable<StateInstance> States
        {
            get { return this.states; }
        }

        /// <summary>
        /// Коллекция экземпляров перехода между состояниями
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual IEnumerable<TransitionInstance> Transitions
        {
            get { return this.transitions; }
        }

        /// <summary>
        /// Экземпляр состояния объекта с выполненной задачей
        /// </summary>
        [IsMaster]
        public virtual StateInstance OwnerState
        {
            get { return this.ownerState; }
        }

        /// <summary>
        /// Вычисляемый через экземпляр состояния экземпляр запущенного воркфлоу
        /// </summary>
        [ExpandPath("OwnerState.Workflow")]
        public virtual WorkflowInstance Owner
        {
            get { return this.OwnerState.Maybe(os => os.Workflow); }
        }

        /// <summary>
        /// Воркфлоу, к которому относится экземпляр воркфлоу
        /// </summary>
        [ExpandPath("Owner.Definition")]
        public virtual Definition.Workflow OwnerDefinition
        {
            get { return this.Owner.Maybe(owner => owner.Definition); }
        }

        /// <summary>
        /// ID доменного объекта, на основе которого создан экземпляр воркфлоу
        /// </summary>
        [Required]
        public virtual Guid DomainObjectId
        {
            get { return this.domainObjectId; }
            internal protected set { this.domainObjectId = value; }
        }

        /// <summary>
        /// Название экземпляра воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        [MaxLength]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Описание экземпляра воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        [MaxLength]
        public override string Description
        {
            get { return base.Description; }
            set { base.Description = value; }
        }

        /// <summary>
        /// Definition воркфлоу
        /// </summary>
        public virtual Definition.Workflow Definition
        {
            get { return this.definition; }
        }

        /// <summary>
        /// Текущее состояние экземпляра воркфлоу
        /// </summary>
        [Required]
        public virtual StateInstance CurrentState
        {
            get { return this.currentState; }
            internal protected set { this.currentState = value; }
        }

        /// <summary>
        /// Definition текущего состояния экземпляра воркфлоу
        /// </summary>
        [ExpandPath("CurrentState.Definition")]
        public virtual Definition.StateBase CurrentStateDefinition
        {
            get { return this.CurrentState.Definition; }
        }

        //[CustomSerialization(CustomSerializationMode.Ignore)]
        //public virtual bool OwnerStateIsAvailable
        //{
        //    get
        //    {
        //        return this.Owner.Maybe(o => o.CurrentState.Definition == this.Definition.State)
        //    }
        //}

        /// <summary>
        /// Возвращает один из параметров заданного вида
        /// </summary>
        /// <param name="role">Роль</param>
        /// <returns></returns>
        public virtual WorkflowInstanceParameter GetParameter(Definition.WorkflowParameterRole role)
        {
            return this.Parameters.SingleOrDefault(parameter => parameter.Definition.Role == role);
        }

        #region IMaster<WorkflowInstanceParameter> Members

        ICollection<WorkflowInstanceParameter> IMaster<WorkflowInstanceParameter>.Details
        {
            get { return (ICollection<WorkflowInstanceParameter>)this.Parameters; }
        }

        #endregion

        #region IMaster<WorkflowInstanceWatcher> Members

        ICollection<WorkflowInstanceWatcher> IMaster<WorkflowInstanceWatcher>.Details
        {
            get { return (ICollection<WorkflowInstanceWatcher>)this.Watchers; }
        }

        #endregion

        #region IMaster<TaskInstance> Members

        ICollection<StateInstance> IMaster<StateInstance>.Details
        {
            get { return (ICollection<StateInstance>)this.States; }
        }

        #endregion

        #region IMaster<TransitionInstance> Members

        ICollection<TransitionInstance> IMaster<TransitionInstance>.Details
        {
            get { return (ICollection<TransitionInstance>)this.Transitions; }
        }

        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual bool IsAvailable
        {
            get { return this.Active && !this.IsFinished; }
        }

        public virtual bool IsFinished
        {
            get { return this.isFinished; }
            protected internal set { this.isFinished = value; }
        }

        public virtual bool IsAborted
        {
            get { return this.isAborted; }
            protected internal set { this.isAborted = value; }
        }

        #endregion

        /// <summary>
        /// Метод выставляет признак того, что экземпляр воркфлоу закончен или прерван
        /// </summary>
        public virtual void Abort()
        {
            this.IsFinished = true;
            this.IsAborted = true;
        }

        #region IParentSource<WorkflowInstance> Members

        WorkflowInstance IParentSource<WorkflowInstance>.Parent
        {
            get { return this.Owner; }
        }

        #endregion

        #region IChildrenSource<WorkflowInstance> Members

        IEnumerable<WorkflowInstance> IChildrenSource<WorkflowInstance>.Children
        {
            get { return this.CurrentState.SubWorkflows; }
        }

        #endregion

        #region IDetail<StateInstance> Members

        StateInstance IDetail<StateInstance>.Master
        {
            get { return this.OwnerState; }
        }

        #endregion

        #region IRestrictedWofkflowInstance Members

        IRestrictedStateInstance IRestrictedWofkflowInstance.CurrentState
        {
            get { return this.CurrentState; }
        }

        INamedCollection<string> IRestrictedWofkflowInstance.Parameters
        {
            get
            {
                var dict = this.Parameters.ToDictionary(p => p.Definition.Name, p => p.Value);

                return new NamedCollection<string>(dict.Keys, v => dict[v]);
            }
        }

        bool IRestrictedWofkflowInstance.IsFinished
        {
            get { return this.IsFinished; }
        }

        #endregion

        /// <summary>
        /// Имплементация через интерфейс для унификации кода
        /// </summary>
        [ExpandPath("Watchers")]
        IEnumerable<WorkflowInstanceWatcher> IPrincipalsContainer<WorkflowInstanceWatcher>.Principals
        {
            get { return this.Watchers; }
        }
    }
}