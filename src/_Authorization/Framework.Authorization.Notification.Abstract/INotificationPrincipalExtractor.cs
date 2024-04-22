﻿using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.Notification;

public interface INotificationPrincipalExtractor
{
    IEnumerable<Principal> GetNotificationPrincipalsByRoles(SecurityRole[] securityRole, IEnumerable<NotificationFilterGroup> notificationFilterGroups);
}
