using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;

using Serilog;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

/// <summary>
/// Компонент для поиска всех возможных получателей уведомлений по подписке.
/// Поиск проводится по ролям пользователей и лямбда-выражениям типа "Generation".
/// </summary>
public class RecipientsResolver<TBLLContext>
        where TBLLContext : class
{
    private readonly GenerationRecipientsResolver<TBLLContext> generationResolver;
    private readonly ByRolesRecipientsResolver<TBLLContext> rolesResolver;
    private readonly ILogger logger;

    /// <summary>Создаёт экземпляр класса <see cref="RecipientsResolver"/>.</summary>
    /// <param name="generationResolver">
    ///     Компонент, выполняющий поиск получателей уведомлений по лямбда-выражению типа "Generation".
    /// </param>
    /// <param name="rolesResolver">
    ///     Компонент, выполняющий поиск получателей уведомлений по ролям пользователей
    /// .</param>
    /// <param name="configurationContextFacade">Фасад контекста конфигурации</param>
    /// <exception cref="ArgumentNullException">Аргумент
    /// generationResolver
    /// или
    /// rolesResolver
    /// или
    /// configurationContextFacade равен null.
    /// </exception>
    public RecipientsResolver(
            GenerationRecipientsResolver<TBLLContext> generationResolver,
            ByRolesRecipientsResolver<TBLLContext> rolesResolver,
            ConfigurationContextFacade configurationContextFacade)
    {
        if (generationResolver == null)
        {
            throw new ArgumentNullException(nameof(generationResolver));
        }

        if (rolesResolver == null)
        {
            throw new ArgumentNullException(nameof(rolesResolver));
        }

        if (configurationContextFacade == null)
        {
            throw new ArgumentNullException(nameof(configurationContextFacade));
        }

        this.generationResolver = generationResolver;
        this.rolesResolver = rolesResolver;

        this.logger = Log.Logger.ForContext(this.GetType());
    }

    /// <summary>Выполняет поиск всех возможных получателей уведомлений по подписке.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="subscription">Подписка.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Коллекцию экземпляров <see cref="RecipientsResolverResult" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент
    ///     subscription
    ///     или
    ///     versions равен null.
    /// </exception>
    public virtual IEnumerable<RecipientsResolverResult> Resolve<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        if (subscription == null)
        {
            throw new ArgumentNullException(nameof(subscription));
        }

        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        this.logger.Information("Search recipients for subscription '{subscription}'", subscription);

        var generationResults = this.generationResolver.Resolve(subscription, versions);
        var rolesRecipients = this.rolesResolver.Resolve(subscription, versions);

        var generationRecipients = generationResults
                                   .SelectMany(r => r.RecipientsBag.To.Concat(r.RecipientsBag.Cc).Concat(r.RecipientsBag.ReplyTo))
                                   .ToList();

        var results = this.CreateResultsByGeneration(
                                                     subscription,
                                                     generationResults,
                                                     rolesRecipients,
                                                     new DomainObjectVersions<object>(versions.Previous, versions.Current));

        this.logger.Information(
                                "For subscription '{subscription}' '{generationRecipientsCount}' recipients by generation and '{rolesRecipients.Count()}' "
                                + "recipients by roles has been found. Recipients by generation: {generationRecipients}. Recipients by roles: {rolesRecipients}.",
                                subscription,
                                generationRecipients.Count,
                                rolesRecipients.Count(),
                                generationRecipients.Select(r => r.Email).Join(","),
                                rolesRecipients.Select(r => r.Email).Join(","));

        return results;
    }

    private static string TryGetUserEmail<T>(T domainObject)
            where T : class
    {
        return (domainObject as ICurrentUserEmailContainer)?.CurrentUserEmail ?? string.Empty;
    }

    private RecipientsResolverResult CreateResult(
            Subscription subscription,
            RecipientsResolverResult generationResult,
            RecipientCollection rolesRecipients)
    {
        var mergeResult = rolesRecipients.Merge(
                                                generationResult.RecipientsBag.To,
                                                (RecepientsMergeMode)subscription.RecepientsMode);

        var to = this.ExcludeCurrentUser(
                                         subscription,
                                         generationResult.DomainObjectVersions,
                                         mergeResult);

        var cc = this.ExcludeCurrentUser(
                                         subscription,
                                         generationResult.DomainObjectVersions,
                                         generationResult.RecipientsBag.Cc);

        var replyTo = this.ExcludeCurrentUser(
                                              subscription,
                                              generationResult.DomainObjectVersions,
                                              generationResult.RecipientsBag.ReplyTo);

        var result = new RecipientsResolverResult(
                                                  new RecipientsBag(to, cc, replyTo),
                                                  generationResult.DomainObjectVersions);

        return result;
    }

    private IEnumerable<RecipientsResolverResult> CreateResultsByGeneration(
            Subscription subscription,
            IEnumerable<RecipientsResolverResult> generationResults,
            RecipientCollection rolesRecipients,
            DomainObjectVersions<object> versions)
    {
        if (!generationResults.Any())
        {
            var bag = new RecipientsBag(
                                        this.ExcludeCurrentUser(subscription, versions, rolesRecipients),
                                        new RecipientCollection(),
                                        new RecipientCollection());

            var result = new RecipientsResolverResult(bag, versions);
            return new[] { result };
        }

        var results = generationResults.Select(r => this.CreateResult(subscription, r, rolesRecipients)).ToArray();
        return results;
    }

    private RecipientCollection ExcludeCurrentUser<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions,
            RecipientCollection recipients)
            where T : class
    {
        if (!subscription.ExcludeCurrentUser)
        {
            return recipients;
        }

        var previousEmail = TryGetUserEmail(versions.Previous);
        var currentEmail = TryGetUserEmail(versions.Current);

        var filteredRecipients = recipients
                                 .Where(r => !string.Equals(r.Email, previousEmail, StringComparison.OrdinalIgnoreCase))
                                 .Where(r => !string.Equals(r.Email, currentEmail, StringComparison.OrdinalIgnoreCase))
                                 .ToList();

        return new RecipientCollection(filteredRecipients);
    }
}
