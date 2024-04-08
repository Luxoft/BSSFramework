using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLFactory<out TBLL, TDomainObject>
{
    TBLL Create();

    TBLL Create(SecurityRule securityMode);

    TBLL Create(SecurityRule securityRule);

    TBLL Create(ISecurityProvider<TDomainObject> securityProvider);
}
