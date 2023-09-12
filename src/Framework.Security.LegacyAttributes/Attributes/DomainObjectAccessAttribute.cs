using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class DomainObjectAccessAttribute : Attribute
{
    public DomainObjectAccessAttribute(Type securityOperationType, string name)
        : this(securityOperationType.Maybe(v => v.GetSecurityOperation(name)))
    {
    }

    protected DomainObjectAccessAttribute(SecurityOperation securityOperation)
    {
        this.SecurityOperation = securityOperation;
    }


    public SecurityOperation SecurityOperation { get; private set; }


    public bool HasContext => this.SecurityOperation is ContextSecurityOperation;
}
