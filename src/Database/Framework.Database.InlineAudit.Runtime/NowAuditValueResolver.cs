namespace Framework.Database.InlineAudit;

public class NowAuditValueResolver(TimeProvider timeProvider) : IAuditValueResolver<DateTime?>
{
    public DateTime? GetCurrentValue() => DateTime.SpecifyKind(timeProvider.GetUtcNow().DateTime, DateTimeKind.Utc);
}
