using System.Collections.Generic;

using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public partial interface ICodeFirstSubscriptionBLL
{
    IEnumerable<string> GetActiveCodeFirstSubscriptionCodes();

    void Save(IEnumerable<CodeFirstSubscription> subscriptions);
}
