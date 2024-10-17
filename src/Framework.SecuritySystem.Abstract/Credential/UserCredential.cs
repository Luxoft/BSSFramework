namespace Framework.SecuritySystem.Credential;

public abstract record UserCredential
{
    public record NamedUserCredential(string Name) : UserCredential;

    public record IdentUserCredential(Guid Id) : UserCredential;

    public static implicit operator UserCredential(string name)
    {
        return new NamedUserCredential(name);
    }

    public static implicit operator UserCredential(Guid id)
    {
        return new IdentUserCredential(id);
    }
}
