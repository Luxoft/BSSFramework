using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL.Tracking;

/// <summary>
/// Represents tracking serivice. Tracking service helps track domain object changes during DB session
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public class TrackingService<TPersistentDomainObjectBase> : ITrackingService<TPersistentDomainObjectBase>
{
    private readonly IObjectStateService objectStatesService;

    private readonly IPersistentInfoService persistentInfoService;

    /// <summary>
    /// Creates new tracking service instance
    /// </summary>
    /// <param name="objectStatesService">Object states service implementation</param>
    public TrackingService(IObjectStateService objectStatesService, IPersistentInfoService persistentInfoService)
    {
        this.objectStatesService = objectStatesService ?? throw new ArgumentNullException(nameof(objectStatesService));
        this.persistentInfoService = persistentInfoService;
    }

    /// <inheritdoc />
    public TrackingResult<TDomainObject> GetChanges<TDomainObject>(TDomainObject value, GetChangesMode mode = GetChangesMode.Default)
            where TDomainObject : TPersistentDomainObjectBase
    {
        if (this.GetPersistentState(value) == PersistentLifeObjectState.NotPersistent)
        {
            return TrackingResult.Create(this.persistentInfoService, value, mode);
        }

        var trackingProperties = this.GetModifiedObjectStates(value).Select(z => new TrackingProperty(z.PropertyName, z.PreviusValue, z.CurrentValue));

        return new TrackingResult<TDomainObject>(trackingProperties);
    }

    /// <inheritdoc />
    public PersistentLifeObjectState GetPersistentState<TDomainObject>(TDomainObject value)
            where TDomainObject : TPersistentDomainObjectBase
    {
        return this.objectStatesService.IsNew(value)
                       ? PersistentLifeObjectState.NotPersistent
                       : this.objectStatesService.IsRemoving(value) ? PersistentLifeObjectState.MarkAsRemoved : PersistentLifeObjectState.Persistent;
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

    [NotNull]
    private IEnumerable<ObjectState> GetModifiedObjectStates<TDomainObject>(TDomainObject value)
            where TDomainObject : TPersistentDomainObjectBase
    {
        return this.objectStatesService.GetModifiedObjectStates(value) ?? Enumerable.Empty<ObjectState>();
    }
}
