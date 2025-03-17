namespace Framework.SecuritySystem;

public abstract record SecurityContextRestriction(bool Required, string? Key)
{
    public abstract Type SecurityContextType { get; }

    public abstract SecurityContextRestrictionFilterInfo? RawFilter { get; }
}

public record SecurityContextRestriction<TSecurityContext>(
    bool Required,
    string? Key,
    SecurityContextRestrictionFilterInfo<TSecurityContext>? Filter)
    : SecurityContextRestriction(Required, Key)
    where TSecurityContext : ISecurityContext
{
    public override Type SecurityContextType { get; } = typeof(TSecurityContext);

    public override SecurityContextRestrictionFilterInfo? RawFilter => this.Filter;
}
