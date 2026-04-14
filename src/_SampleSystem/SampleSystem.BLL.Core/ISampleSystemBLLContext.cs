using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.Configuration.BLL;
using Framework.Validation;

using OData;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;

using SecuritySystem.SecurityAccessor;
using SecuritySystem.UserSource;

namespace SampleSystem.BLL;

public partial interface ISampleSystemBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>
{
    ICurrentUserSource<Employee> CurrentEmployeeSource { get; }

    IConfigurationBLLContext Configuration { get; }

    ISecurityAccessorResolver SecurityAccessorResolver { get; }

    IValidator Validator { get; }

    ISelectOperationParser SelectOperationParser { get; }
}
