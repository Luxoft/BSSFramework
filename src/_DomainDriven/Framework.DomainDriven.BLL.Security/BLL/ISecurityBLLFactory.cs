using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;
public interface ISecurityBLLFactory<out TBLL>
{
    TBLL Create();

    TBLL Create(SecurityRule securityRule);
}

public interface ISecurityBLLFactory<out TBLL, TDomainObject> : ISecurityBLLFactory<TBLL>
{
    TBLL Create(ISecurityProvider<TDomainObject> securityProvider);
}
