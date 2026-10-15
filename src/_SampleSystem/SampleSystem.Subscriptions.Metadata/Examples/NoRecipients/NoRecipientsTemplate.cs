using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.Examples.NoRecipients;

public class NoRecipientsTemplate : RazorTemplate<NoRecipientModel>
{
    public override string Subject => $"No recipients example for {this.Current!.Year}";

    public override void Execute() => this.Writer.Write($"<h2>Year: {this.Current!.Year}</h2>");
}
