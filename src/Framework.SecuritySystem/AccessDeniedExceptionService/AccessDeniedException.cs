namespace Framework.SecuritySystem.AccessDeniedExceptionService;

public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message)
        : base(message)
    {
    }
}
