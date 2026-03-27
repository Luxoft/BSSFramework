using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Tracking;

using GenericQueryable.Fetching;
using SampleSystem.Domain;
using SecuritySystem.SecurityAccessor;
using SecuritySystem.UserSource;

namespace SampleSystem.BLL;

public partial interface ISampleSystemBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITrackingServiceContainer<PersistentDomainObjectBase>,

    ITypeResolverContainer<string>,

    IDefaultHierarchicalBLLContext<PersistentDomainObjectBase, Guid>
{
    IFetchRuleExpander FetchRuleExpander { get; }

    ICurrentUserSource<Employee> CurrentEmployeeSource { get; }

    IConfigurationBLLContext Configuration { get; }

    ISecurityAccessorResolver SecurityAccessorResolver { get; }
}
