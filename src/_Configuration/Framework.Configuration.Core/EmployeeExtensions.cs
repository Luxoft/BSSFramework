using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Persistent;

namespace Framework.Configuration;

public static class EmployeeExtensions
{
    public static IEnumerable<IEmployee> GetMergeResult(this IEnumerable<IEmployee> recepientsByRoles, IEnumerable<IEmployee> recipientsByGeneration, RecepientsSelectorMode mode)
    {
        if (recepientsByRoles == null) throw new ArgumentNullException(nameof(recepientsByRoles));
        if (recipientsByGeneration == null) throw new ArgumentNullException(nameof(recipientsByGeneration));

        var employeeComparer = EmployeeEqualityComparer.EMail;

        switch (mode)
        {
            case RecepientsSelectorMode.Union:
                return recepientsByRoles.Union(recipientsByGeneration, employeeComparer);

            case RecepientsSelectorMode.Intersect:
                return recepientsByRoles.Intersect(recipientsByGeneration, employeeComparer);

            case RecepientsSelectorMode.RolesExceptGeneration:
                return recepientsByRoles.Except(recipientsByGeneration, employeeComparer);

            case RecepientsSelectorMode.GenerationExceptRoles:
                return recipientsByGeneration.Except(recepientsByRoles, employeeComparer);

            default:
                throw new ArgumentOutOfRangeException(mode.ToString());
        }
    }
}
