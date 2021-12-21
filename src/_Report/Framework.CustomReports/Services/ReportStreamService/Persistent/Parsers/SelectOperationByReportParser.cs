using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Core;
using Framework.DomainDriven.SerializeMetadata;
using Framework.OData;
using Framework.QueryLanguage;

namespace Framework.CustomReports.Services
{
    public class SelectOperationByReportParser<TDomain> : ISelectOperationByReportParser
    {
        private readonly ITypeResolver<TypeHeader> typeResolver;

        private readonly ReadOnlyCollection<TypeMetadata> types;

        public SelectOperationByReportParser(ITypeResolver<TypeHeader> typeResolver, ReadOnlyCollection<TypeMetadata> types)
        {
            this.typeResolver = typeResolver;
            this.types = types;
        }

        public SelectOperation Parse(ReportGenerationModel model)
        {
            var filter = this.ParseFilter(model);

            var orders = this.ParseOrders(model.Report).ToList();

            var selects = this.ParseSelects(model.Report).ToList();

            var expands = this.ParseExpands(model.Report).ToList();

            var selectOperation = new SelectOperation(
                filter,
                orders,
                expands,
                selects,
                SelectOperation.Default.SkipCount,
                SelectOperation.Default.TakeCount);

            return selectOperation;
        }

        private LambdaExpression ParseFilter(ReportGenerationModel model)
        {
            return new SelectOperationFilterParser<TDomain>(this.types, this.typeResolver).Parse(model);
        }

        protected virtual IEnumerable<LambdaExpression> ParseSelects(Configuration.Domain.Reports.Report report)
        {
            var parameter = new ParameterExpression("z");

            return report.Properties.Select(z => new LambdaExpression(parameter.ToPropertyExpr(z.PropertyPath), parameter));
        }

        protected virtual IEnumerable<LambdaExpression> ParseExpands(Configuration.Domain.Reports.Report report)
        {
            var result = new HashSet<string>();
            var parameter = new ParameterExpression("z");

            foreach (var property in report.Properties.Select(z => z.PropertyPath))
            {
                var splitted = property.Split('/').ToList();
                for (int q = 1; q < splitted.Count; q++)
                {
                    result.Add(splitted.Take(q).Join("/"));
                }
            }

            return result.Select(z => new LambdaExpression(parameter.ToPropertyExpr(z), parameter));
        }

        protected virtual IEnumerable<SelectOrder> ParseOrders(Configuration.Domain.Reports.Report report)
        {
            var parameter = new ParameterExpression("z");
            var orders = report.Properties.Where(q => q.SortType != 0).OrderBy(z => z.SortOrdered);
            return orders.Select(z => new SelectOrder(new LambdaExpression(parameter.ToPropertyExpr(z.PropertyPath), parameter), z.SortType.ToOrderType()));
        }
    }
}
