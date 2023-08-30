using Framework.Core;

namespace Framework.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class DomainObjectAccessAttribute : Attribute
{
    public DomainObjectAccessAttribute()
    {

    }

    protected DomainObjectAccessAttribute(Enum securityOperationCode)
    {
        this.SecurityOperationCode = securityOperationCode;
    }


    public Enum SecurityOperationCode { get; private set; }


    public bool HasContext
    {
        get { return this.SecurityOperationCode.Maybe(v => v.ToFieldInfo().HasAttribute<SecurityOperationAttribute>(attr => attr.IsContext)); }
    }
}
