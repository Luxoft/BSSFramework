using Framework.Configuration.SubscriptionModeling;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerTemplateImpl;

public class RazorTemplateImpl : RazorTemplate<Domain.Employee>
{
    public override string Subject => this.Current.NameNative.FirstName + " loves string.concat";

    public override void Execute()
    {
        this.Output.Write($"String.Concat it is good choice for {this.Current.NameNative.FullName.Trim()}.");
    }
}
