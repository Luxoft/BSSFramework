using System;

using JetBrains.Annotations;

namespace Framework.CustomReports.Domain
{
    public static class AccessReportRights
    {
        public static AccessReportRight ForPrincipals([NotNull] params string[] principals)
        {
            if (principals == null)
            {
                throw new ArgumentNullException(nameof(principals));
            }

            var result = new AccessReportRight();

            return result.ForPrincipals(principals);
        }

        public static AccessReportRight ForOperations<TSecurityOperationCode>([NotNull] params TSecurityOperationCode[] operations)
            where TSecurityOperationCode : struct, Enum
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            var result = new AccessReportRight();

            return result.ForOperations(operations);
        }

        public static AccessReportRight ForRoles([NotNull] params Guid[] roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var result = new AccessReportRight();

            return result.ForRoles(roles);
        }

        public static IAccessReportRight ForAll()
        {
            return new AccessReportRight();
        }
    }
}
