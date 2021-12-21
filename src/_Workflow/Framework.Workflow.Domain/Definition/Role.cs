using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Transfering;
using Framework.Validation;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Элемент безопасности, определяющий доступность команды пользователю
    /// </summary>
    /// <remarks>
    /// Роль определяется либо с помощью операции, либо с помощью custom security provider
    /// SecurityProvider<,> по целевой системе и доменному объекту из роли вычисляется двуми способами:
    /// Выставление непосредственно идентификатора авторизационной операции в свойство "SecurityOperationId"
    /// Выдача кастомного провайдера через лямбду "CustomSecurityProvider"
    /// </remarks>
    [WorkflowViewDomainObject]
    [BLLViewRole(MaxCollection = MainDTOType.RichDTO)]
    [RequiredGroupValidator(RequiredGroupValidatorMode.One, OperationContext = WorkflowOperationContextC.Start, GroupKey = "Security")]
    public class Role : WorkflowItemBase, IDetail<Workflow>, IWorkflowElement
    {
        private readonly Workflow workflow;

        private Guid securityOperationId;

        private WorkflowLambda customSecurityProvider;


        protected Role()
        {

        }

        /// <summary>
        /// Конструктор создает роль воркфлоу с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public Role(Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.workflow.AddDetail(this);
        }

        /// <summary>
        /// Воркфлоу, к которому относится роль
        /// </summary>
        public virtual Workflow Workflow
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// Идентификатор секьюрной операции
        /// </summary>
        [UniqueElement("Security")]
        public virtual Guid SecurityOperationId
        {
            get { return this.securityOperationId; }
            set { this.securityOperationId = value; }
        }

        /// <summary>
        /// Элемент доступа
        /// </summary>
        /// <remarks>
        /// Лямбда создается, когда невозможно выразить безопасность через «Security Operation»
        /// Пример authContext => authContext.GetOperationSecurityProvider()
        /// </remarks>
        [UniqueElement("Security")]
        [WorkflowElementValidator]
        public virtual WorkflowLambda CustomSecurityProvider
        {
            get { return this.customSecurityProvider; }
            set { this.customSecurityProvider = value; }
        }


        #region IDetail<Workflow> Members

        Workflow IDetail<Workflow>.Master
        {
            get { return this.Workflow; }
        }

        #endregion
    }
}
