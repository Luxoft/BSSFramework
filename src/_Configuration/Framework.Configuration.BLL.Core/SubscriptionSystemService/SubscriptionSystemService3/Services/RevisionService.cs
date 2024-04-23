using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Core;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Services;

/// <summary>
///     Служба для поиска версий доменного объекта и связанной с версиями информации.
/// </summary>
/// <typeparam name="T">Тип доменного объекта.</typeparam>
public class RevisionService<T>
        where T : class, IIdentityObject<Guid>
{
    private readonly IRevisionBLL<T, Guid> revisionBll;

    private readonly ConfigurationContextFacade _configurationContextFacade;

    private readonly SubscriptionResolver subscriptionResolver;

    /// <summary>Создаёт экземпляр класса <see cref="RevisionService{T}" />.</summary>
    /// <param name="revisionBll">Системная служба поиска версий доменного объекта.</param>
    /// <param name="configurationContextFacade">Фасад контекста конфигурации.</param>
    /// <param name="subscriptionResolver">Экземпляр компонента, предназначенного для поиска подписок.</param>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент <paramref name="revisionBll" /> или
    ///     <paramref name="configurationContextFacade" /> равен null.
    /// </exception>
    public RevisionService(
            IRevisionBLL<T, Guid> revisionBll,
            ConfigurationContextFacade configurationContextFacade,
            SubscriptionResolver subscriptionResolver)
    {
        this.revisionBll = revisionBll ?? throw new ArgumentNullException(nameof(revisionBll));
        this._configurationContextFacade = configurationContextFacade ?? throw new ArgumentNullException(nameof(configurationContextFacade));
        this.subscriptionResolver = subscriptionResolver ?? throw new ArgumentNullException(nameof(subscriptionResolver));
    }

    /// <summary>
    ///     Возвращает экземпляр <see cref="DomainObjectVersions{T}"/> по номеру версии.
    /// </summary>
    /// <param name="domainObjectId">Идентификатор доменного объекта.</param>
    /// <param name="revisionNumber">Номер версии доменного объекта.</param>
    /// <returns>
    ///     Найденный экземпляр <see cref="DomainObjectVersions{T}"/> или null,
    ///     если для найденного типа доменного объекта не найдено ни одной активной подписки.
    /// </returns>
    public virtual DomainObjectVersions<T> GetDomainObjectVersions(Guid domainObjectId, long? revisionNumber)
    {
        if (!this.subscriptionResolver.IsActiveSubscriptionForTypeExists(typeof(T)))
        {
            return null;
        }

        var prev = this.GetPreviousDomainObjectByRevisionNumber(domainObjectId, revisionNumber);
        var next = this.GetDomainObjectByRevisionNumber(domainObjectId, revisionNumber);

        if (prev == null && next == null)
        {
            throw new ArgumentException($"For DomainObject ({typeof(T).Name}) [{domainObjectId}] both states (previous and current) can't be null. Revision: {revisionNumber}");
        }

        var versions = new DomainObjectVersions<T>(prev, next);

        return versions;
    }

    /// <summary>
    ///     Возвращает данные об изменениях доменного объекта.
    /// </summary>
    /// <param name="changes">Описатель операций, проведенных над объектом в слое доступа к данным.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{ObjectModificationInfo}" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент changes равен null.</exception>
    public virtual IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes)
    {
        if (changes == null)
        {
            throw new ArgumentNullException(nameof(changes));
        }

        var group = changes.GetSubset(typeof(T)).GroupByType();

        var result = group
                     .Where(kvp => this.subscriptionResolver.IsActiveSubscriptionForTypeExists(kvp.Key))
                     .SelectMany(this.ProcessDalChanges);

        return result;
    }

    private static ObjectModificationInfo<Guid> ProcessDalChange(
            KeyValuePair<object, DALObjectChangeType> item,
            long revisionNumber,
            Type objectType)
    {
        var @object = (T)item.Key;

        return new ObjectModificationInfo<Guid>
               {
                       Identity = @object.Id,
                       Revision = revisionNumber,
                       ModificationType = item.Value.ToModificationType(),
                       TypeInfo = new TypeInfoDescription(objectType)
               };
    }

    private T GetDomainObjectByRevisionNumber(Guid domainObjectId, long? revisionNumber)
    {
        var logger = this.GetLogger();
        logger.LogInformation("Get current domain object revision by domain object id '{domainObjectId}' and revision number '{revisionNumber}'.", domainObjectId, revisionNumber?.ToString() ?? "null");

        var lastRevisionNumber = this.ResolveLastObjectRevisionNumber(domainObjectId, revisionNumber);
        var result = this.revisionBll.GetObjectByRevision(domainObjectId, lastRevisionNumber);

        logger.LogInformation("Current domain object revision '{result}' has been found by domain object id '{domainObjectId}' and revision number '{revisionNumber}'.", domainObjectId, revisionNumber?.ToString() ?? "null");

        return result;
    }

    private T GetPreviousDomainObjectByRevisionNumber(Guid domainObjectId, long? revisionNumber)
    {
        var lastRevisionNumber = this.ResolveLastObjectRevisionNumber(domainObjectId, revisionNumber);

        var previousRevisionNumber = this.revisionBll.GetPreviousRevision(domainObjectId, lastRevisionNumber);

        if (previousRevisionNumber == null)
        {
            return null;
        }

        var result = this.GetDomainObjectByRevisionNumber(domainObjectId, previousRevisionNumber);

        this.GetLogger().LogInformation("Previous domain object revision '{result}' has been found by domain object id '{domainObjectId}' and revision number '{revisionNumber}'.", result, domainObjectId, revisionNumber);

        return result;
    }

    private long ResolveLastObjectRevisionNumber(Guid domainObjectId, long? revisionNumber)
    {
        if (revisionNumber != null)
        {
            return revisionNumber.Value;
        }

        var revisions = this.revisionBll.GetObjectRevisions(domainObjectId)
                            .RevisionInfos.OrderByDescending(info => info.Date);

        var message = $"Object with id:{domainObjectId} not found. (Type:{typeof(T)})";

        var result = revisions.First(() => new InvalidOperationException(message)).RevisionNumber;

        return result;
    }

    private IEnumerable<ObjectModificationInfo<Guid>> ProcessDalChanges(
            KeyValuePair<Type, DALChanges<object>> groupItem)
    {
        var revisionNumber = this.revisionBll.GetCurrentRevision();

        if (revisionNumber == 0)
        {
            yield break;
        }

        foreach (var pair in groupItem.Value.ToChangeTypeDict())
        {
            var result = ProcessDalChange(pair, revisionNumber, groupItem.Key);

            if (result != null)
            {
                yield return result;
            }
        }
    }

    private ILogger<RevisionService<T>> GetLogger() =>
        this._configurationContextFacade.ServiceProvider.GetRequiredService<ILogger<RevisionService<T>>>();
}
