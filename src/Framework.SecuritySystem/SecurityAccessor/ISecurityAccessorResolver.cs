namespace Framework.SecuritySystem.SecurityAccessor;

public interface ISecurityAccessorResolver
{
    IEnumerable<string> Resolve(SecurityAccessorData data);
}
