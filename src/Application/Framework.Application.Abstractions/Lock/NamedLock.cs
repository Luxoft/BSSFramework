namespace Framework.Application.Lock;

/// <summary>
/// Operation for an object on which a pessimistic lock can be taken
/// </summary>
public record NamedLock(string Name, Type? DomainType = null)
{
    public NamedLock(Type domainType)
        : this(domainType.Name, domainType)
    {
    }
}
