using Framework.SecuritySystem.UserSource;

namespace Framework.SecuritySystem.Credential;

public abstract record UserCredential
{
    public abstract bool IsMatch(User user);

    public record NamedUserCredential(string Name) : UserCredential
    {
        public override bool IsMatch(User user) => this.IsMatch(user.Name);

        public override int GetHashCode() => this.Name.ToLower().GetHashCode();

        public virtual bool Equals(NamedUserCredential? other) => other is not null && this.IsMatch(other.Name);

        public override string ToString() => this.Name;

        private bool IsMatch(string name) => string.Equals(name, this.Name, StringComparison.OrdinalIgnoreCase);
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
