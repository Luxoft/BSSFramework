using System;

namespace Framework.SecuritySystem;

/// <summary>
/// Специальная операция для отключённой безопасности
/// </summary>
/// <typeparam name="TSecurityOperationCode"></typeparam>
public class DisabledSecurityOperation<TSecurityOperationCode> : SecurityOperation<TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum
{
    public DisabledSecurityOperation()
            : base(default)
    {
    }
}
