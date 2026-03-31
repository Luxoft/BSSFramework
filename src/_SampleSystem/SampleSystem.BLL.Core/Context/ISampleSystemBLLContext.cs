using Framework.Authorization.BLL;
using Framework.BLL;
using Framework.Configuration.BLL;
using Framework.Core.TypeResolving;
using Framework.Tracking;

using GenericQueryable.Fetching;

using SampleSystem.Domain;

using SecuritySystem.SecurityAccessor;
using SecuritySystem.UserSource;

namespace SampleSystem.BLL;

public partial interface ISampleSystemBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITrackingServiceContainer<PersistentDomainObjectBase>,

    ITypeResolverContainer<string>
{
    IFetchRuleExpander FetchRuleExpander { get; }

    ICurrentUserSource<Employee> CurrentEmployeeSource { get; }

    IConfigurationBLLContext Configuration { get; }

    ISecurityAccessorResolver SecurityAccessorResolver { get; }
}
