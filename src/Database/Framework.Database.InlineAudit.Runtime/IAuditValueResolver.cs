namespace Framework.Database.InlineAudit;

public interface IAuditValueResolver<out TProperty>
{
    TProperty GetCurrentValue();
}
