namespace Framework.Persistent
{
    public interface IDomainTypeElement<out TDomainType>
    {
        TDomainType DomainType { get; }
    }
}