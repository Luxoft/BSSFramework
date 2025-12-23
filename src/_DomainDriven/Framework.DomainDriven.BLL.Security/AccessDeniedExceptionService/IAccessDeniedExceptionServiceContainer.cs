using SecuritySystem.AccessDenied;

namespace Framework.DomainDriven.BLL.Security;

public interface IAccessDeniedExceptionServiceContainer
{
    IAccessDeniedExceptionService AccessDeniedExceptionService
    {
        get;
    }
}
