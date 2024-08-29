namespace Framework.SecuritySystem;

public interface ISecurityContextDisplayService<in TSecurityContext>
{
    string ToString(TSecurityContext securityContext);
}
