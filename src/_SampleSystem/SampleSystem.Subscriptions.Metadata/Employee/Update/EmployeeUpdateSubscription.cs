using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain;

using SecuritySystem.Notification.Domain;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

public class EmployeeUpdateSubscription : ISubscription<Domain.Employee>
{
    public IEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new TypedNotificationFilterGroup<ManagementUnit>
                     {
                         ExpandType = NotificationExpandType.All, SecurityContextList = [], SecurityContextType = typeof(ManagementUnit)
                     };
    }

    public IEnumerable<NotificationMessageGenerationInfo> GetTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions.Previous, versions.Current);
    }

    public IEnumerable<NotificationMessageGenerationInfo> GetCopyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions.Previous, versions.Current);
    }

    public IEnumerable<NotificationMessageGenerationInfo> GetReplyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new ("replayTo@luxoft.com", versions.Previous, versions.Current);
    }
}
