namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Интерфейс для доменного объекта, у которого есть definition
    /// </summary>
    public interface IDefinitionDomainObject<out TDomainObject>
    {
        TDomainObject Definition { get; }
    }
}