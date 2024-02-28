namespace Framework.DomainDriven.ServiceModel.IAD;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    public List<Type> SecurityOperationTypes { get; set; } = new();

    public List<Type> NamedLockTypes { get; set; } = new();

    public bool RegisterBaseSecurityOperationTypes { get; set; } = true;

    public bool RegisterBaseNamedLockTypes { get; set; } = true;


    public IBssFrameworkSettings AddSecurityOperationType(Type securityOperationType)
    {
        this.SecurityOperationTypes.Add(securityOperationType);

        return this;
    }

    public IBssFrameworkSettings AddNamedLockType(Type namedLockType)
    {
        this.NamedLockTypes.Add(namedLockType);

        return this;
    }
}
