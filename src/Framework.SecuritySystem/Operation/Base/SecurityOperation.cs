namespace Framework.SecuritySystem;

/// <summary>
/// Операция доступа
/// </summary>
/// <typeparam name="TSecurityOperationCode">Тип кода контекстной операции (Enum)</typeparam>
public abstract class SecurityOperation<TSecurityOperationCode> : IEquatable<SecurityOperation<TSecurityOperationCode>>, ISecurityOperation
        where TSecurityOperationCode : struct, Enum
{
    protected SecurityOperation(TSecurityOperationCode code)
    {
        this.Code = code;
    }


    public TSecurityOperationCode Code { get; }

    public override bool Equals(object other)
    {
        return this.Equals(other as SecurityOperation<TSecurityOperationCode>);
    }

    public override int GetHashCode()
    {
        var contextOperation = this as ContextSecurityOperation<TSecurityOperationCode>;

        return contextOperation == null ? this.Code.GetHashCode() : contextOperation.Code.GetHashCode() ^ contextOperation.SecurityExpandType.GetHashCode();
    }

    public override string ToString()
    {
        return $"Code = {this.Code}";
    }

    public virtual bool Equals(SecurityOperation<TSecurityOperationCode> other)
    {
        if (object.ReferenceEquals(other, null))
        {
            return false;
        }
        else if (!EqualityComparer<TSecurityOperationCode>.Default.Equals(this.Code, other.Code))
        {
            return false;
        }
        else if (this is NonContextSecurityOperation<TSecurityOperationCode> && other is NonContextSecurityOperation<TSecurityOperationCode>)
        {
            return true;
        }
        else if (this is DisabledSecurityOperation<TSecurityOperationCode> && other is DisabledSecurityOperation<TSecurityOperationCode>)
        {
            return true;
        }
        else if (this is ContextSecurityOperation<TSecurityOperationCode> op1 && other is ContextSecurityOperation<TSecurityOperationCode> op2)
        {
            return op1.SecurityExpandType == op2.SecurityExpandType;
        }
        else
        {
            return false;
        }
    }

    public static bool operator ==(SecurityOperation<TSecurityOperationCode> securityOperation1, SecurityOperation<TSecurityOperationCode> securityOperation2)
    {
        return object.ReferenceEquals(securityOperation1, securityOperation2)
               || (!object.ReferenceEquals(securityOperation1, null) && securityOperation1.Equals(securityOperation2));
    }

    public static bool operator !=(SecurityOperation<TSecurityOperationCode> securityOperation1, SecurityOperation<TSecurityOperationCode> securityOperation2)
    {
        return !(securityOperation1 == securityOperation2);
    }

    Enum ISecurityOperation.Code => this.Code;
}
