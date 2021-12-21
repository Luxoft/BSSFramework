using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Базовое состояние воркфлоу-процесса
    /// </summary>
    /// <remarks>
    /// Класса "State Base" объединяет базовые объекты, которые есть у состояний:
    /// 1) <see cref="State"/>
    /// 2) <see cref="ParallelState"/>
    /// 3) <see cref="ConditionState"/>
    /// Запросы по состояниям в SQL обрабатываются через 2 таблицы: State Base и State/Parallel State/Condition State
    /// </remarks>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
    public partial class StateBase : WorkflowItemBase, IDetail<Workflow>, IWorkflowElement
    {
        private readonly Workflow workflow;

        private readonly StateType type;

        private string autoSetStatePropertyName;

        private string autoSetStatePropertyValue;

        private bool isInitial;


        protected StateBase()
        {

        }

        /// <summary>
        /// Конструктор создает базовое состояние с ссылкой на воркфлоу и на тип состояний
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        /// <param name="type">Тип состояний</param>
        protected StateBase(Workflow workflow, StateType type)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.type = type;
        }

        /// <summary>
        /// Ворклоу, который содержит состояние
        /// </summary>
        public virtual Workflow Workflow
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// Тип состояния
        /// </summary>
        public virtual StateType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Признак того, что состояние является начальным для воркфлоу
        /// </summary>
        public virtual bool IsInitial
        {
            get { return this.isInitial; }
            set { this.isInitial = value; }
        }

        /// <summary>
        /// Название свойства доменного объекта, которому необходимо присвоить значение при переходе в это состояние
        /// </summary>
        /// <remarks>
        /// В пользовательском интерфейсе по умолчанию не отражается состояние доменного объекта
        /// Если требуется указать статус в гриде, то необходимо завести свой статус, не связанный с состоянием воркфлоу
        /// При переходе в конкретное состояние берется определенное свойство объекта и присваивается ему какое-нибудь значение
        /// Например, строковому свойству статус присваивается значение «In Progress»
        /// </remarks>
        public virtual string AutoSetStatePropertyName
        {
            get { return this.autoSetStatePropertyName.TrimNull(); }
            set { this.autoSetStatePropertyName = value.TrimNull(); }
        }

        /// <summary>
        /// Значение свойства <see cref="AutoSetStatePropertyName"/> доменного объекта, присваиваемое при переходе в это состояние
        /// </summary>
        public virtual string AutoSetStatePropertyValue
        {
            get { return this.autoSetStatePropertyValue.TrimNull(); }
            set { this.autoSetStatePropertyValue = value.TrimNull(); }
        }

        #region IDetail<Workflow> Members

        Workflow IDetail<Workflow>.Master
        {
            get { return this.Workflow; }
        }

        #endregion
    }
}