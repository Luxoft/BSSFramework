using SecuritySystem.AccessDenied;

namespace Framework.BLL.AccessDeniedExceptionService;

public interface IAccessDeniedExceptionServiceContainer
{
    IAccessDeniedExceptionService AccessDeniedExceptionService
    {
        get;
    }
}
