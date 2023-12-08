namespace Automation.ServiceEnvironment.Services;

/// <summary>
/// Реализация интерфейса <see cref="TimeProvider"/> для интеграционных тестов
/// </summary>
public class IntegrationTestTimeProvider : TimeProvider
{
    private Func<DateTimeOffset> getNow;

    public IntegrationTestTimeProvider() => this.Reset();

    public void Reset() => this.getNow = () => DateTimeOffset.Now;

    public override DateTimeOffset GetUtcNow() => this.getNow();

    public virtual void SetCurrentDateTime(DateTime dateTime)
    {
        var dateTimeDelta = dateTime - DateTimeOffset.Now;

        this.getNow = () => DateTimeOffset.Now + dateTimeDelta;
    }
}
