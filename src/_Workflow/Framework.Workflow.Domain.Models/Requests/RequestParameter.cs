using Framework.Restriction;

using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public abstract class RequestParameter<TParameter> : DomainObjectBase, IParameterInstanceBase<TParameter>
        where TParameter : Parameter
    {
        [Required]
        [UniqueElement]
        public TParameter Definition
        {
            get; set;
        }

        [MaxLength]
        public string Value
        {
            get; set;
        }
    }
}