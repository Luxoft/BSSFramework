using System;

using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.DiTests;

public class ExampleSecurityOperationResolver : ISecurityOperationResolver<PersistentDomainObjectBase, ExampleSecurityOperation>
{
    public SecurityOperation<ExampleSecurityOperation> GetSecurityOperation(ExampleSecurityOperation securityOperationCode)
    {
        switch (securityOperationCode)
        {
            case ExampleSecurityOperation.EmployeeView:
                return new ContextSecurityOperation<ExampleSecurityOperation>(ExampleSecurityOperation.EmployeeView, HierarchicalExpandType.Children);

            default:
                throw new NotImplementedException();
        }
    }

    public SecurityOperation<ExampleSecurityOperation> GetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
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
