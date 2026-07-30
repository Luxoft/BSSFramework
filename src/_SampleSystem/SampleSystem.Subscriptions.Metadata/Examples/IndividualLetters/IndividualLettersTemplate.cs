using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.Examples.IndividualLetters;

public class IndividualLettersTemplate : RazorTemplate<DateModel>
{
    public override string Subject => $"Individual letters example for {this.Current!.Year}";

    public override void Execute() => this.Writer.Write($"<h2>Year: {this.Current!.Year}</h2>");
}
