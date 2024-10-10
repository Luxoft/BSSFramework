namespace Framework.SecuritySystem;

public record ClientSecurityRuleHeader(string Name)
{
    public override string ToString() => this.Name;
}
