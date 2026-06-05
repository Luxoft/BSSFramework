using Anch.OData;
using Anch.SecuritySystem.SecurityAccessor;
using Anch.SecuritySystem.UserSource;

using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.Configuration.BLL;
using Framework.Validation;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;

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

