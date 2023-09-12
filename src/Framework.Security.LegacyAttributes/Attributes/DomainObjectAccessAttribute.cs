using Framework.SecuritySystem;

namespace Framework.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class DomainObjectAccessAttribute : Attribute
{
    public DomainObjectAccessAttribute()
    {

    }

    protected DomainObjectAccessAttribute(SecurityOperation securityOperation)
    {
        this.SecurityOperation = securityOperation;
    }


    public SecurityOperation SecurityOperation { get; private set; }


    public bool HasContext
    {
        get { return this.SecurityOperation is ContextSecurityOperation; }
    }
}
