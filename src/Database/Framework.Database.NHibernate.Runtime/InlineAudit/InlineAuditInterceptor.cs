using Framework.Database.InlineAudit;

using NHibernate;
using NHibernate.Type;

namespace Framework.Database.NHibernate.InlineAudit;

public class InlineAuditInterceptor(IServiceProvider serviceProvider, IAuditPropertiesSetterMapFactory auditPropertiesSetterMapFactory)
    : EmptyInterceptor, IInlineAuditInterceptor
{
    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types) =>
        auditPropertiesSetterMapFactory
            .CreateSetterMap(entity.GetType(), InlineAuditAction.Modify, propertyNames)
            .CreateSetter(serviceProvider)
            .SetAuditFields(currentState);

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types) =>
        new[] { InlineAuditAction.Create, InlineAuditAction.Modify }
            .Select(action => auditPropertiesSetterMapFactory.CreateSetterMap(entity.GetType(), action, propertyNames))
            .Aggregate()
            .CreateSetter(serviceProvider)
            .SetAuditFields(state);
}
