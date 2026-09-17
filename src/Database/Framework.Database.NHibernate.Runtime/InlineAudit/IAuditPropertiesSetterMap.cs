namespace Framework.Database.NHibernate.InlineAudit;

public interface IAuditPropertiesSetterMap
{
    IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider);
}
