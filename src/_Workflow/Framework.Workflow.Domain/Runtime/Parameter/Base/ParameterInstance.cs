using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Экземпляр параметра
    /// </summary>
    /// <typeparam name="TParameter">Тип параметра</typeparam>
    public abstract class ParameterInstance<TParameter> : AuditPersistentDomainObjectBase,

        IParameterInstance<TParameter>

        where TParameter : Definition.Parameter
    {
        private TParameter definition;

        private string value;


        protected ParameterInstance()
        {
        }

        /// <summary>
        /// Экземпляр воркфлоу
        /// </summary>
        public abstract WorkflowInstance WorkflowInstance { get; }

        /// <summary>
        /// Тип параметра
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual TParameter Definition
        {
            get { return this.definition; }
            set { this.SetValueSafe(v => v.definition, value); }
        }

        /// <summary>
        /// Вычисляемый через definition доменный тип
        /// </summary>
        [ExpandPath("Definition.Type")]
        public virtual Definition.DomainType Type
        {
            get { return this.Definition.Type; }
        }

        /// <summary>
        /// Значение параметра
        /// </summary>
        [MaxLength]
        public virtual string Value
        {
            get { return this.value; }
            set { this.SetValueSafe(v => v.value, value); }
        }
    }
}