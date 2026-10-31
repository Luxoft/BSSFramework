namespace Framework.AutomationCore.Services;

/// <summary>
/// Implementation of <see cref="TimeProvider"/> for integration tests
/// </summary>
public class IntegrationTestTimeProvider : TimeProvider
{
    private TimeSpan offset;

    public virtual void Reset() => this.offset = TimeSpan.Zero;

    public override DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow + this.offset;

    public virtual void SetCurrentDateTime(DateTime dateTime) => this.offset = dateTime - DateTimeOffset.UtcNow;
}
