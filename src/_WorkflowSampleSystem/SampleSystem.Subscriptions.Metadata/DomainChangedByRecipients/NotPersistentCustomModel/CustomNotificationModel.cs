using System.Linq;
using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel
{
    public class CustomNotificationModel
    {
        public CustomNotificationModel(ISampleSystemBLLContext context, Domain.Country country)
        {
            this.Country = country;

            this.LocationsCount = context.Logics.Location.GetUnsecureQueryable().Count(x => x.Country == country);
        }

        public Domain.Country Country { get; }

        public int LocationsCount { get; }
    }
}
