using Framework.Core;

namespace Framework.SecuritySystem.Expanders;

public record SecurityRuleExpandSettings(DeepEqualsCollection<Type> IgnoredTypes)
{
    public SecurityRuleExpandSettings(IEnumerable<Type> ignoredTypes)
        : this(DeepEqualsCollection.Create(ignoredTypes))
    {
    }

    public static SecurityRuleExpandSettings Disabled { get; } = new(new[] { typeof(SecurityRule) });
}
