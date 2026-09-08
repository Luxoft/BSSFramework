using Framework.Database.AuditProperty;
using Framework.Database.InlineAudit;

using NHibernate;
using NHibernate.Type;

namespace Framework.Database.NHibernate.Audit;

internal sealed class InlineAuditInterceptor(IEnumerable<InlineAuditBinding> inlineAuditBindings)
    : EmptyInterceptor
{
    private readonly AuditPropertiesSetter createSetter = new(createAuditProperties);

    private readonly AuditPropertiesSetter modifySetter = new(modifyAuditProperties);

    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
    {
        var result = false;

        if (entity is IAuditObject)
        {
            result = this.modifySetter.SetAuditFields(AuditPropertiesSetter.DomainObjectDescription.Get(entity.GetType(), propertyNames), currentState);
        }

        return result;
    }

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        var result = false;

        if (entity is IAuditObject)
        {
            var domainObjectDescription = AuditPropertiesSetter.DomainObjectDescription.Get(entity.GetType(), propertyNames);
            var createSetterRes = this.createSetter.SetAuditFields(domainObjectDescription, state);
            var modifySetterRes = this.modifySetter.SetAuditFields(domainObjectDescription, state);
            result = createSetterRes | modifySetterRes;
        }

        return result;
    }
}
