using System;
using System.Collections.Generic;

using Framework.Authorization.Domain;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL
{
    public partial interface IPermissionBLL
    {
        [NotNull]
        IEnumerable<Principal> GetNotificationPrincipalsByRoles([NotNull]Guid[] roleIdents);

        [NotNull] IEnumerable<Principal> GetNotificationPrincipalsByRoles([NotNull] Guid[] roleIdents, [NotNull]IEnumerable<NotificationFilterGroup> notificationFilterGroups);

        /// <summary>
        /// Получить все принципалы у которых есть доступ к роли <paramref name="relatedRoleId"/>
        /// для всех EntityType к которым есть доступ у <paramref name="principalNames"/> для ролей <paramref name="roleIdents"/>
        /// </summary>
        /// <param name="roleIdents">Список ролей по которым ищутся пермишены у переданных принципалов</param>
        /// <param name="principalNames">Список принципалов по которым получается список Entity с учетом <paramref name="roleIdents"/></param>
        /// <param name="relatedRoleId">Роль которая должна быть у результирующих принципалов</param>
        /// <returns></returns>
        [NotNull]
        IEnumerable<Principal> GetNotificationPrincipalsByRelatedRole([NotNull] Guid[] roleIdents, [NotNull] IEnumerable<string> principalNames, Guid relatedRoleId);

        [NotNull]
        IEnumerable<Principal> GetNotificationPrincipalsByOperations([NotNull] Guid[] operationsIds, [NotNull]IEnumerable<NotificationFilterGroup> notificationFilterGroups);
    }
}
