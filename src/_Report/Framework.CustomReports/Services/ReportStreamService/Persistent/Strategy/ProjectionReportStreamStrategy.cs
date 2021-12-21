using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Configuration.Domain.Reports;
using Framework.Core;
using Framework.CustomReports.Services.ExcelBuilder;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.CustomReports.Services.Persistent.Strategy
{
    internal class ProjectionReportStreamStrategy<TMainBLLContext, TDomainObject, TPersistentDomainObjectBase, TSecurityOperationCode> : ReportStreamStrategy<TMainBLLContext, TDomainObject, TPersistentDomainObjectBase, TSecurityOperationCode>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TMainBLLContext :
        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
        DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Configuration.BLL.IConfigurationBLLContext>,
        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,
        ISecurityServiceContainer<IRootSecurityService<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
        ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum where TDomainObject : class, TPersistentDomainObjectBase
    {
        private static ConcurrentDictionary<Tuple<Guid, long, string>, Delegate> plainPropertyIdentToGetFuncDictionary = new ConcurrentDictionary<Tuple<Guid, long, string>, Delegate>();
        private static HashSet<Guid> plainMaterializedPropertyFuncs = new HashSet<Guid>();

        private static ConcurrentDictionary<Tuple<Guid, long>, Type> reportKeyToProjectionTypeDictionary = new ConcurrentDictionary<Tuple<Guid, long>, Type>();

        public ProjectionReportStreamStrategy(TMainBLLContext context, SystemMetadata systemMetadata) : base(context, systemMetadata)
        {
        }

        public override Stream Generate(
            ReportGenerationModel model,
            List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[] propertyInfoChain)> reportPropertyToMetadataLinks)
        {
            var plainReportType = this.GetProjectionType(model.Report);

            var method = new Func<ReportGenerationModel, List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[] propertyInfoChain)>, Stream>(this.EvaluatePlain<object>).CreateGenericMethod(plainReportType);

            return (Stream)method.Invoke(this, new object[] { model, reportPropertyToMetadataLinks });

        }

        private Type GetProjectionType(Configuration.Domain.Reports.Report report)
        {
            return reportKeyToProjectionTypeDictionary.GetOrAdd(Tuple.Create(report.Id, report.Version), (key) =>
            {
                var reportProperties = report.Properties;

                var members = reportProperties.Select(z => new TypeMapMember(z.ToStandartPropertyName(), this.GetPropertyType(z))).ToList();

                var reportPlainTypeBuilder = ProjectionReportTypeBuilder.Instance;

                var plainReportType = reportPlainTypeBuilder.GetAnonymousType(new TypeMap<TypeMapMember>(this.GetProjectionReportName(report), members));

                return plainReportType;
            });
        }

        private string GetProjectionReportName(Configuration.Domain.Reports.Report report)
        {
            return string.Join(string.Empty, (report.Id.ToString() + report.Version.ToString()).Split(new[] { '-' }));
        }

        private Type GetPropertyType(ReportProperty z)
        {
            var propertyInfoChain = typeof(TDomainObject).ToPropertyInfoChain(z.GetPropertyNameChain());

            var type = propertyInfoChain.LastOrDefault()?.PropertyType.TryToNullable()
                               ?? typeof(TDomainObject).GetProperty(z.PropertyPath, true).PropertyType;
            return type;
        }

        private Stream EvaluatePlain<TPlainAnon>(ReportGenerationModel model, List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[] propertyInfoChain)> links)
        {
            var report = model.Report;

            var filters = FilterEvaluator.Instance.Evaluate<TDomainObject>(model);

            var partialFilter = filters
                .Partial(
                z => typeof(TDomainObject).ToPropertyInfoChain(z.PropertyChain).Any(q => !q.HasPrivateField()),
                (virtualFilters, strongFilters) => new { virtualFilters, strongFilters });

            if (partialFilter.virtualFilters.Any())
            {
                // TODO process it!
                throw new ArgumentException("Current version is not supported virtual filters");
            }

            var projectionExpression = this.GetProjectionExpression<TPlainAnon>(model.Report);

            IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, Guid> bll;

            if (this.context.GetSecurityOperation<TDomainObject>(BLLSecurityMode.View) != null)
            {
                bll = this.context.Logics.Implemented.Create<TDomainObject>(BLLSecurityMode.View);
            }
            else
            {
                bll = this.context.Logics.Implemented.Create<TDomainObject>();
            }

            var anonResultQueryables = bll.GetSecureQueryable()
                .Where(this.ToFilterExpression(partialFilter.strongFilters));

            var orderedAnonResults = this.ProcessOrder(anonResultQueryables, report)
                .Select(projectionExpression)
                .ToList();

            var headers = this.GetPlainDesignPropertyInfos<TPlainAnon>(report, links).ToHeaders();

            var parameterModel = this.GetReportParameterInfos(model);

            return this.GetExcelReportStreamService().Generate(
                    model.Report.Name,
                    headers,
                    orderedAnonResults,
                    parameterModel);
        }

        private Expression<Func<TDomainObject, bool>> ToFilterExpression(IList<EvaluatedFilter> evaluateFilters)
        {
            if (evaluateFilters.Any())
            {
                var parameter = Expression.Parameter(typeof(TDomainObject), "z");

                var filterBody = evaluateFilters
                   .Select(reportFilter => this.GetBinary(parameter, reportFilter))
                   .Aggregate(
                   (prev, current) => Expression.MakeBinary(ExpressionType.AndAlso, prev, current));

                return Expression.Lambda<Func<TDomainObject, bool>>(filterBody, parameter);
            }

            return z => true;
        }

        protected Expression GetBinary(ParameterExpression parameter, EvaluatedFilter reportFilter)
        {
            var propertyExpr = parameter.ToPropertyExpr(reportFilter.PropertyChain);

            return this.TryExpand(propertyExpr, reportFilter.FilterOperator, reportFilter.Value)
                   ?? FilterToExpressionParser.Parse(propertyExpr, reportFilter);
        }

        interface ITemplateExpandSource : IHierarchicalSource<ITemplateExpandSource>, IIdentityObject<Guid>
        {
        }

        private Expression TryExpand(MemberExpression propertyExpr, string filterOperator, string value)
        {
            if (filterOperator != "eq")
            {
                return null;
            }

            var propertyType = propertyExpr.GetValueType();

            if (!propertyType.IsInterfaceImplementation(typeof(IHierarchicalSource<>), new[] { propertyType }))
            {

                return null;
            }

            var func = ((Func<ITemplateExpandSource, Guid, HierarchicalExpandType, bool>)HierarchicalExtensions.IsExpandableBy).Method;

            return Expression.Call(
                func.GetGenericMethodDefinition().MakeGenericMethod(new[] { propertyType, typeof(Guid) }),
                new Expression[] { propertyExpr, Expression.Constant(new Guid(value)), Expression.Constant(HierarchicalExpandType.Children, typeof(HierarchicalExpandType)) });
        }

        protected IEnumerable<ExcelDesignProperty<TProjection>> GetPlainDesignPropertyInfos<TProjection>(
            Configuration.Domain.Reports.Report report,
            List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[]
                propertyInfoChain)> links)
        {
            return report.Properties
                         .OrderBy(z => z.Order)
                         .Select(z =>
                                 {
                                     var func = plainPropertyIdentToGetFuncDictionary.GetOrAdd(Tuple.Create(z.Id, report.Version, z.PropertyPath + report.DomainTypeName), (_) =>
                                     {
                                         if (!plainMaterializedPropertyFuncs.Add(z.Id))
                                         {
                                             //remove previus funcs
                                             var forRemove = plainPropertyIdentToGetFuncDictionary.Keys.Where(q => q.Item1 == z.Id).ToList();

                                             forRemove.Foreach(q =>
                                                               {
                                                                   Delegate @delegate;
                                                                   plainPropertyIdentToGetFuncDictionary.TryRemove(q, out @delegate);
                                                               });
                                         }

                                         var parameter = Expression.Parameter(typeof(TProjection), "z");

                                         return Expression.Lambda<Func<TProjection, object>>(Expression.Convert(Expression.Property(parameter, z.ToStandartPropertyName()), typeof(object)), new[] { parameter }).Compile();
                                     });

                                     var castedFunc = (Func<TProjection, object>)func;

                                     return this.GetExcelDesignProperty(z, castedFunc, links.First(l => l.reportProperty.Id == z.Id).propertyInfoChain.Last().PropertyType.GetNullableElementTypeOrSelf());
                                 });
        }

        private Expression<Func<TDomainObject, TPlain>> GetProjectionExpression<TPlain>(Configuration.Domain.Reports.Report report)
        {
            var source = Expression.Parameter(typeof(TDomainObject), "source");

            var target = Expression.New(typeof(TPlain));

            var sourcePropertiesDictionary = typeof(TDomainObject).GetProperties().ToDictionary(z => z.Name, z => z);

            var targetPropertiesDictionary = typeof(TPlain).GetProperties().ToDictionary(z => z.Name, z => z);

            var targetMemberInits = new List<MemberAssignment>();

            foreach (var reportProperty in report.Properties.OrderBy(z => z.PropertyPath))
            {
                var paths = typeof(TDomainObject).ToPropertyInfoChain(reportProperty.GetPropertyNameChain()).ToList();

                if (paths.Count == 0)
                {
                    paths.Add(sourcePropertiesDictionary[reportProperty.ToStandartPropertyName()]);
                }

                var sourcePropertyExpression = paths.Aggregate((Expression)source, Expression.Property);

                var targetPropertyInfo = targetPropertiesDictionary[reportProperty.ToStandartPropertyName()];

                if (targetPropertyInfo.PropertyType != paths.Last().PropertyType)
                {
                    sourcePropertyExpression = Expression.Convert(sourcePropertyExpression, targetPropertyInfo.PropertyType);
                }

                targetMemberInits.Add(Expression.Bind(targetPropertyInfo, sourcePropertyExpression));
            }

            return Expression.Lambda<Func<TDomainObject, TPlain>>(Expression.MemberInit(target, targetMemberInits), source);
        }

        private IQueryable<TDomainObject> ProcessOrder(IQueryable<TDomainObject> source, Configuration.Domain.Reports.Report report)
        {
            if (report.Properties.All(z => z.SortType == 0))
            {
                return source;
            }

            var sourceParameter = Expression.Parameter(typeof(TDomainObject), "sourceParameter");

            IQueryable<TDomainObject> result = source;

            var isFirst = true;

            foreach (var reportProperty in report.Properties.Where(z => z.SortType != 0).OrderBy(z => z.SortOrdered))
            {
                var paths = typeof(TDomainObject).ToPropertyInfoChain(reportProperty.GetPropertyNameChain()).ToList();

                if (paths.Count == 0)
                {
                    continue;
                }

                var sourcePropertyExpression = paths.Aggregate((Expression)sourceParameter, Expression.Property);

                var orderExpression = Expression.Lambda<Func<TDomainObject, object>>(Expression.Convert(sourcePropertyExpression, typeof(object)), sourceParameter);

                if (isFirst)
                {
                    if (reportProperty.SortType == 1)
                    {
                        result = result.OrderBy(orderExpression);
                    }
                    else
                    {
                        result = result.OrderByDescending(orderExpression);
                    }
                }
                else
                {
                    if (reportProperty.SortType == 1)
                    {
                        result = ((IOrderedQueryable<TDomainObject>)result).ThenBy(orderExpression);
                    }
                    else
                    {
                        result = ((IOrderedQueryable<TDomainObject>)result).ThenByDescending(orderExpression);
                    }
                }
                isFirst = false;
            }

            return result;
        }
    }
}
