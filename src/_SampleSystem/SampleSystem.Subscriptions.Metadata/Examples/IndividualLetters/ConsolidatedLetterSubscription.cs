using System.Net.Mail;

namespace SampleSystem.Subscriptions.Metadata.Examples.IndividualLetters;

/// <summary>
/// Пример подписки с отправкой одного консолидированного письма на всех адресатов
/// </summary>
public class ConsolidatedLetterSubscription : IndividualLettersSubscriptionBase
{
    public override MailAddress Sender { get; } = new("ConsolidatedLetter@luxoft.com", "ConsolidatedLetter");
}
