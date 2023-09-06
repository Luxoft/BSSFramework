namespace Framework.SecuritySystem;

public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message)
        : base(message)
    {
    }
}
