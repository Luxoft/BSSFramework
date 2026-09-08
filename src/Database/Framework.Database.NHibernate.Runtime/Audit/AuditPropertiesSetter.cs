using Anch.Core;
using Anch.Core.DictionaryCache;

using Framework.Core;
using Framework.Database.InlineAudit;

namespace Framework.Database.NHibernate.Audit;

/// <summary> Setter of Audit properties (<seealso cref="IAuditProperty"/>) into NHibernate domain object state collection
/// </summary>
/// <remarks>
/// Encapsulates logic of searching and setting audit properties inside domain object's NHibernate state collection
/// </remarks>
internal sealed partial class AuditPropertiesSetter
{
    private readonly IDictionaryCache<DomainObjectDescription, Func<object[], bool>> setCache;

    public AuditPropertiesSetter(IEnumerable<InlineAuditBinding> auditProperties)
    {
        if (auditProperties is null)
        {
            throw new ArgumentNullException(nameof(auditProperties));
        }

        var getSetAuditActionMethod = new Func<string[], InlineAuditBinding<object, object>, Func<object[], bool>>(GetSetAuditAction<object, object, object>).Method.GetGenericMethodDefinition();

        this.setCache = new DictionaryCache<DomainObjectDescription, Func<object[], bool>>(domainObjectDescription =>
        {
            var requests = (from auditProperty in auditProperties
                            where auditProperty is not null
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

    public bool SetAuditFields(DomainObjectDescription domainObjectDescription, object[] state)
    {
        if (domainObjectDescription is null)
        {
            throw new ArgumentNullException(nameof(domainObjectDescription));
        }

        var setAction = this.setCache[domainObjectDescription];
        return setAction(state);
    }

    private static Func<object[], bool> GetSetAuditAction<TDomainObject, TPropertyDomainObject, TProperty>(string[]? propertyNames, InlineAuditBinding<TPropertyDomainObject, TProperty> auditBinding)
            where TDomainObject : TPropertyDomainObject
    {
        if (propertyNames?.Any() == false)
        {
            return _ => false;
        }

        int? propertyIndex = null;

        var domainObjectPropertyName = auditBinding.PropertyAccessors.Path.GetMemberName();
        if (!string.IsNullOrEmpty(domainObjectPropertyName))
        {
            var property = typeof(TDomainObject).GetProperty(domainObjectPropertyName, true)!;
            propertyIndex = GetPropertyIndex(propertyNames, property.Name);
        }

        return state =>
               {
                   var result = false;
                   if (propertyIndex.HasValue)
                   {
                       var auditValue = (getAuditValue is not null) ? getAuditValue.Invoke() : default(TProperty);
                       state[propertyIndex.Value] = auditValue!;
                       result = true;
                   }

                   return result;
               };
    }

    private static int? GetPropertyIndex(IReadOnlyList<string>? source, string propertyName)
    {
        if (source is null || string.IsNullOrEmpty(propertyName))
        {
            return null;
        }

        int? result = null;
        for (var i = 0; i < source.Count; i++)
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
