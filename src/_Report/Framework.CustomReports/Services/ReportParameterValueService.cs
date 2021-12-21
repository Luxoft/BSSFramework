using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Configuration.Domain.Reports;
using Framework.Core;
using Framework.CustomReports.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.OData;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.CustomReports.Services
{
    public class ReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : IReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>

        where TBLLContext : DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>,
            IAuthorizationBLLContextContainer<IAuthorizationBLLContext>,
            IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
            ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
            IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,
            ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TSecurityOperationCode : struct, Enum

    {
        internal class MockDomainType
        {
            public string EmbeddedDesignValue { get; set; }
            public Guid EmbeddedId { get; set; }
        }

        internal class ModelReportParameterValue
        {
        }

        private static IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, Guid> GetBLLBy<TDomainObject>(TSecurityOperationCode? securityOperationCode, TBLLContext mainContext)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, Guid> bll;
            if (securityOperationCode.HasValue && mainContext.GetSecurityOperation(securityOperationCode.Value) != null)
            {
                bll = mainContext.Logics.Implemented.Create<TDomainObject>(securityOperationCode.Value);
            }
            else
            {
                bll = mainContext.Logics.Implemented.Create<TDomainObject>();
            }
            return bll;
        }

        public ISelectOperationResult<ReportParameterValue> GetParameterValuesBy(
    ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext,
    ReportParameter parameter,
    SelectOperation<ReportParameterValue> selectOperation)
        {
            var parameterInfo = this.GetReportParameterInfo(reportServiceContext, parameter);

            return this.GetParameterValuesBy(reportServiceContext, parameterInfo, selectOperation);
        }

        public ISelectOperationResult<ReportParameterValue> GetParameterValuesBy(ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext, string typeName, SelectOperation<ReportParameterValue> selectOperation)
        {
            var parameterInfo = this.GetReportParameterInfo(reportServiceContext, typeName);

            return this.GetParameterValuesBy(reportServiceContext, parameterInfo, selectOperation);
        }

        private ISelectOperationResult<ReportParameterValue> GetParameterValuesBy(
            ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext,
            ReportParameterInfo parameterInfo,
            SelectOperation<ReportParameterValue> selectOperation)
        {
            TSecurityOperationCode? securityOperationCode = parameterInfo.SecurityOperationCode;

            if (Equals(securityOperationCode, default(TSecurityOperationCode)))
            {
                securityOperationCode = new TSecurityOperationCode?();
            }

            var domainType = parameterInfo.DomainType;

            var propertyMetadata = parameterInfo.PropertyMetadata;

            var domainFilter = selectOperation.Filter.Convert(domainType, Tuple.Create("DesignValue", propertyMetadata.Name));

            var parameterExpressionInfo = this.GetParameterExpressionInfo(domainType, propertyMetadata);

            var orderExpression = parameterExpressionInfo.Order;

            var parameterSelect = parameterExpressionInfo.Select;

            var method = new Func<
                TSecurityOperationCode?,
                TBLLContext,
                Expression<Func<TPersistentDomainObjectBase, bool>>,
                Expression<Func<TPersistentDomainObjectBase, string>>,
                int?,
                int?,
                Expression<Func<TPersistentDomainObjectBase, ReportParameterValue>>,
                bool,
                SelectOperationResult<ReportParameterValue>>(this.GetDynamicResultByOData)
                .CreateGenericMethod(domainType);

            var mainContext = reportServiceContext.Context;

            var untypedResult = method.Invoke(this, new object[]
            {
                securityOperationCode,
                mainContext,
                domainFilter,
                orderExpression,
                selectOperation.HasPaging ? new int?(selectOperation.SkipCount) : null,
                selectOperation.HasPaging ? new int?(selectOperation.TakeCount) : null,
                parameterSelect,
                true
            });

            var result = (ISelectOperationResult<ReportParameterValue>)untypedResult;

            result.Items.Foreach(z => z.Value = z.StrictId.ToString());

            return result;
        }

        private ReportParameterInfo GetReportParameterInfo(ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext, ReportParameter parameter)
        {
            var securityOperationCode = reportServiceContext.SecurityOperationCodeProvider.GetSecurityOperationCode(parameter.Report, reportServiceContext.Context);

            var systemMetadataTypeBuilder = reportServiceContext.SystemMetadataTypeBuilder;

            var domainProperty = parameter.ToDomainProperty(systemMetadataTypeBuilder);

            var propertyMetadata = domainProperty.PropertyMetadata;

            var domainType = domainProperty.DomainType;

            if (propertyMetadata.IsSecurity)
            {
                throw new NotImplementedException("Not implement for security property");
            }

            return new ReportParameterInfo(securityOperationCode, propertyMetadata, domainType);
        }

        private ReportParameterInfo GetReportParameterInfo(ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext, string typeName)
        {
            var systemMetadataTypeBuilder = reportServiceContext.SystemMetadataTypeBuilder;

            var domainProperty = new TypeHeader(typeName).ToVisualDomainProperty(systemMetadataTypeBuilder);

            var propertyMetadata = domainProperty.PropertyMetadata;

            var domainType = domainProperty.DomainType;

            var securityOperationCode = reportServiceContext.SecurityOperationCodeProvider.GetByDomain(domainProperty.DomainType, BLLSecurityMode.View);

            if (propertyMetadata.IsSecurity)
            {
                throw new NotImplementedException("Not implemented because of security property");
            }

            return new ReportParameterInfo(securityOperationCode, propertyMetadata, domainType);
        }

        private struct ParameterExpressionInfo
        {
            public Expression Order { get; private set; }
            public Expression Select { get; private set; }

            public ParameterExpressionInfo(Expression select, Expression order) : this()
            {
                this.Select = select;
                this.Order = order;
            }
        }

        private struct ReportParameterInfo
        {
            public ReportParameterInfo(TSecurityOperationCode securityOperationCode, PropertyMetadata propertyMetadata, Type domainType) : this()
            {
                this.PropertyMetadata = propertyMetadata;
                this.DomainType = domainType;
                this.SecurityOperationCode = securityOperationCode;
            }

            public TSecurityOperationCode SecurityOperationCode { get; private set; }
            public PropertyMetadata PropertyMetadata { get; private set; }
            public Type DomainType { get; private set; }
        }

        public IList<int> GetParameterValuePositions(
            ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext,
            IList<ReportParameterValue> parameterValue)
        {
            var systemMetadataTypeBuilder = reportServiceContext.SystemMetadataTypeBuilder;

            var report = parameterValue.Select(z => z.ReportParameter.Report).Distinct().Single();
            var reportParameter = parameterValue.Select(z => z.ReportParameter).Distinct().Single();

            TSecurityOperationCode? securityOperationCode = reportServiceContext.SecurityOperationCodeProvider.GetSecurityOperationCode(report, reportServiceContext.Context);

            if (Equals(securityOperationCode, default(TSecurityOperationCode)))
            {
                securityOperationCode = new TSecurityOperationCode?();
            }

            var domainProperty = reportParameter.ToDomainProperty(systemMetadataTypeBuilder);

            var propertyMetadata = domainProperty.PropertyMetadata;

            var domainType = domainProperty.DomainType;

            var parameterExpressionInfo = this.GetParameterExpressionInfo(domainType, propertyMetadata);

            var orderExpression = parameterExpressionInfo.Order;

            var parameterSelect = parameterExpressionInfo.Select;

            var method = new Func<
                TSecurityOperationCode?,
                TBLLContext,
                Expression<Func<TPersistentDomainObjectBase, string>>,
                Expression<Func<TPersistentDomainObjectBase, ReportParameterValue>>,
                IList<ReportParameterValue>,
                IList<int>>(this.GetValuePosition)
                .CreateGenericMethod(domainType);

            var mainContext = reportServiceContext.Context;

            var untypedResult = method.Invoke(this, new object[]
            {
                securityOperationCode,
                mainContext,
                orderExpression,
                parameterSelect,
                parameterValue
            });

            var result = (IList<int>)untypedResult;

            return result;
        }

        public string GetDesignValue(ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext, Guid id, TypeHeader typeHeader)
        {
            var visualDomainProperty = typeHeader.ToVisualDomainProperty(reportServiceContext.SystemMetadataTypeBuilder);

            var method = new Func<
                            TBLLContext,
                            Guid,
                            string,
                            string>(this.GetDesingValue<TPersistentDomainObjectBase>)
                    .CreateGenericMethod(visualDomainProperty.DomainType);

            var mainContext = reportServiceContext.Context;

            var untypedResult = method.Invoke(
                this,
                new object[]
                {
                    mainContext,
                    id,
                    visualDomainProperty.PropertyMetadata.Name
                });

            var result = (string)untypedResult;

            return result;
        }

        private string GetDesingValue<TDomainObject>(
                TBLLContext mainContext,
                Guid id,
                string designPropertyName)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var bll = GetBLLBy<TDomainObject>(null, mainContext);

            var parameter = Expression.Parameter(typeof(TDomainObject), "z");
            Expression<Func<TDomainObject, string>> selectExpression = Expression.Lambda<Func<TDomainObject, string>>(Expression.Property(parameter, designPropertyName), parameter);

            var result = bll.GetSecureQueryable().Where(z => z.Id == id).Select(selectExpression).FirstOrDefault();

            return result;
        }

        private ParameterExpressionInfo GetParameterExpressionInfo(Type domainType, PropertyMetadata propertyMetadata)
        {
            var orderExpression = ((Expression<Func<MockDomainType, string>>)(z => z.EmbeddedDesignValue)).Convert(domainType, Tuple.Create("EmbeddedDesignValue", propertyMetadata.Name));

            Expression<Func<MockDomainType, ReportParameterValue>> selectTemplate = z => new ReportParameterValue { StrictId = z.EmbeddedId, DesignValue = z.EmbeddedDesignValue };

            var parameterSelect = selectTemplate.Convert(domainType, Tuple.Create("EmbeddedId", "Id"), Tuple.Create("EmbeddedDesignValue", propertyMetadata.Name));

            return new ParameterExpressionInfo(parameterSelect, orderExpression);
        }

        private IList<int> GetValuePosition<TDomainObject>(
            TSecurityOperationCode? securityOperationCode,
            TBLLContext mainContext,
            Expression<Func<TDomainObject, string>> orderExpression,
            Expression<Func<TDomainObject, ReportParameterValue>> selectExpression,
            IList<ReportParameterValue> templateValue)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var bll = GetBLLBy<TDomainObject>(securityOperationCode, mainContext);

            var result = bll.GetSecureQueryable().OrderBy(orderExpression).Select(selectExpression).ToList();

            var valueHashSet = templateValue.Select(z => z.DesignValue?.ToLowerInvariant()).ToHashSet();

            return result.Select(TupleStruct.Create)
                .Where(z => valueHashSet.Contains(z.Item1.DesignValue?.ToLower()))
                .Select(z => z.Item2)
                .Take(valueHashSet.Count)
                .ToList();
        }


        private SelectOperationResult<ReportParameterValue> GetDynamicResultByOData<TDomainObject>(
                TSecurityOperationCode? securityOperationCode,
                TBLLContext mainContext,
                Expression<Func<TDomainObject, bool>> filterExpression,
                Expression<Func<TDomainObject, string>> orderExpression,
                int? skip,
                int? count,
                Expression<Func<TDomainObject, ReportParameterValue>> selectExpression,
                bool materializeBeforSelect)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var bll = GetBLLBy<TDomainObject>(securityOperationCode, mainContext);

            var preResult = bll.GetSecureQueryable().Where(filterExpression);
            if (!materializeBeforSelect)
            {
                preResult = preResult.OrderBy(orderExpression);
            }

            if (skip.HasValue && count.HasValue)
            {
                if (materializeBeforSelect)
                {
                    preResult = preResult.ToList().AsQueryable().OrderBy(orderExpression);
                }
                return new SelectOperationResult<ReportParameterValue>(
                                                                       preResult.Skip(skip.Value).Take(count.Value).Select(selectExpression).ToList(),
                                                                       preResult.Count());
            }

            if (!skip.HasValue && !count.HasValue)
            {
                var result = preResult.OrderBy(orderExpression).Select(selectExpression).ToList();
                return new SelectOperationResult<ReportParameterValue>(result, result.Count);
            }

            throw new NotImplementedException($"{nameof(skip)} and {nameof(count)} must be all has value or has't value. Skip:'{skip}'.Count:'{count}'");
        }
    }
}
