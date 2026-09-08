namespace Framework.Database.InlineAudit;

public interface IAuditValueResolver<out TProperty>
    where TProperty : notnull
{
    TProperty GetCurrentValue();
}
