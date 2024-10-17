using Framework.SecuritySystem.UserSource;

namespace Framework.SecuritySystem.Credential;

public abstract record UserCredential
{
    public abstract bool IsMatch(User user);

    public record NamedUserCredential(string Name) : UserCredential
    {
        public override bool IsMatch(User user) => user.Name == this.Name;

        public override string ToString() => this.Name;
    }

    public record IdentUserCredential(Guid Id) : UserCredential
    {
        public override bool IsMatch(User user) => user.Id == this.Id;

        public override string ToString() => this.Id.ToString();
    }

    public static implicit operator UserCredential(string name)
    {
        return name == null ? null : new NamedUserCredential(name);
    }

    public static implicit operator UserCredential(Guid id)
    {
        return new IdentUserCredential(id);
    }
}
