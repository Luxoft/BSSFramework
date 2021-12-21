using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public interface IParameterizedRequest<out TDefinition, out TParameter> : IDefinitionDomainObject<TDefinition>, IParametersContainer<TParameter>
    {

    }
}