namespace Framework.Authorization.SecuritySystem.UserSource;

public interface IPrincipalIdentitySource
{
    Guid? TryGetId(string name);
}
