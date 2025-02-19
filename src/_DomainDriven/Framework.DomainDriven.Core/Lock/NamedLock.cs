﻿namespace Framework.DomainDriven.Lock;

/// <summary>
/// Операция для объекта, на котором можно сделать пессимистическую блокировку
/// </summary>
public record NamedLock(string Name, Type? DomainType = null)
{
    public NamedLock(Type domainType)
        : this(domainType.Name, domainType)
    {
    }
}
