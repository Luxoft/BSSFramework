using Anch.Core;
using Anch.Core.DictionaryCache;

using Framework.Core;
using Framework.Database.AuditProperty;

using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace Framework.Database.NHibernate.Audit;

public abstract class AuditEventListenerBase
{
    private readonly IAuditProperty[] auditProperties;

    private readonly IDictionaryCache<IEntityPersister, Action<AbstractPreDatabaseOperationEvent, object[]>> setCache;


    protected AuditEventListenerBase(IEnumerable<IAuditProperty> auditProperties)
    {
        if (auditProperties == null) throw new ArgumentNullException(nameof(auditProperties));

        this.auditProperties = auditProperties.ToArray();

        var getSetAuditActionMethod = new Func<IEntityPersister, IAuditProperty<object, object>, Action<AbstractPreDatabaseOperationEvent, object[]>>(this.GetSetAuditAction<object, object, object>).Method.GetGenericMethodDefinition();

        this.setCache = new DictionaryCache<IEntityPersister, Action<AbstractPreDatabaseOperationEvent, object[]>>(entityPersister =>
        {
            var securityContextType = entityPersister.EntityMetamodel.Type;

            if (securityContextType == null)
            {
                return (_, __) => { };
            }
            else
            {
                var request = from auditProperty in this.auditProperties

                              let propertyDomainObjectType = auditProperty.PropertyExpr.Parameters.Single().Type

                              let propertyType = auditProperty.PropertyExpr.ReturnType

                              where propertyDomainObjectType.IsAssignableFrom(securityContextType)

                              select getSetAuditActionMethod.MakeGenericMethod(securityContextType, propertyDomainObjectType, propertyType)
                                                            .Invoke<Action<AbstractPreDatabaseOperationEvent, object[]>>(this, entityPersister, auditProperty);

                return request.Composite();
            }
        }).WithLock();
    }


    protected bool SetAuditFields(AbstractPreDatabaseOperationEvent @event, object[] state)
    {
        var setAction = this.setCache[@event.Persister];

        setAction(@event, state);

        return false;
    }

    private Action<AbstractPreDatabaseOperationEvent, object[]> GetSetAuditAction<TDomainObject, TPropertyDomainObject, TProperty>(IEntityPersister entityPersister, IAuditProperty<TPropertyDomainObject, TProperty> auditProperty)
            where TDomainObject : TPropertyDomainObject
    {
        var property = typeof(TDomainObject).GetProperty(auditProperty.PropertyExpr.GetMemberName(), true);

        var setAuditAction = property.GetSetValueAction<TDomainObject, TProperty>();

        var getAuditValue = auditProperty.GetCurrentValue;

        var propertyIndex = entityPersister.PropertyNames.GetIndex(property.Name);

        return (operationEvent, state) =>
               {
                   var domainObject = (TDomainObject) operationEvent.Entity;

                   var auditValue = getAuditValue();

                   setAuditAction(domainObject, auditValue);

                   state[propertyIndex] = auditValue;
               };
    }
}
