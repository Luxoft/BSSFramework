namespace Framework.Database.InlineAudit;

public class NowDateTimeAuditValueResolver(TimeProvider timeProvider) : IAuditValueResolver<DateTime>
{
    public DateTime GetCurrentValue() => timeProvider.GetUtcNow().DateTime;
}
