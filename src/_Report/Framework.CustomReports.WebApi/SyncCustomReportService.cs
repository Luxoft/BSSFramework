using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Core;
using Framework.Configuration.Domain.Reports;
using Framework.Core;
using Framework.CustomReports.Attributes;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Tracking;
using Framework.DomainDriven.SerializeMetadata;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.CustomReports.WebApi
{
    public class SyncCustomReportService<TBLLContext, TSecurityOperationCode> : ISyncCustomReportService<TBLLContext, TSecurityOperationCode>
        where TBLLContext : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>
    {
        private readonly IContextEvaluator<TBLLContext> contextEvaluator;

        public SyncCustomReportService(ISystemMetadataTypeBuilder systemMetadataBuilder, IContextEvaluator<TBLLContext> contextEvaluator)
        {
            this.contextEvaluator = contextEvaluator;
            this.SystemMetadataBuilder = systemMetadataBuilder;
        }

        public ISystemMetadataTypeBuilder SystemMetadataBuilder { get; }

        public void Sync(IList<ICustomReport<TSecurityOperationCode>> runtimeReports)
        {
            this.contextEvaluator.Evaluate(DBSessionMode.Write, context =>
            {
                var expectedReports = runtimeReports;

                var reportBll = context.Configuration.Logics.Report;

                var actualReports = reportBll.GetUnsecureQueryable(context.Configuration.FetchService.GetContainer<Configuration.Domain.Reports.Report>(MainDTOType.RichDTO)).Where(z => z.ReportType == ReportType.Custom).ToList();

                var mergeResult = actualReports.GetMergeResult(expectedReports, z => z.Id, z => z.Id);

                foreach (var removingItem in mergeResult.RemovingItems)
                {
                    reportBll.Remove(removingItem);
                }

                var adding = mergeResult.AddingItems.Select(z => new { Source = z, Target = new Configuration.Domain.Reports.Report(z.Id) });
                var combining = mergeResult.CombineItems.Select(z => new { Source = z.Item2, Target = z.Item1 });

                var resultCombined = adding.Concat(combining).ToList();

                foreach (var combineItem in resultCombined)
                {
                    this.MapSimple(combineItem.Source, combineItem.Target);

                    if (context.Configuration.TrackingService.GetPersistentState(combineItem.Target) ==
                        PersistentLifeObjectState.NotPersistent)
                    {
                        reportBll.Insert(combineItem.Target, combineItem.Target.Id);
                    }

                    this.MapReportProperties(combineItem.Source, combineItem.Target);

                    this.MapReportFilters(combineItem.Source, combineItem.Target);

                    this.MapAccessableDetails(combineItem.Source, combineItem.Target);

                    this.MapReportParameters(combineItem.Source, combineItem.Target);

                    this.Validate(combineItem.Target.Parameters.ToList());

                    reportBll.Save(combineItem.Target);
                }
            });

        }

        private void Validate(IList<ReportParameter> parameters)
        {
            var expeptionMessage = parameters
                .Where(z => this.SystemMetadataBuilder
                    .SystemMetadata.Types.Where(t => t.Role.HasSubset())
                    .CastStrong<TypeMetadata, DomainTypeMetadata>()
                    .GetById(new TypeHeader(z.TypeName), false)?.Role == TypeRole.Domain)
                .Where(z => string.IsNullOrWhiteSpace(z.GetVisualIdentityPropertyName(this.SystemMetadataBuilder)))
                .Select(z => $"Report:'{z.Report.Name}' has parameter:'{z.Name}' with type:'{z.TypeName}' and has not actual displayProperty. Actual display property is:'{z.DisplayValueProperty}'. Please use [{nameof(ReportParameterVisualIdentityAttribute)}] on the parameterProperty or [{nameof(VisualIdentityAttribute)}] on the domainProperty.")
                .Join(Environment.NewLine);

            if (!string.IsNullOrWhiteSpace(expeptionMessage))
            {
                throw new ArgumentException(expeptionMessage);
            }
        }

        private void MapReportParameters(ICustomReport<TSecurityOperationCode> source, Configuration.Domain.Reports.Report target)
        {
            var customReportParameters = source.GetReportParameters().ToList();

            this.CheckDuplicates(customReportParameters, source, z => z.Name);

            var mergeResult = target.Parameters.GetMergeResult(customReportParameters, z => z.Name, z => z.Name);

            foreach (var removingItem in mergeResult.RemovingItems)
            {
                target.RemoveDetail(removingItem);
            }

            var addings = mergeResult.AddingItems.Select(z => new { Source = z, Target = new ReportParameter(target) });
            var combining = mergeResult.CombineItems.Select(z => new { Source = z.Item2, Target = z.Item1 });

            var resultCombined = addings.Concat(combining);

            foreach (var combiteItem in resultCombined)
            {
                var sourceParameter = combiteItem.Source;

                var targetReportParameter = combiteItem.Target;

                targetReportParameter.TypeName = sourceParameter.TypeName;

                targetReportParameter.Name = sourceParameter.Name;

                targetReportParameter.IsRequired = sourceParameter.IsRequired;

                targetReportParameter.Order = sourceParameter.Order;

                targetReportParameter.DisplayValueProperty = sourceParameter.DisplayValueProperty;

                targetReportParameter.IsCollection = sourceParameter.IsCollection;
            }
        }


        private void MapAccessableDetails(ICustomReport<TSecurityOperationCode> source, Configuration.Domain.Reports.Report target)
        {
            this.RemoveDuplicates<Configuration.Domain.Reports.Report, AccessableBusinessRoleReportRight, Guid>(target);
            this.RemoveDuplicates<Configuration.Domain.Reports.Report, AccessableOperationReportRight, Guid>(target);
            this.RemoveDuplicates<Configuration.Domain.Reports.Report, AccessablePrincipalReportRight, string>(target);

            this.CheckDuplicates(source.AccessReportRight.Roles, source);
            this.CheckDuplicates(source.AccessReportRight.Operations, source);
            this.CheckDuplicates(source.AccessReportRight.Principals, source);

            this.MapAccessableDetail(target, source.AccessReportRight.Roles, r => new AccessableBusinessRoleReportRight(r));
            this.MapAccessableDetail(target, source.AccessReportRight.Operations, r => new AccessableOperationReportRight(r));
            this.MapAccessableDetail(target, source.AccessReportRight.Principals, r => new AccessablePrincipalReportRight(r));

        }

        private void MapAccessableDetail<TTargetMaster, TTargetDetail, TKey>(TTargetMaster targetMaster, IEnumerable<TKey> sourceKeys, Func<TTargetMaster, TTargetDetail> createTargetDetailFunc)
            where TTargetDetail : AccessableReportRightsBase<TKey>, IDetail<TTargetMaster>
            where TTargetMaster : class, IMaster<TTargetDetail>

        {
            var mergeRoleResult = targetMaster.Details.GetMergeResult(sourceKeys, z => z.Value, z => z);

            foreach (var removingItem in mergeRoleResult.RemovingItems)
            {
                targetMaster.RemoveDetail(removingItem);
            }

            foreach (var adding in mergeRoleResult.AddingItems)
            {
                createTargetDetailFunc(targetMaster).Self(z => z.Value = adding);
            }
        }

        private void CheckDuplicates<TValue, TKey>(IEnumerable<TValue> custromReportDetails, ICustomReport<TSecurityOperationCode> customReport, Func<TValue, TKey> keyFunc)
        {
            var duplicates = custromReportDetails.GroupBy(keyFunc, z => z).Where(z => z.Skip(1).Any()).Select(z => z.Key).ToList();

            if (duplicates.Any())
            {
                throw new ArgumentException($"CustomReport with name:'{customReport.Name}' has duplicates: '{string.Join(",", duplicates)}'");
            }

        }


        private void CheckDuplicates<TValue>(IEnumerable<TValue> values, ICustomReport<TSecurityOperationCode> customReport)
        {
            this.CheckDuplicates(values, customReport, z => z);
        }

        private void RemoveDuplicates<TMaster, TDetail, TKey>(TMaster master)
            where TDetail : AccessableReportRightsBase<TKey>, IDetail<TMaster>
            where TMaster : class, IMaster<TDetail>
        {
            var duplicates = master.Details.GroupBy(z => z.Value).Select(z => z.ToList()).SelectMany(z => z.Skip(1)).ToList();

            master.RemoveDetails(duplicates);
        }


        private void MapReportFilters(ICustomReport<TSecurityOperationCode> source, Configuration.Domain.Reports.Report target)
        {
            target.ClearDetails<Configuration.Domain.Reports.Report, ReportFilter>();
        }

        private void MapReportProperties(ICustomReport<TSecurityOperationCode> source, Configuration.Domain.Reports.Report target)
        {
            target.ClearDetails<Configuration.Domain.Reports.Report, ReportProperty>();
        }

        private void MapSimple(ICustomReport<TSecurityOperationCode> source, Configuration.Domain.Reports.Report target)
        {
            target.Name = source.Name;
            target.Description = source.Description;
            target.ReportType = ReportType.Custom;
            target.Owner = string.Empty;
            target.DomainTypeName = string.Empty;
            target.SecurityOperationCode = (int)Convert.ChangeType(source.SecurityOperation, typeof(int));
        }
    }
}
