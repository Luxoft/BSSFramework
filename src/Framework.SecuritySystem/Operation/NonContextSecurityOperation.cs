namespace Framework.SecuritySystem;

/// <summary>
/// Неконстектстная операция доступа
/// </summary>
public abstract record NonContextSecurityOperation(string Name) : SecurityOperation(Name)
{
}

public record NonContextSecurityOperation<TIdent>(string Name, TIdent Id) : NonContextSecurityOperation(Name)
{
}
