using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Условие инициализации создания экземпляра воркфлоу
    /// </summary>
    /// <remarks>
    /// Условие, при выполнении которого запускается экземпляр воркфлоу
    /// </remarks>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
    public class StartWorkflowDomainObjectCondition : AuditPersistentDomainObjectBase, IDetail<Workflow>, IWorkflowElement
    {
        private readonly Workflow workflow;

        private WorkflowLambda condition;

        private WorkflowLambda factory;


        protected StartWorkflowDomainObjectCondition()
        {

        }

        /// <summary>
        /// Конструктор создает стартовое условие с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public StartWorkflowDomainObjectCondition(Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.workflow.AddDetail(this);
        }

        /// <summary>
        /// Воркфлоу, который вступает в работу после выполнения инициирующей лямбды
        /// </summary>
        public virtual Workflow Workflow
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// Стартовое условие для инициализации создания экземпляра воркфлоу
        /// </summary>
        [UniqueElement]
        [RequiredStartValidator]
        [WorkflowElementValidator]
        public virtual WorkflowLambda Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }

        /// <summary>
        /// Лямбда "Factory" создает экземпляр через фабричную функцию, когда у воркфлоу более одного параметра
        /// </summary>
        /// <remarks>
        /// В лямбде инициализируются параметры воркфлоу с конкретными значениями
        /// Пример (authContext, wfObj) => authContext.Logics.Permission.GetApproveOperationWorkflowStartupObjects(wfObj.DomainObject)
        /// </remarks>
        [UniqueElement]
        [WorkflowElementValidator]
        public virtual WorkflowLambda Factory
        {
            get { return this.factory; }
            set { this.factory = value; }
        }

        /// <summary>
        /// Признак того, что лямбда активна
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Normal)]
        public override bool Active
        {
            get { return base.Active; }
            set { base.Active = value; }
        }

        #region IDetail<Workflow> Members

        Workflow IDetail<Workflow>.Master
        {
            get { return this.workflow; }
        }

        #endregion
    }
}