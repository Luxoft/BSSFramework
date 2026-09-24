namespace Framework.Database.EntityFramework.InlineAudit;

public interface IAuditPropertiesSetterMap
{
    IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider);
}
