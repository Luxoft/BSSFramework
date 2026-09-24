using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Database.EntityFramework.InlineAudit;

public class EmptyAuditPropertiesSetterMap : IAuditPropertiesSetterMap
{
    private EmptyAuditPropertiesSetterMap()
    {
    }

    private readonly IAuditPropertiesSetter setter = new EmptyAuditPropertiesSetter();

    public IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider) => this.setter;

    private class EmptyAuditPropertiesSetter : IAuditPropertiesSetter
    {
        public bool SetAuditFields(EntityEntry entry) => false;
    }

    public static EmptyAuditPropertiesSetterMap Instance { get; } = new();
}
