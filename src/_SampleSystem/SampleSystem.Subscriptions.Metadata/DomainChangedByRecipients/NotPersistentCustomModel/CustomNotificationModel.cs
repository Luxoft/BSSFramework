using CommonFramework.GenericRepository;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public class CustomNotificationModel(IServiceProvider serviceProvider, Domain.Country country)
{
    public Domain.Country Country { get; } = country;

    public int LocationsCount { get; } = serviceProvider.GetRequiredService<IQueryableSource>().GetQueryable<Location>().Count(x => x.Country == country);
}
