using CommonFramework.GenericRepository;

using SampleSystem.Domain;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public class CustomNotificationModel(IQueryableSource queryableSource, Domain.Country country)
{
    public Domain.Country Country { get; } = country;

    public int LocationsCount { get; } = queryableSource.GetQueryable<Location>().Count(x => x.Country == country);
}
