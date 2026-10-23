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

public class NotificationEmailExtractor<TEmployee>(
    INotificationPrincipalNameExtractor notificationPrincipalNameExtractor,
    IVisualIdentityInfo<TEmployee> employeeVisualIdentityInfo,
    IQueryableSource queryableSource,
    EmployeeInfo<TEmployee> employeeInfo) : INotificationEmailExtractor
    where TEmployee : class
{
    public IAsyncEnumerable<string> GetEmails(DomainSecurityRule.RoleBaseSecurityRule securityRule, ImmutableArray<NotificationFilterGroup> notificationFilterGroups) =>
        this.GetEmailsInternal(securityRule, notificationFilterGroups);

    private async IAsyncEnumerable<string> GetEmailsInternal(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        ImmutableArray<NotificationFilterGroup> notificationFilterGroups,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var principalNames = await notificationPrincipalNameExtractor.GetPrincipalNamesAsync(securityRule, notificationFilterGroups).ToHashSetAsync(null, ct);

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
