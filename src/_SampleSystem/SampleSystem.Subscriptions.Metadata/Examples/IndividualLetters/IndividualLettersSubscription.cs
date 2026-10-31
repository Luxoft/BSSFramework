using System.Net.Mail;

namespace SampleSystem.Subscriptions.Metadata.Examples.IndividualLetters;

/// <summary>
/// Example of a subscription sending an individual letter to each addressee
/// </summary>
public class IndividualLettersSubscription : IndividualLettersSubscriptionBase
{
    public override MailAddress Sender { get; } = new("IndividualLetters@luxoft.com", "IndividualLetters");

    public override bool SendIndividualLetters { get; } = true;
}
