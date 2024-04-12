namespace Framework.SecuritySystem;

public record SpecialRoleSecurityRule(string Name) : SecurityRule.DomainObjectSecurityRule
{
    public static SpecialRoleSecurityRule Administrator { get; } = new (nameof(Administrator));

    public static SpecialRoleSecurityRule SystemIntegration { get; } = new(nameof(SystemIntegration));

    public override string ToString() => this.Name;
}
