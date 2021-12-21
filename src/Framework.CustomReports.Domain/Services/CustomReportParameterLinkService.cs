using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using Framework.CustomReports.Domain;

namespace Framework.CustomReports.Services
{
    public class CustomReportParameterLinkService
    {
        private readonly CustomReportAssembly assembly;

        public CustomReportParameterLinkService(CustomReportAssembly assembly)
        {
            this.assembly = assembly;
        }

        public IList<CustomReportParameterLink> GetLinks()
        {
            var result = this.assembly.CustomReportTypes
                .Where(z => !z.IsAbstract)
                .Where(z => z.GetAllElements(q => q.BaseType, false).Where(q => q.IsGenericType).Any(q => q.GetGenericTypeDefinition() == typeof(CustomReportBase<,>)))
                .Select(z => new CustomReportParameterLink(z, z.GetGenericArguments(typeof(CustomReportBase<,>))[1]))
                .ToList();

            var withParameterDuplicate = result.GroupBy(z => z.ParameterType).Where(z => z.Skip(1).Any()).ToList();

            if (withParameterDuplicate.Any())
            {
                throw new ArgumentException(
                    $"Custom report system can't supported custom reports with same parameter types. Duplicated parameters: '{string.Join(",", withParameterDuplicate.Select(z => z.Key.FullName))}'");
            }

            return result;
        }
    }
}