using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.Core;

namespace Framework.Configuration.BLL;

public partial class CodeFirstSubscriptionBLL
{
    public IEnumerable<string> GetActiveCodeFirstSubscriptionCodes()
    {
        var result = this.GetUnsecureQueryable()
                         .Where(s => s.Active)
                         .Select(s => s.Code);

        return result;
    }

    public void Save(IEnumerable<CodeFirstSubscription> subscriptions)
    {
        var allCodes = this.GetUnsecureQueryable().Select(s => s.Code).ToArray();
        var nonStoredSubscriptions = subscriptions.Where(s => !allCodes.Contains(s.Code));

        nonStoredSubscriptions.Foreach(this.Save);
    }
}
