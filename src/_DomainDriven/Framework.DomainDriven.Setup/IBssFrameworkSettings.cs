using Framework.Persistent;

using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

namespace Framework.DomainDriven.Setup;

public interface IBssFrameworkSettings
{
    bool RegisterBaseSecurityOperationTypes { get; set; }

    bool RegisterBaseNamedLockTypes { get; set; }

    IBssFrameworkSettings AddSecurityOperationType(Type securityOperationType);

    IBssFrameworkSettings AddSecurityRoleTypeType(Type securityRoleType);

    IBssFrameworkSettings AddNamedLockType(Type namedLockType);

    //IBssFrameworkSettings AddSecurityContext(Action<ISecurityContextInfoBuilder<Guid>> setup);

    IBssFrameworkSettings AddSecurityContext<TSecurityContext>(Guid ident, string name = null, Func<TSecurityContext, string> displayFunc = null)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>;

    IBssFrameworkSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup);

    IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension);
}
