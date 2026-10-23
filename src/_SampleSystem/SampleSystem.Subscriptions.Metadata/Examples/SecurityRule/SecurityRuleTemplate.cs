using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.Examples.SecurityRule;

public class SecurityRuleTemplate : RazorTemplate<NoRecipientModel>
{
    public override string Subject => $"Security rule example for {this.Current!.Year}";

    public override void Execute() => this.Writer.Write($"<h2>Year: {this.Current!.Year}</h2>");
}
