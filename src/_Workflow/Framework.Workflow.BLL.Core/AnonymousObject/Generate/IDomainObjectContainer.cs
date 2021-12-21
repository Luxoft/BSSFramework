namespace Framework.Workflow.BLL
{
    public interface IDomainObjectContainer<out TDomainObject>
    {
        TDomainObject DomainObject { get; }
    }
}