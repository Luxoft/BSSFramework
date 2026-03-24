using System.Linq.Expressions;

namespace Framework.DomainDriven.Tracking;

/// <summary>
/// Represents tracking service. Tracking service helps track domain object changes during DB session
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public class TrackingService<TPersistentDomainObjectBase>(IObjectStateService objectStatesService, IPersistentInfoService persistentInfoService)
    : ITrackingService<TPersistentDomainObjectBase>
{
    /// <inheritdoc />
    public TrackingResult<TDomainObject> GetChanges<TDomainObject>(TDomainObject value, GetChangesMode mode = GetChangesMode.Default)
            where TDomainObject : TPersistentDomainObjectBase
    {
        if (this.GetPersistentState(value) == PersistentLifeObjectState.NotPersistent)
        {
            return TrackingResult.Create(persistentInfoService, value, mode);
        }

        var trackingProperties = this.GetModifiedObjectStates(value).Select(z => new TrackingProperty(z.PropertyName, z.PreviousValue, z.CurrentValue));

        return new TrackingResult<TDomainObject>(trackingProperties);
    }

    /// <inheritdoc />
    public PersistentLifeObjectState GetPersistentState<TDomainObject>(TDomainObject value)
            where TDomainObject : TPersistentDomainObjectBase
    {
        return objectStatesService.IsNew(value)
                       ? PersistentLifeObjectState.NotPersistent
                       : objectStatesService.IsRemoving(value) ? PersistentLifeObjectState.MarkAsRemoved : PersistentLifeObjectState.Persistent;
    }

    /// <inheritdoc />
    public ChangingLifeObjectState GetChangingState<TDomainObject>(TDomainObject value)
            where TDomainObject : TPersistentDomainObjectBase
    {
        if (this.GetPersistentState(value) == PersistentLifeObjectState.NotPersistent)
        {
            return ChangingLifeObjectState.Changing;
        }

        return this.GetModifiedObjectStates(value).Any() ? ChangingLifeObjectState.Changing : ChangingLifeObjectState.Original;
    }

    /// <inheritdoc />
    public TProperty GetPrevOrCurrentValue<TDomainObject, TProperty>(TDomainObject domainObject, Expression<Func<TDomainObject, TProperty>> propertyExpression)
            where TDomainObject : TPersistentDomainObjectBase
    {
        var defaultValue = propertyExpression.Compile()(domainObject);

        return this.GetPrevValue(domainObject, propertyExpression, defaultValue);
    }

    /// <inheritdoc />
    public TProperty GetPrevValue<TDomainObject, TProperty>(TDomainObject domainObject, Expression<Func<TDomainObject, TProperty>> propertyExpression, TProperty defaultValue)
            where TDomainObject : TPersistentDomainObjectBase
    {
        var changes = this.GetChanges(domainObject);

        return changes.GetPrevValue(propertyExpression, defaultValue);
    }


    private IEnumerable<ObjectState> GetModifiedObjectStates<TDomainObject>(TDomainObject value)
            where TDomainObject : TPersistentDomainObjectBase
    {
        return objectStatesService.GetModifiedObjectStates(value) ?? Enumerable.Empty<ObjectState>();
    }
}
