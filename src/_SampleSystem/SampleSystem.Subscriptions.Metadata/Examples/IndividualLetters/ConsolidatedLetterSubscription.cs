using System.Net.Mail;

namespace SampleSystem.Subscriptions.Metadata.Examples.IndividualLetters;

/// <summary>
/// Example of a subscription sending one consolidated letter to all addressees
/// </summary>
public class ConsolidatedLetterSubscription : IndividualLettersSubscriptionBase
{
    public override MailAddress Sender { get; } = new("ConsolidatedLetter@luxoft.com", "ConsolidatedLetter");
}
