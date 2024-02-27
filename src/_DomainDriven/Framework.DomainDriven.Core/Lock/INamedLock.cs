namespace Framework.DomainDriven.Lock;

/// <summary>
/// Операция для объекта, на котором можно сделать пессимистическую блокировку
/// </summary>
public record NamedLock(Type DomainType);
