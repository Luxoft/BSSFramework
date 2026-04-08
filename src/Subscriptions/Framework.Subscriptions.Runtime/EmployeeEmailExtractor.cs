using System.Collections.Immutable;

using CommonFramework;
using CommonFramework.GenericRepository;
using CommonFramework.VisualIdentitySource;

using Framework.Subscriptions.Domain;

using GenericQueryable;

using SecuritySystem;
using SecuritySystem.Notification;
using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions;

public class EmployeeEmailExtractor<TEmployee, TPrincipal>(
    INotificationPrincipalExtractor<TPrincipal> notificationPrincipalExtractor,
    IVisualIdentityInfo<TPrincipal> principalVisualIdentityInfo,
    IVisualIdentityInfo<TEmployee> employeeVisualIdentityInfo,
    IQueryableSource queryableSource,
    EmployeeInfo<TEmployee> employeeInfo,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null) : IEmployeeEmailExtractor
    where TEmployee : class
{
    public ImmutableHashSet<string> GetEmails(ImmutableArray<SecurityRole> securityRoles, ImmutableArray<NotificationFilterGroup> notificationFilterGroups) =>

        defaultCancellationTokenSource.RunSync(async ct =>
        {
            var principalNames = await notificationPrincipalExtractor.GetPrincipalsAsync(securityRoles, notificationFilterGroups)
                                                                     .Select(principalVisualIdentityInfo.Name.Getter)
                                                                     .ToHashSetAsync(null, ct);

            return await queryableSource.GetQueryable<TEmployee>()
                                        .Where(employeeVisualIdentityInfo.Name.Path.Select(employeeName => principalNames.Contains(employeeName)))
                                        .Select(employeeInfo.Email.Path)
                                        .GenericAsAsyncEnumerable()
                                        .ToImmutableHashSetAsync(ct);
        });
}
