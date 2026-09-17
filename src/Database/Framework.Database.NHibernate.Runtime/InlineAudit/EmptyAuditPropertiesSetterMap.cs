namespace Framework.Database.NHibernate.InlineAudit;

public class EmptyAuditPropertiesSetterMap : IAuditPropertiesSetterMap
{
    private EmptyAuditPropertiesSetterMap()
    {
    }

    private readonly IAuditPropertiesSetter setter = new EmptyAuditPropertiesSetter();

    public IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider) => this.setter;

    private class EmptyAuditPropertiesSetter : IAuditPropertiesSetter
    {
        public bool SetAuditFields(object[] state) => false;
    }

    public static EmptyAuditPropertiesSetterMap Instance { get; } = new();
}
