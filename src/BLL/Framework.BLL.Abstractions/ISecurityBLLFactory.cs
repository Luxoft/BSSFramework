using Anch.SecuritySystem;
using Anch.SecuritySystem.Providers;

namespace Framework.BLL;
public interface ISecurityBLLFactory<out TBLL>
{
    TBLL Create();

    TBLL Create(SecurityRule securityRule);
}

public interface ISecurityBLLFactory<out TBLL, TDomainObject> : ISecurityBLLFactory<TBLL>
{
    TBLL Create(ISecurityProvider<TDomainObject> securityProvider);
}
