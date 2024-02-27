using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
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

    IDefaultHierarchicalBLLContext<PersistentDomainObjectBase, Guid>
{
    IConfigurationBLLContext Configuration { get; }

    ISecurityExpressionBuilderFactory SecurityExpressionBuilderFactory { get; }
}
