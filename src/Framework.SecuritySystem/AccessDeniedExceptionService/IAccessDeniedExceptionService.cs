namespace Framework.SecuritySystem;

public interface IAccessDeniedExceptionService
{
    Exception GetAccessDeniedException(AccessResult.AccessDeniedResult accessDeniedResult);
}
