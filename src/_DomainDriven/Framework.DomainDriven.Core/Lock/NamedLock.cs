namespace Framework.DomainDriven.Lock;

/// <summary>
/// Операция для объекта, на котором можно сделать пессимистическую блокировку
/// </summary>
public record NamedLock(string Name, Type DomainType)
{
    public NamedLock(string Name)
        : this(Name, typeof(object))
    {
    }
}
