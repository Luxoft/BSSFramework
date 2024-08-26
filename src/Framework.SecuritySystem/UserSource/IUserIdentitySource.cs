namespace Framework.SecuritySystem.UserSource;

public interface IUserIdentitySource
{
    Guid? TryGetId(string name);
}
