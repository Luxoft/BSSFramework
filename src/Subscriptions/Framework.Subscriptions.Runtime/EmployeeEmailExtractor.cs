using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using Anch.Core;
using Anch.GenericQueryable;
using Anch.GenericRepository;
using Anch.SecuritySystem;
using Anch.SecuritySystem.Notification;
using Anch.SecuritySystem.Notification.Domain;
using Anch.VisualIdentitySource;

namespace Framework.Subscriptions;

public class EmployeeEmailExtractor<TEmployee, TPrincipal>(
    INotificationPrincipalExtractor<TPrincipal> notificationPrincipalExtractor,
    IVisualIdentityInfo<TPrincipal> principalVisualIdentityInfo,
    IVisualIdentityInfo<TEmployee> employeeVisualIdentityInfo,
    IQueryableSource queryableSource,
    EmployeeInfo<TEmployee> employeeInfo) : IEmployeeEmailExtractor
    where TEmployee : class
{
    public IAsyncEnumerable<string> GetEmails(ImmutableArray<SecurityRole> securityRoles, ImmutableArray<NotificationFilterGroup> notificationFilterGroups) =>
        this.GetEmailsInternal(securityRoles, notificationFilterGroups);

    private async IAsyncEnumerable<string> GetEmailsInternal(
        ImmutableArray<SecurityRole> securityRoles,
        ImmutableArray<NotificationFilterGroup> notificationFilterGroups,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var principalNames = await notificationPrincipalExtractor.GetPrincipalsAsync(securityRoles, notificationFilterGroups)
                                                                 .Select(principalVisualIdentityInfo.Name.Getter)
                                                                 .ToHashSetAsync(null, ct);

        await foreach (var email in queryableSource.GetQueryable<TEmployee>()
                                                   .Where(employeeVisualIdentityInfo.Name.Path.Select(employeeName => principalNames.Contains(employeeName)))
                                                   .Select(employeeInfo.Email.Path)
                                                   .GenericAsAsyncEnumerable()
                                                   .WithCancellation(ct))
        {
            yield return email;
        }
    }
}
