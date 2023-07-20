using System.Linq.Expressions;

using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

public class TypedAthNotificationPrincipalExtractor : INotificationPrincipalExtractor
{
    public Expression<Func<Permission, bool>> GetRoleBaseNotificationFilter(Guid[] roleIdents) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> notificationFilterGroups) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRelatedRole(Guid[] roleIdents, IEnumerable<string> principalNames, Guid relatedRoleId) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByOperations(Guid[] operationsIds, IEnumerable<NotificationFilterGroup> notificationFilterGroups) => throw new NotImplementedException();
}
