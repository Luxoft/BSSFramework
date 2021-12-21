using Framework.Persistent;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Интерфейс для экземпляров параметра
    /// </summary>
    /// <typeparam name="TParameter">Тип параметра</typeparam>
    public interface IParameterInstanceBase<out TParameter> : IDefinitionDomainObject<TParameter>, IValueObject<string>
        where TParameter : Definition.Parameter
    {

    }

    /// <summary>
    /// Интерфейс для экземпляров параметра
    /// </summary>
    /// <typeparam name="TParameter">Тип параметра</typeparam>
    public interface IParameterInstance<out TParameter> : IParameterInstanceBase<TParameter>, IWorkflowInstanceElement
        where TParameter : Definition.Parameter
    {

    }
}