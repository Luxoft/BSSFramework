namespace Framework.SecuritySystem;

public record SecurityRuleCredential(string Name)
{
    public static SecurityRuleCredential CurrentUserWithRunAs { get; } = new (nameof(CurrentUserWithRunAs));

    public static SecurityRuleCredential CurrentUserWithoutRunAs { get; } = new (nameof(CurrentUserWithoutRunAs));

    public static SecurityRuleCredential AnyUser { get; } = new (nameof(AnyUser));

    public record CustomPrincipalSecurityRuleCredential(string Name) : SecurityRuleCredential(Name);
}
