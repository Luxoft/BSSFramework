using Framework.Database.AuditProperty;

using NHibernate;
using NHibernate.Type;

namespace Framework.Database.NHibernate.Audit;

/// <summary> NHibernate Interceptor for setting Audit properties (<seealso cref="IAuditProperty"/>) on insert\update domain object
/// </summary>
internal sealed class AuditInterceptor(IEnumerable<IAuditProperty> createAuditProperties, IEnumerable<IAuditProperty> modifyAuditProperties)
    : EmptyInterceptor
{
    private readonly AuditPropertiesSetter createSetter = new(createAuditProperties);
    private readonly AuditPropertiesSetter modifySetter = new(modifyAuditProperties);

    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
    {
        var result = false;
        if (entity is IAuditObject)
        {
            result =
                    this.modifySetter.SetAuditFields(
                                                     AuditPropertiesSetter.DomainObjectDescription.Get(entity.GetType(), propertyNames),
                                                     ref currentState);
        }

        return result;
    }

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        var result = false;
        if (entity is IAuditObject)
        {
            var domainObjectDescription = AuditPropertiesSetter.DomainObjectDescription.Get(entity.GetType(), propertyNames);
            var createSetterRes = this.createSetter.SetAuditFields(domainObjectDescription, ref state);
            var modifySetterRes = this.modifySetter.SetAuditFields(domainObjectDescription, ref state);
            result = createSetterRes | modifySetterRes;
        }

        return result;
    }
}
