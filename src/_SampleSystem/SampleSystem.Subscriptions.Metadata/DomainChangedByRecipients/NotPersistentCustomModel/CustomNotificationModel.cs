using Anch.GenericRepository;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain.Directories;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public class CustomNotificationModel(IServiceProvider serviceProvider, Domain.Directories.Country country)
{
    public Domain.Directories.Country Country { get; } = country;

    public int LocationsCount { get; } = serviceProvider.GetRequiredService<IQueryableSource>().GetQueryable<Location>().Count(x => x.Country == country);
}
