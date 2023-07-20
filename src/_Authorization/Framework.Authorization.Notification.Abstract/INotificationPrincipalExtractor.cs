using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

public interface INotificationPrincipalExtractor
{
    IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents);

    IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> notificationFilterGroups);

    /// <summary>
    /// Получить все принципалы у которых есть доступ к роли <paramref name="relatedRoleId"/>
    /// для всех EntityType к которым есть доступ у <paramref name="principalNames"/> для ролей <paramref name="roleIdents"/>
    /// </summary>
    /// <param name="roleIdents">Список ролей по которым ищутся пермишены у переданных принципалов</param>
    /// <param name="principalNames">Список принципалов по которым получается список Entity с учетом <paramref name="roleIdents"/></param>
    /// <param name="relatedRoleId">Роль которая должна быть у результирующих принципалов</param>
    /// <returns></returns>
    IEnumerable<Principal> GetNotificationPrincipalsByRelatedRole(Guid[] roleIdents, IEnumerable<string> principalNames, Guid relatedRoleId);

    IEnumerable<Principal> GetNotificationPrincipalsByOperations(Guid[] operationsIds, IEnumerable<NotificationFilterGroup> notificationFilterGroups);
}
