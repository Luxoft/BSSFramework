using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class SampleSystemEmployeeSecurityService<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee> : ContextDomainSecurityService<PersistentDomainObjectBase, TDomainObject, Guid>
    where TDomainObject : PersistentDomainObjectBase, IEmployeeSecurity<TBusinessUnit, TDepartment, TLocation>, IBusinessUnitSecurityElement<TBusinessUnit>, IDepartmentSecurityElement<TDepartment>, IEmployeeSecurityElement<TEmployee, TBusinessUnit, TDepartment, TLocation>, IEmployeeSecurityElement<TEmployee>
    where TBusinessUnit : PersistentDomainObjectBase, ISecurityContext
    where TDepartment : PersistentDomainObjectBase, ILocationSecurityElement<TLocation>
    where TLocation : PersistentDomainObjectBase, ISecurityContext
    where TEmployee : PersistentDomainObjectBase, IEmployeeSecurity<TBusinessUnit, TDepartment, TLocation>, ISecurityContext
{
    private readonly IRunAsManager runAsManager;

    public SampleSystemEmployeeSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver<PersistentDomainObjectBase> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            IRunAsManager runAsManager)

            : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
    {
        this.runAsManager = runAsManager;
    }

    protected override SecurityPath<TDomainObject> GetSecurityPath() => SecurityPath<TDomainObject>.Create(v => v.Employee)
        .And(v => v.BusinessUnit).And(v => v.Department.Location);

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation securityOperation)
    {
        var baseProvider = base.CreateSecurityProvider(securityOperation);

        if (securityOperation == SampleSystemSecurityOperation.EmployeeView)
        {
            return baseProvider.Or(employee => employee.Login == this.runAsManager.ActualPrincipal.Name);
        }
        else
        {
            return baseProvider;
        }
    }
}
