using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions.Subscriptions;

/// <summary>
///     Преобразователь экземпляров типа <see cref="ISubscriptionMetadata" />.
///     в экземпляры типа <see cref="Subscription" />.
/// </summary>
public class SubscriptionMetadataMapper
{
    private static readonly Dictionary<DomainObjectChangeType, Tuple<bool?, bool?>> Requirements = new()
                                                                                                   {
                { DomainObjectChangeType.Any, Tuple.Create<bool?, bool?>(null, null) },

                { DomainObjectChangeType.Create, Tuple.Create<bool?, bool?>(false, true) },

                { DomainObjectChangeType.Update, Tuple.Create<bool?, bool?>(true, true) },

                { DomainObjectChangeType.Delete, Tuple.Create<bool?, bool?>(true, false) },

                { DomainObjectChangeType.CreateOrUpdate, Tuple.Create<bool?, bool?>(null, true) },

                { DomainObjectChangeType.UpdateOrDelete, Tuple.Create<bool?, bool?>(true, null) }
        };

    /// <summary>
    ///     Преобразует экземпляр типа <see cref="ISubscriptionMetadata" /> (Code first subscription)
    ///     в экземпляр типа <see cref="Subscription" />.
    /// </summary>
    /// <param name="metadata">Описание подписки.</param>
    /// <returns>Экземпляр <see cref="Subscription" />, созданный на основе описания</returns>
    /// <exception cref="System.ArgumentNullException">Параметр <paramref name="metadata" /> равен null.</exception>
    public virtual Subscription Map(ISubscription metadata)
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
        subscription.RecipientsMode = metadata.RecipientMergeType;
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

    private static void MapSubBusinessRoles(ISubscription metadata, Subscription subscription)
    {
        if (metadata.SecurityRoles == null)
        {
            return;
        }

        foreach (var securityRole in metadata.SecurityRoles)
        {
            var role = new SubBusinessRole(subscription) { SecurityRole = securityRole };
        }
    }

    private static void MapSecurityItems(ISubscription metadata, Subscription subscription)
    {
        foreach (var lambda in metadata.GetSecurityItems())
        {
            var securityItem = new SubscriptionSecurityItem(subscription)
                               {
                                   Source = MapLambda(lambda), ExpandType = lambda.ExpandType
                               };

            securityItem.Source.AuthDomainType = lambda.AuthDomainType;
        }
    }

    private static SubscriptionLambda? MapLambda(ILambdaMetadata? metadata)
    {
        if (metadata == null)
        {
            return null;
        }

        var lambda = new SubscriptionLambda { FuncValue = metadata.Lambda, MetadataSourceType = metadata.GetType() };

        var requirements = Requirements[metadata.DomainObjectChangeType];

        lambda.RequiredModePrev = requirements.Item1;
        lambda.RequiredModeNext = requirements.Item2;

        return lambda;
    }
}
