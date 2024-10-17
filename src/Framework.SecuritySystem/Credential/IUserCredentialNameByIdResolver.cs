namespace Framework.SecuritySystem.Credential;

public interface IUserCredentialNameByIdResolver
{
    public string? TryGetUserName(Guid id);
}
