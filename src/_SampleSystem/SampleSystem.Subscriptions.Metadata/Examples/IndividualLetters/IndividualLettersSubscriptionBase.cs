using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.Examples.IndividualLetters;

/// <summary>
/// Базовая подписка примера, демонстрирующего работу признака SendIndividualLetters
/// </summary>
public abstract class IndividualLettersSubscriptionBase : Subscription<DateModel, IndividualLettersTemplate>
{
    public const string FirstToRecipient = "individualFirst@luxoft.com";

    public const string SecondToRecipient = "individualSecond@luxoft.com";

    public const string CopyRecipient = "individualCopy@luxoft.com";

    public const string ReplyToRecipient = "individualReplyTo@luxoft.com";

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<DateModel>> GetTo(IServiceProvider _, DomainObjectVersions<DateModel> versions)
    {
        yield return new($"{FirstToRecipient};{SecondToRecipient}", versions);
    }

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<DateModel>> GetCopyTo(IServiceProvider _, DomainObjectVersions<DateModel> versions)
    {
        yield return new(CopyRecipient, versions);
    }

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<DateModel>> GetReplyTo(IServiceProvider _, DomainObjectVersions<DateModel> versions)
    {
        yield return new(ReplyToRecipient, versions);
    }
}
