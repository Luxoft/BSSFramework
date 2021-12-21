using System.Collections.ObjectModel;
using System.Linq;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Core;
using Framework.DomainDriven.SerializeMetadata;
using Framework.QueryLanguage;

namespace Framework.CustomReports.Services
{
    internal class SelectOperationFilterParser<TDomain>
    {
        private readonly ITypeResolver<TypeHeader> resolver;

        private readonly SystemTypeDictionary systemTypes;

        public SelectOperationFilterParser(ReadOnlyCollection<TypeMetadata> types, ITypeResolver<TypeHeader> resolver)
        {
            this.resolver = resolver;
            this.systemTypes = new SystemTypeDictionary(types);
        }

        public virtual LambdaExpression Parse(ReportGenerationModel model)
        {
            var evaluatedFilters = FilterEvaluator.Instance.Evaluate<TDomain>(model).ToList();

            var parameter = new ParameterExpression("z");

            if (!evaluatedFilters.Any())
            {
                return new LambdaExpression(new BooleanConstantExpression(true), parameter);
            }

            var filterBody = evaluatedFilters
                .Select(reportFilter => this.GetBinary(parameter, model.Report, reportFilter))
                .Aggregate((prev, current) => new BinaryExpression(prev, BinaryOperation.AndAlso, current));

            var filter = new LambdaExpression(filterBody, parameter);


            return filter;
        }

        protected Expression GetBinary(ParameterExpression parameter, Configuration.Domain.Reports.Report report, EvaluatedFilter reportFilter)
        {
            var propertyExpr = parameter.ToPropertyExpr(reportFilter.PropertyChain);

            var sourcePropertyDomainType = this.systemTypes.GetPropertyType(report.DomainTypeName, propertyExpr);
            var propertyDomainType = sourcePropertyDomainType;

            if (propertyDomainType.Source.Role == TypeRole.Domain)
            {
                propertyExpr = new PropertyExpression(propertyExpr, "Id");

                propertyDomainType = this.systemTypes.GetPropertyType(report.DomainTypeName, propertyExpr);
            }

            var propertyType = this.resolver.Resolve(propertyDomainType.Source.Type);

            var constExpression = reportFilter.Value.TryProcessNull(reportFilter.FilterOperator).ToConstExpression(propertyType);

            return this.TryExpand(sourcePropertyDomainType, propertyExpr, reportFilter.FilterOperator, constExpression)
                   ?? FilterToExpressionParser.Parse(propertyExpr, reportFilter, constExpression);
        }

        protected virtual Expression TryExpand(TypeMetadataDictionary sourcePropertyDomainType, PropertyExpression source, string binaryOperator, ConstantExpression constValue)
        {
            if (binaryOperator == "eq" && sourcePropertyDomainType.IsHierarhical)
            {
                return new ExpandContainsExpression(new StringConstantExpression("Children"), constValue, source.Source);
            }

            return null;
        }
    }
}
