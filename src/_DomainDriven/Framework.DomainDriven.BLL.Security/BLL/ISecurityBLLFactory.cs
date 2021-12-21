using Framework.Core;

namespace Framework.DomainDriven.BLL.Security
{
    public interface ISecurityBLLFactory<out TBLL, in TSecurityOperationObject> : IFactory<TSecurityOperationObject, TBLL>, IFactory<TBLL>
    {
    }
}
