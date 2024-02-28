namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IBssFrameworkSettings
{
    bool RegisterBaseSecurityOperationTypes { get; set; }

    bool RegisterBaseNamedLockTypes { get; set; }

    IBssFrameworkSettings AddSecurityOperationType(Type securityOperationType);

    IBssFrameworkSettings AddNamedLockType(Type namedLockType);
}
