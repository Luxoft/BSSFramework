using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public interface IRequiredApproveService
{
    public bool RequiredApprove(BusinessRole businessRole);
}
