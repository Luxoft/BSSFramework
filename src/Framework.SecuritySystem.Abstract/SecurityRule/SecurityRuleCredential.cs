namespace Framework.SecuritySystem;

public record SecurityRuleCredential(string Name) : SecurityRule
{
    public static SecurityRuleCredential CurrentUserWithRunAs { get; } = new SecurityRuleCredential(nameof(CurrentUserWithRunAs));

    public static SecurityRuleCredential CurrentUserWithoutRunAs { get; } = new SecurityRuleCredential(nameof(CurrentUserWithoutRunAs));

    public static SecurityRuleCredential AnyUser { get; } = new SecurityRuleCredential(nameof(AnyUser));

    public record CustomPrincipalSecurityRuleCredential(string Name) : SecurityRuleCredential(Name);
}
