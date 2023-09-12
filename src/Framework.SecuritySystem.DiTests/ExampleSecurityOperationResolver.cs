using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.DiTests;

public class ExampleSecurityOperationResolver : ISecurityOperationResolver<PersistentDomainObjectBase>
{
    public SecurityOperation GetSecurityOperation(SecurityOperation securityOperationCode)
    {
        throw new NotImplementedException();
    }

    public SecurityOperation GetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : PersistentDomainObjectBase
    {
        if (typeof(TDomainObject) == typeof(Employee) && securityMode == BLLSecurityMode.View)
        {
            return this.GetSecurityOperation(ExampleSecurityOperation.EmployeeView);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
