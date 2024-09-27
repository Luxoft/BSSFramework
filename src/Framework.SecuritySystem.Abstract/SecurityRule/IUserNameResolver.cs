namespace Framework.SecuritySystem;

public interface IUserNameResolver
{
    string? Resolve(SecurityRuleCredential credential);
}
