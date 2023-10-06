using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial interface ISampleSystemBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITrackingServiceContainer<PersistentDomainObjectBase>,

    ITypeResolverContainer<string>,

    Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,

    IDefaultHierarchicalBLLContext<PersistentDomainObjectBase, Guid>
{
    ISecurityExpressionBuilderFactory SecurityExpressionBuilderFactory { get; }
}
