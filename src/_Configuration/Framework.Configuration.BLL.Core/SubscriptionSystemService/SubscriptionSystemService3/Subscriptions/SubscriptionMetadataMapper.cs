using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
///     Преобразователь экземпляров типа <see cref="ISubscriptionMetadata" />.
///     в экземпляры типа <see cref="Subscription" />.
/// </summary>
public class SubscriptionMetadataMapper
{
    private static readonly Dictionary<DomainObjectChangeType, Tuple<bool?, bool?>> Requirements = new Dictionary<DomainObjectChangeType, Tuple<bool?, bool?>>
        {
                { DomainObjectChangeType.Any, Tuple.Create<bool?, bool?>(null, null) },

                { DomainObjectChangeType.Create, Tuple.Create<bool?, bool?>(false, true) },

                { DomainObjectChangeType.Update, Tuple.Create<bool?, bool?>(true, true) },

                { DomainObjectChangeType.Delete, Tuple.Create<bool?, bool?>(true, false) },

                { DomainObjectChangeType.CreateOrUpdate, Tuple.Create<bool?, bool?>(null, true) },

                { DomainObjectChangeType.UpdateOrDelete, Tuple.Create<bool?, bool?>(true, null) }
        };

    private readonly ConfigurationContextFacade configurationContextFacade;

    /// <summary>
    ///     Создаёт экземпляр класса <see cref="SubscriptionMetadataMapper" />.
    /// </summary>
    /// <param name="configurationContextFacade">Фасад контекста конфигурации.</param>
    /// <exception cref="System.ArgumentNullException">
    ///     Аргумент <paramref name="configurationContextFacade" /> равен null.
    /// </exception>
    public SubscriptionMetadataMapper(ConfigurationContextFacade configurationContextFacade)
    {
        if (configurationContextFacade == null)
        {
            throw new ArgumentNullException(nameof(configurationContextFacade));
        }

        this.configurationContextFacade = configurationContextFacade;
    }

    /// <summary>
    ///     Преобразует экземпляр типа <see cref="ISubscriptionMetadata" /> (Code first subscription)
    ///     в экземпляр типа <see cref="Subscription" />.
    /// </summary>
    /// <param name="metadata">Описание подписки.</param>
    /// <returns>Экземпляр <see cref="Subscription" />, созданный на основе описания</returns>
    /// <exception cref="System.ArgumentNullException">Параметр <paramref name="metadata" /> равен null.</exception>
    public virtual Subscription Map(ISubscriptionMetadata metadata)
    {
        if (metadata == null)
        {
            throw new ArgumentNullException(nameof(metadata));
        }

        var subscription = new Subscription();

        subscription.SenderName = metadata.SenderName;
        subscription.SenderEmail = metadata.SenderEmail;
        subscription.SendIndividualLetters = metadata.SendIndividualLetters;
        subscription.ExcludeCurrentUser = metadata.ExcludeCurrentUser;
        subscription.IncludeAttachments = metadata.IncludeAttachments;
        subscription.AllowEmptyListOfRecipients = metadata.AllowEmptyListOfRecipients;
        subscription.RazorMessageTemplateType = metadata.MessageTemplateType;
        subscription.RecepientsMode = metadata.RecepientsSelectorMode;
        subscription.MetadataSourceType = metadata.GetType();

        subscription.Condition = MapLambda(metadata.GetConditionLambda());
        subscription.Generation = MapLambda(metadata.GetGenerationLambda());
        subscription.CopyGeneration = MapLambda(metadata.GetCopyGenerationLambda());

        subscription.ReplyToGeneration = MapLambda(metadata.GetReplyToGenerationLambda());

        subscription.Attachment = MapLambda(metadata.GetAttachmentLambda());

        MapSubBusinessRoles(metadata, subscription);
        MapSecurityItems(metadata, subscription);

        subscription.MessageTemplate = new MessageTemplate();


        return subscription;
    }

    private static void MapSubBusinessRoles(ISubscriptionMetadata metadata, Subscription subscription)
    {
        if (metadata.SubBusinessRoleIds == null)
        {
            return;
        }

        foreach (var id in metadata.SubBusinessRoleIds)
        {
            var role = new SubBusinessRole(subscription);
            role.BusinessRoleId = id;
        }
    }

    private static void MapSecurityItems(ISubscriptionMetadata metadata, Subscription subscription)
    {
        var lambdas = metadata.GetSecurityItemSourceLambdas();

        if (lambdas == null)
        {
            return;
        }

        foreach (var lambda in lambdas)
        {
            var securityItem = new SubscriptionSecurityItem(subscription);
            securityItem.Source = MapLambda(lambda);
            securityItem.ExpandType = lambda.ExpandType;
            securityItem.Source.AuthDomainType = lambda.AuthDomainType;
        }
    }

    private static SubscriptionLambda MapLambda(ILambdaMetadata metadata)
    {
        if (metadata == null)
        {
            return null;
        }

        var lambda = new SubscriptionLambda();
        lambda.FuncValue = metadata.Lambda;
        lambda.MetadataSourceType = metadata.GetType();


        var requirements = Requirements[metadata.DomainObjectChangeType];

        lambda.RequiredModePrev = requirements.Item1;
        lambda.RequiredModeNext = requirements.Item2;

        return lambda;
    }
}
