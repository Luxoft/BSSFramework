namespace Framework.SecuritySystem;

public abstract record SecurityAccessorResult
{
    public static SecurityAccessorResult Infinity { get; } = new InfinitySecurityAccessorResult();

    public static SecurityAccessorResult Empty { get; } = Return();

    public static SecurityAccessorResult Return(params string[] items) => new FixedSecurityAccessorResult(items);

    public static SecurityAccessorResult Return(IEnumerable<string> items) => Return(items.ToArray());

    public record FixedSecurityAccessorResult(IReadOnlyList<string> Items) : SecurityAccessorResult;

    public record InfinitySecurityAccessorResult : SecurityAccessorResult;

    public record AndSecurityAccessorResult(SecurityAccessorResult Left, SecurityAccessorResult Right) : SecurityAccessorResult;

    public record OrSecurityAccessorResult(SecurityAccessorResult Left, SecurityAccessorResult Right) : SecurityAccessorResult;

    public record NegateSecurityAccessorResult(SecurityAccessorResult BaseResult) : SecurityAccessorResult;
}
