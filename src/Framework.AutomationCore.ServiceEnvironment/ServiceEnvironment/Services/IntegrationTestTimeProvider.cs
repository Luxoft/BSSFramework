namespace Automation.ServiceEnvironment.Services;

/// <summary>
/// Реализация интерфейса <see cref="TimeProvider"/> для интеграционных тестов
/// </summary>
public class IntegrationTestTimeProvider : TimeProvider
{
    private Func<DateTimeOffset> getNow;

    public IntegrationTestTimeProvider() => this.Reset();

    public void Reset() => this.getNow = () => DateTimeOffset.UtcNow;

    public override DateTimeOffset GetUtcNow() => this.getNow();

    public virtual void SetCurrentDateTime(DateTime dateTime)
    {
        var dateTimeDelta = dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime - DateTimeOffset.UtcNow,
            _ => dateTime - DateTimeOffset.Now
        };

        this.getNow = () => DateTimeOffset.UtcNow + dateTimeDelta;
    }
}
