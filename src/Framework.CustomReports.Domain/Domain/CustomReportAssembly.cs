using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Core;

namespace Framework.CustomReports.Domain
{
    public class CustomReportAssembly
    {
        private readonly IList<Type> customReportTypes;

        private readonly IList<Type> customReportBLLTypes;

        public CustomReportAssembly(IList<Type> customReportTypes, IList<Type> customReportBllTypes)
        {
            this.customReportTypes = customReportTypes;
            this.customReportBLLTypes = customReportBllTypes;
        }

        public CustomReportAssembly() : this(new List<Type>(), new List<Type>())
        {
        }

        public IEnumerable<Type> CustomReportTypes => this.customReportTypes;

        public IEnumerable<Type> CustomReportBllTypes => this.customReportBLLTypes;

        public CustomReportAssembly WithDomainAssembly(Assembly assembly)
        {
            this.customReportTypes
                .AddRange(assembly
                    .GetExportedTypes()
                    .Where(z => z.GetInterfaces().Where(q => q.IsGenericType).Any(q => q.GetGenericTypeDefinition() == typeof(ICustomReport<>)))
                    .Where(z => !z.IsAbstract));

            return this;
        }

        public CustomReportAssembly WithBLLAssembly(Assembly assembly)
        {
            this.customReportBLLTypes
                .AddRange(assembly
                    .GetExportedTypes()
                    .Where(z => z.GetInterfaces().Where(q => q.IsGenericType).Any(q => q.GetGenericTypeDefinition() == typeof(ICustomReportBLL<>)))
                    .Where(z => !z.IsAbstract));

            return this;
        }
    }
}
