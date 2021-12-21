using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Framework.Configuration.Domain.Reports;
using Framework.Core;
using Framework.CustomReports.Domain;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.SerializeMetadata;
using Framework.Exceptions;
using Framework.Security;
using Framework.SecuritySystem;

using CustomAttributeProviderExtensions = Framework.Core.CustomAttributeProviderExtensions;

namespace Framework.CustomReports.Services
{
    public static class Extenstions
    {
        internal static bool HasViewSecurityAttributes(this IList<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[] propertyInfoChain)> source)
        {
            return source.Where(z => z.propertyMetadataChain.Any(q => q.IsSecurity))
                         .SelectMany(z => z.propertyInfoChain)
                         .Any(z => CustomAttributeProviderExtensions.GetCustomAttributes<ViewDomainObjectAttribute>(z).Any());
        }

        internal static bool HasVirtualProperty(this IList<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[] propertyInfoChain)> source)
        {
            return source
                   .Where(z => z.propertyMetadataChain.Any(q => q.IsVirtual))
                   .Select(z => z.propertyInfoChain)
                   .Any(z => z.Any(q => !q.HasPrivateField()));
        }

        internal static bool HasVirtualFilter<TDomain>(this Framework.Configuration.Domain.Reports.Report report, SystemMetadata metadata)
        {
            var startDomain = metadata.GetDomainType(new TypeHeader(report.DomainTypeName));

            return report.Filters.Any(z => typeof(TDomain).ToPropertyInfoChain(startDomain.GetPropertyMetadataChain(z.Property, metadata).Select(q => q.Name).ToArray()).Any(q => !q.HasPrivateField()));
        }

        internal static IList<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[] propertyInfoChain)> GetReportPropertyToMetadataLinks<TDomainObject>(this Framework.Configuration.Domain.Reports.Report report, SystemMetadata metadata)
        {
            var startDomain = metadata.GetDomainType(new TypeHeader(report.DomainTypeName));

            return report.Properties
                         .Select(z => (
                                          reportProperty: z,
                                          propertyMetadataChain: startDomain.GetPropertyMetadataChain(z.PropertyPath, metadata).ToArray()
                                        ))
                         .Select(z =>
                                     (
                                        reportProperty: z.reportProperty,
                                        propertyMetadataChain: z.propertyMetadataChain,
                                        propertyInfoChain: typeof(TDomainObject).ToPropertyInfoChain(z.propertyMetadataChain.Select(q => q.Name).ToArray()).ToArray()
                                        ))
                         .ToList();
        }

        public static TSecurityOperationCode GetSecurityOperationCode<TBLLContext, TSecurityOperationCode>(
            this ISecurityOperationCodeProvider<TSecurityOperationCode> securityOperationCodeProvider,
            Guid reportIdent, TBLLContext context)
            where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>
        {
            var reportValue =
                context.Configuration.Logics.Report.GetUnsecureQueryable()
                    .Where(z => z.Id == reportIdent)
                    .Select(z => new ReportValueSource { Name = z.Name, SecurityOperationCode = z.SecurityOperationCode, DomainTypeName = z.DomainTypeName })
                    .FirstOrDefault();

            if (null == reportValue)
            {
                throw new ObjectByIdNotFoundException<Guid>(typeof(Configuration.Domain.Reports.Report), reportIdent);
            }

            return securityOperationCodeProvider.GetSecurityOperationCode(reportValue, context);
        }

        public static TSecurityOperationCode GetSecurityOperationCode<TBLLContext, TSecurityOperationCode>(
            this ISecurityOperationCodeProvider<TSecurityOperationCode> securityOperationCodeProvider,
            Configuration.Domain.Reports.Report report, TBLLContext context)
            where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>
        {
            return securityOperationCodeProvider.GetSecurityOperationCode(new ReportValueSourceAdapter(report), context);
        }



        private static TSecurityOperationCode GetSecurityOperationCode<TBLLContext, TSecurityOperationCode>(this ISecurityOperationCodeProvider<TSecurityOperationCode> securityOperationCodeProvider, IReportValueSource reportValue, TBLLContext context)
            where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>

        {
            if (reportValue.SecurityOperationCode.HasValue)
            {
                if (Enum.IsDefined(typeof(TSecurityOperationCode), reportValue.SecurityOperationCode.Value))
                {
                    var result = (TSecurityOperationCode)((object)reportValue.SecurityOperationCode.Value);

                    return result;
                }
                else
                {
                    throw new ArgumentException($"SecurityOperation:'{nameof(TSecurityOperationCode)}' has no code:'{reportValue.SecurityOperationCode.Value}. Which definded in report:'{reportValue.Name}''");
                }

            }

            var domainType = context.Configuration.Logics.DomainType.GetUnsecureQueryable()
                .First(q => reportValue.DomainTypeName == q.Name);

            var type = context.Configuration.ComplexDomainTypeResolver.Resolve(domainType);

            return securityOperationCodeProvider.GetByDomain(type, BLLSecurityMode.View);
        }


        private interface IReportValueSource
        {
            int? SecurityOperationCode { get; }
            string DomainTypeName { get; }

            string Name { get; }
        }

        private class ReportValueSource : IReportValueSource
        {
            public int? SecurityOperationCode { get; set; }
            public string DomainTypeName { get; set; }
            public string Name { get; set; }
        }

        private class ReportValueSourceAdapter : IReportValueSource
        {
            private readonly Configuration.Domain.Reports.Report report;

            public ReportValueSourceAdapter(Configuration.Domain.Reports.Report report)
            {
                this.report = report;
            }

            public int? SecurityOperationCode => this.report.SecurityOperationCode;
            public string DomainTypeName => this.report.DomainTypeName;
            public string Name => this.report.Name;
        }
    }
}
