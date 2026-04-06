using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain;

using SecuritySystem.Notification.Domain;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

public class EmployeeUpdateSubscription : ISubscription<Domain.Employee>
{
    public IEnumerable<TypedNotificationFilterGroup> GetTypedNotificationFilterGroups(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new TypedNotificationFilterGroup<ManagementUnit>
                     {
                         ExpandType = NotificationExpandType.All, SecurityContextList = []
                     };
    }

    public IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetCopyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetReplyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new ("replayTo@luxoft.com", versions);
    }
}
