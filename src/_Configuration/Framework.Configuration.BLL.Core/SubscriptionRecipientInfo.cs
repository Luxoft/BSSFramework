using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public class SubscriptionRecipientInfo
{
    public Subscription Subscription { get; set; }

    public List<string> Recipients { get; set; }
}
