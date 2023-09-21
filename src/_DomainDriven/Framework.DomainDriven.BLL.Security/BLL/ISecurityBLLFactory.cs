using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLFactory<out TBLL, TDomainObject>
{
    TBLL Create();

    TBLL Create(BLLSecurityMode securityMode);

    TBLL Create(SecurityOperation securityOperation);

    TBLL Create(ISecurityProvider<TDomainObject> securityProvider);
}
