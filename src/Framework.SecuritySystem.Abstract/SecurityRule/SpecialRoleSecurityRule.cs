namespace Framework.SecuritySystem;

/// <summary>
/// Правила доступа для специальных ролей
/// </summary>
/// <param name="Name"></param>
public record SpecialRoleSecurityRule(string Name) : SecurityRule.DomainObjectSecurityRule
{
    /// <summary>
    /// Правило доступа по администраторской роли
    /// </summary>
    public static SpecialRoleSecurityRule Administrator { get; } = new (nameof(Administrator));

    /// <summary>
    /// Правло доступа для интеграционной роли
    /// </summary>
    public static SpecialRoleSecurityRule SystemIntegration { get; } = new(nameof(SystemIntegration));

    public override string ToString() => this.Name;
}
