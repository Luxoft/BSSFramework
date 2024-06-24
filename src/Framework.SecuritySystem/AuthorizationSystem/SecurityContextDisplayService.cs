namespace Framework.SecuritySystem;

public class SecurityContextDisplayService<TSecurityContext> : ISecurityContextDisplayService<TSecurityContext>
{
    private readonly Func<TSecurityContext, string> toStringFunc;

    public SecurityContextDisplayService(Func<TSecurityContext, string> toStringFunc)
    {
        this.toStringFunc = toStringFunc;
    }

    public string ToString(TSecurityContext securityContext)
    {
        return this.toStringFunc(securityContext);
    }
}
