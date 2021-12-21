using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL.Security;

using JetBrains.Annotations;

namespace Framework.CustomReports.Domain
{
    public class AccessReportRight : IAccessReportRight
    {
        private readonly IList<string> principals = new List<string>();
        private readonly IList<Guid> operations = new List<Guid>();
        private readonly IList<Guid> roles = new List<Guid>();

        public AccessReportRight()
        {
        }

        public AccessReportRight ForPrincipals([NotNull] params string[] principalsIn)
        {
            if (principalsIn == null)
            {
                throw new ArgumentNullException(nameof(principalsIn));
            }

            this.principals.AddRange(principalsIn);

            return this;
        }

        public AccessReportRight ForOperations<TSecurityOperationCode>([NotNull] params TSecurityOperationCode[] operationsIn)
            where TSecurityOperationCode : struct, Enum
        {
            if (operationsIn == null)
            {
                throw new ArgumentNullException(nameof(operationsIn));
            }

            this.operations.AddRange(operationsIn.Select(z => z.ToGuid<TSecurityOperationCode>()));

            return this;
        }

        public AccessReportRight ForRoles([NotNull] params Guid[] rolesIn)
        {
            if (rolesIn == null)
            {
                throw new ArgumentNullException(nameof(rolesIn));
            }

            this.roles.AddRange(rolesIn);

            return this;
        }

        public IEnumerable<string> Principals => this.principals;

        public IEnumerable<Guid> Operations => this.operations;

        public IEnumerable<Guid> Roles => this.roles;
    }
}
