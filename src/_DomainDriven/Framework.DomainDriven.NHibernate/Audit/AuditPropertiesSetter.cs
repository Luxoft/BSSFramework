using Framework.Core;
using Framework.DomainDriven.Audit;

namespace Framework.DomainDriven.NHibernate.Audit;

/// <summary> Setter of Audit properties (<seealso cref="IAuditProperty"/>) into NHibernate domain object state collection
/// </summary>
/// <remarks>
/// Incapsulates logic of searching and setting audit properties inside domain object's NHibernate state collection
/// </remarks>
internal sealed partial class AuditPropertiesSetter
{
    private readonly IDictionaryCache<DomainObjectDescription, Func<object[], bool>> setCache;

    public AuditPropertiesSetter(IEnumerable<IAuditProperty> auditProperties)
    {
        if (auditProperties == null)
        {
            throw new ArgumentNullException(nameof(auditProperties));
        }

        var getSetAuditActionMethod = new Func<string[], IAuditProperty<object, object>, Func<object[], bool>>(GetSetAuditAction<object, object, object>).Method.GetGenericMethodDefinition();

        this.setCache = new DictionaryCache<DomainObjectDescription, Func<object[], bool>>(domainObjectDescription =>
        {
            var requests = (from auditProperty in auditProperties
                            where auditProperty != null
                            let propertyDomainObjectType = auditProperty.PropertyExpr?.Parameters?.Single()?.Type
                            let propertyType = auditProperty.PropertyExpr?.ReturnType
                            where propertyDomainObjectType?.IsAssignableFrom(domainObjectDescription.Type) == true
                            select getSetAuditActionMethod
                                   .MakeGenericMethod(domainObjectDescription.Type, propertyDomainObjectType, propertyType)
                                   .Invoke<Func<object[], bool>>(this, domainObjectDescription.PropertyNames, auditProperty))
                    .ToList();

            return param => requests.Aggregate(false, (total, next) => total | next.Invoke(param));
        }).WithLock();
    }

    public bool SetAuditFields(DomainObjectDescription domainObjectDescription, ref object[] state)
    {
        if (domainObjectDescription == null)
        {
            throw new ArgumentNullException(nameof(domainObjectDescription));
        }

        var setAction = this.setCache[domainObjectDescription];
        return setAction(state);
    }

    private static Func<object[], bool> GetSetAuditAction<TDomainObject, TPropertyDomainObject, TProperty>(string[] propertyNames, IAuditProperty<TPropertyDomainObject, TProperty> auditProperty)
            where TDomainObject : TPropertyDomainObject
    {
        if (propertyNames?.Any() == false || auditProperty?.PropertyExpr == null)
        {
            return _ => false;
        }

        int? propertyIndex = null;
        Func<TProperty> getAuditValue = null;

        string domainObjectPropertyName = auditProperty.PropertyExpr.GetMemberName();
        if (!string.IsNullOrEmpty(domainObjectPropertyName))
        {
            var property = typeof(TDomainObject).GetProperty(domainObjectPropertyName, true);
            getAuditValue = auditProperty.GetCurrentValue;
            propertyIndex = GetPropertyIndex(propertyNames, property?.Name);
        }

        return (state) =>
               {
                   bool result = false;
                   if (propertyIndex.HasValue)
                   {
                       var auditValue = (getAuditValue != null) ? getAuditValue.Invoke() : default(TProperty);
                       state[propertyIndex.Value] = auditValue;
                       result = true;
                   }

                   return result;
               };
    }

    private static int? GetPropertyIndex(IReadOnlyList<string> source, string propertyName)
    {
        if (source == null || string.IsNullOrEmpty(propertyName))
        {
            return null;
        }

        int? result = null;
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i]?.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase) == true)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
