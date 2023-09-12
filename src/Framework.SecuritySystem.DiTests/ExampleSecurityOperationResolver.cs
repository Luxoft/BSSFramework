using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.DiTests;

public class ExampleSecurityOperationResolver : ISecurityOperationResolver<PersistentDomainObjectBase>
{
    public SecurityOperation GetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : PersistentDomainObjectBase
    {
        if (typeof(TDomainObject) == typeof(Employee) && securityMode == BLLSecurityMode.View)
        {
            return ExampleSecurityOperation.EmployeeView;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
