using System.Linq.Expressions;

namespace Framework.DomainDriven.Tracking;

/// <summary>
/// Define tracking serice interface. Tracking service is a service that helps track domain object changes during DB session.
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public interface ITrackingService<in TPersistentDomainObjectBase>
{
    /// <summary>
    /// Gets all domain object's changes.
    /// </summary>
    /// <typeparam name="TDomainObject">Domain object type</typeparam>
    /// <param name="value">Domain object instance</param>
    /// <param name="mode">Mode that defines changes detection algorithm on not persistent objects (that not exist in DB)</param>
    /// <returns>Domain object changes object (collection)</returns>
    TrackingResult<TDomainObject> GetChanges<TDomainObject>(TDomainObject value, GetChangesMode mode = GetChangesMode.Default)
            where TDomainObject : TPersistentDomainObjectBase;

    /// <summary>
    /// Gets  a domain object's Persistent state, i.e. it can be new (not in DB), persistent (already in DB) or marked to remove from DB after session close.
    /// </summary>
    /// <typeparam name="TDomainObject">Domain object type</typeparam>
    /// <param name="value">Domain object instance</param>
    /// <returns><see cref="PersistentLifeObjectState"/> enum that defines persistent state</returns>
    PersistentLifeObjectState GetPersistentState<TDomainObject>(TDomainObject value)
            where TDomainObject : TPersistentDomainObjectBase;

    /// <summary>
    ///     Gets a domain object's changing state <see cref="ChangingLifeObjectState" /> that defines whether the instance
    ///     changed or not in the DB session.
    /// </summary>
    /// <typeparam name="TDomainObject">Domain object type</typeparam>
    /// <param name="value">Domain object instance</param>
    /// <returns>Changing state value</returns>
    ChangingLifeObjectState GetChangingState<TDomainObject>(TDomainObject value)
            where TDomainObject : TPersistentDomainObjectBase;

    /// <summary>
    ///     Gets previous or current value that returns a domain object property specified by expression passed
    /// </summary>
    /// <typeparam name="TDomainObject">Domain object type</typeparam>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="domainObject">Domain object instance to get property of</param>
    /// <param name="propertyExpression">Property get expression</param>
    /// <returns>Previous or current property value identified by expression specified</returns>
    TProperty GetPrevOrCurrentValue<TDomainObject, TProperty>(TDomainObject domainObject, Expression<Func<TDomainObject, TProperty>> propertyExpression)
            where TDomainObject : TPersistentDomainObjectBase;

    /// <summary>
    ///     Gets previous or default value that returns a domain object property specified by expression passed
    /// </summary>
    /// <typeparam name="TDomainObject">Domain object type</typeparam>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <param name="domainObject">Domain object instance to get property of</param>
    /// <param name="propertyExpression">Property get expression</param>
    /// <param name="defaultValue">Default property value if no previous value found</param>
    /// <returns>Previous or default property value identified by expression specified</returns>
    TProperty GetPrevValue<TDomainObject, TProperty>(TDomainObject domainObject, Expression<Func<TDomainObject, TProperty>> propertyExpression, TProperty defaultValue)
            where TDomainObject : TPersistentDomainObjectBase;
}
