using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLL.Tracking;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial interface ISampleSystemBLLContext :

        ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>,

        ITrackingServiceContainer<PersistentDomainObjectBase>,

        ITypeResolverContainer<string>,

        Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,

        IDefaultHierarchicalBLLContext<PersistentDomainObjectBase, Guid>,

        ISecurityTypeResolverContainer
{
}
