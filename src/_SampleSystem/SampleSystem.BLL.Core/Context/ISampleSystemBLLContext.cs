using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using SecuritySystem.SecurityAccessor;

using SampleSystem.Domain;

using SecuritySystem.UserSource;

namespace SampleSystem.BLL;

public partial interface ISampleSystemBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITrackingServiceContainer<PersistentDomainObjectBase>,

    ITypeResolverContainer<string>,

    IDefaultHierarchicalBLLContext<PersistentDomainObjectBase, Guid>
{
    ICurrentUserSource<Employee> CurrentEmployeeSource { get; }

    IConfigurationBLLContext Configuration { get; }

    ISecurityAccessorResolver SecurityAccessorResolver { get; }
}
