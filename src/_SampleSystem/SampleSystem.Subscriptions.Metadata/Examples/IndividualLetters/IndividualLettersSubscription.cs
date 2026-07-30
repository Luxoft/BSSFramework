using System.Net.Mail;

namespace SampleSystem.Subscriptions.Metadata.Examples.IndividualLetters;

/// <summary>
/// Пример подписки с отправкой индивидуального письма каждому адресату
/// </summary>
public class IndividualLettersSubscription : IndividualLettersSubscriptionBase
{
    public override MailAddress Sender { get; } = new("IndividualLetters@luxoft.com", "IndividualLetters");

    public override bool SendIndividualLetters { get; } = true;
}
