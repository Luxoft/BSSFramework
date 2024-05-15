namespace Framework.Authorization.SecuritySystem;

public interface IPrincipalIdentitySource
{
    Guid? TryGetId(string name);
}
