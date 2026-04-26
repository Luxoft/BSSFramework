using Anch.SecuritySystem.AccessDenied;

namespace Framework.BLL;

public interface IAccessDeniedExceptionServiceContainer
{
    IAccessDeniedExceptionService AccessDeniedExceptionService
    {
        get;
    }
}
