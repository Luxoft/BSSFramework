namespace Framework.SecuritySystem;

/// <summary>
/// Специальная операция для отключённой безопасности
/// </summary>
public record DisabledSecurityOperation() : SecurityOperation("Disabled")
{
}
