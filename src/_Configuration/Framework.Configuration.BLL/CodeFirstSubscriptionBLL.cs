using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class CodeFirstSubscriptionBLL
{
    public IEnumerable<string> GetActiveCodeFirstSubscriptionCodes()
    {
        var result = Queryable
                           .Where<CodeFirstSubscription>(this.GetUnsecureQueryable(), s => s.Active)
                           .Select(s => s.Code);

        return result;
    }

    public void Save(IEnumerable<CodeFirstSubscription> subscriptions)
    {
        var allCodes = Queryable.Select<CodeFirstSubscription, string>(this.GetUnsecureQueryable(), s => s.Code).ToArray();
        var nonStoredSubscriptions = subscriptions.Where(s => !allCodes.Contains(s.Code));

        nonStoredSubscriptions.Foreach(this.Save);
    }
}
