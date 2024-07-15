namespace Framework.SecuritySystem;

public abstract record SecurityAccessorData
{
    public static SecurityAccessorData Infinity { get; } = new InfinitySecurityAccessorData();

    public static SecurityAccessorData Empty { get; } = Return();

    public static SecurityAccessorData Return(params string[] items) => new FixedSecurityAccessorData(items);

    public static SecurityAccessorData Return(IEnumerable<string> items) => Return(items.ToArray());

    public record FixedSecurityAccessorData(IReadOnlyList<string> Items) : SecurityAccessorData;

    public record InfinitySecurityAccessorData : SecurityAccessorData;

    public record AndSecurityAccessorData(SecurityAccessorData Left, SecurityAccessorData Right) : SecurityAccessorData;

    public record OrSecurityAccessorData(SecurityAccessorData Left, SecurityAccessorData Right) : SecurityAccessorData;

    public record NegateSecurityAccessorData(SecurityAccessorData InnerData) : SecurityAccessorData;
}
