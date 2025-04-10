using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.DiTests;

public class TestCheckboxConditionFactory<TDomainObject>(IRelativeDomainPathInfo<TDomainObject, Employee> pathToEmployeeInfo)
    : IFactory<Expression<Func<TDomainObject, bool>>>
{
    public Expression<Func<TDomainObject, bool>> Create()
    {
        return pathToEmployeeInfo.CreateCondition(employee => employee.TestCheckbox);
    }
}
