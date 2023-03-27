using Framework.Core;

namespace Framework.SecuritySystem;

/// <summary>
/// Неконстектстная операция доступа
/// </summary>
/// <typeparam name="TSecurityOperationCode">Код контекстной операции (Enum)</typeparam>
public class NonContextSecurityOperation<TSecurityOperationCode> : SecurityOperation<TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum
{
    public NonContextSecurityOperation(TSecurityOperationCode code)
            : base(code)
    {
        if (this.Code.IsDefault()) { throw new ArgumentOutOfRangeException(nameof(code)); }
    }
}
