namespace Framework.Core;

public record TypeNameIdentity
{
    public required string Name { get; init; }

    public required string Namespace { get; init; }

    public override string ToString() => string.IsNullOrWhiteSpace(this.Namespace) ? this.Name : $"{this.Namespace}.{this.Name}";

    public static implicit operator TypeNameIdentity(Type type) => new() { Namespace = type.Namespace!, Name = type.Name };

    public static implicit operator TypeNameIdentity(string fullName)
    {
        var lastSep = fullName.LastIndexOf('.');

        return lastSep < 0
                   ? new TypeNameIdentity { Name = fullName, Namespace = string.Empty }
                   : new TypeNameIdentity { Namespace = fullName[..lastSep], Name = fullName[(lastSep + 1)..] };
    }
}
