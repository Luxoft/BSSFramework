using System;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Параметр воркфлоу
    /// </summary>
    /// <remarks>
    /// После выполнения стартового условия, создается экземпляр воркфлоу, связанный с экземпляром параметра воркфлоу
    /// Как минимум один из параметров должен называться "DomainObject" и иметь тип доменного объекта, который будет считаться как доменный тип самого воркфлоу
    /// </remarks>
    public class WorkflowParameter : Parameter, IDetail<Workflow>
    {
        private readonly Workflow workflow;

        /// <summary>
        /// Конструктор создает параметр воркфлоу с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public WorkflowParameter(Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.workflow.AddDetail(this);
        }

        protected WorkflowParameter()
        {
        }

        /// <summary>
        /// Воркфлоу, который содержит параметр
        /// </summary>
        public override Workflow Workflow
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// Вычиляемое свойство типа параметра
        /// </summary>
        /// <remarks>
        /// Сравнивает имя параметра с одним из заданных имен
        /// </remarks>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual WorkflowParameterRole Role
        {
            get
            {
                if (DomainObjectName.Equals(this.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return WorkflowParameterRole.DomainObject;
                }
                else if (InstanceIdentityName.Equals(this.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return WorkflowParameterRole.InstanceIdentity;
                }
                else if (InstanceDescriptionName.Equals(this.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return WorkflowParameterRole.InstanceDescription;
                }
                else
                {
                    return WorkflowParameterRole.Other;
                }
            }
        }

        #region IDetail<Workflow> Members

        Workflow IDetail<Workflow>.Master
        {
            get { return this.Workflow; }
        }

        #endregion


        public const string DomainObjectName = "DomainObject";

        public const string InstanceIdentityName = "Name";

        public const string InstanceDescriptionName = "Description";

        public const string OwnerWorkflowName = "Owner";
    }
}