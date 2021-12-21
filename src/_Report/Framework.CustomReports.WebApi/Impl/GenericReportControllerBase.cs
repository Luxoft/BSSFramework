using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Configuration.Domain.Reports;
using Framework.Configuration.Generated.DTO;
using Framework.Core.Helpers;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.OData;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Transfering;

using Microsoft.AspNetCore.Mvc;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;
using ReportIdentityDTO = Framework.Configuration.Generated.DTO.ReportIdentityDTO;

namespace Framework.CustomReports.WebApi
{
    public abstract class GenericReportControllerBase<TServiceEnvironment, TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode, TIdent, TMappingService> : ApiControllerBase<TServiceEnvironment, TBLLContext, EvaluatedData<TBLLContext, TMappingService>>
        where TServiceEnvironment : class, IServiceEnvironment<TBLLContext>, ISystemMetadataTypeBuilderContainer, IReportServiceContainer<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>, ISecurityOperationCodeProviderContainer<TSecurityOperationCode>

        where TBLLContext : class,

        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, TIdent>>>,

        ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,

        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,

        IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,

        IAuthorizationBLLContextContainer<Framework.Authorization.BLL.IAuthorizationBLLContext>,

        IStandartExpressionBuilderContainer, IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,

        IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, IIdentityObject<Guid>

        where TSecurityOperationCode : struct, Enum where TMappingService : class
    {
        private Lazy<Dictionary<string, TypeMetadata>> typeMetadataDictLazy;

        public GenericReportControllerBase(TServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
            this.InitDict();
        }

        public string DoHealthCheck()
        {
            return PerformanceHelper.GetHealthStatus(() => this.EvaluateRead((evaluatedData) => evaluatedData.Context.Configuration.Logics.Report.GetUnsecureQueryable().FirstOrDefault()));
        }

        public SelectOperationResult<ReportSimpleDTO> GetSimpleReports(string odataQueryString)
        {
            return base.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var selectOperation = SelectOperation.Parse(odataQueryString);

                var selectOperationResult = this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View).GetObjectsByOData(selectOperation);

                return new SelectOperationResult<ReportSimpleDTO>(selectOperationResult.Items.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData)));
            });
        }

        private ReportServiceContext<TBLLContext, TSecurityOperationCode> GetReportServiceContext(TBLLContext context)
        {
            return new ReportServiceContext<TBLLContext, TSecurityOperationCode>(context, this.ServiceEnvironment.SystemMetadataTypeBuilder, this.ServiceEnvironment.SecurityOperationCodeProvider);
        }

        public IEnumerable<ReportPropertySimpleDTO> GetSimpleReportProperties(Guid reportIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var results = this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View)
                    .GetSecureQueryable()
                    .Where(z => z.Id == reportIdentity)
                    .SelectMany(z => z.Properties)
                    .ToList();

                return results.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData));
            });
        }

        public IEnumerable<ReportFilterSimpleDTO> GetSimpleReportFilters(Guid reportIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var results = this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View)
                    .GetSecureQueryable()
                    .Where(z => z.Id == reportIdentity)
                    .SelectMany(z => z.Filters)
                    .ToList();

                return results.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData));
            });
        }

        public IEnumerable<ReportParameterSimpleDTO> GetSimpleReportParameters(Guid reportIdentity)
        {
            return base.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var results = this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View)
                                  .GetSecureQueryable()
                                  .Where(z => z.Id == reportIdentity)
                                  .SelectMany(z => z.Parameters)
                                  .ToList();

                return results.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData));
            });
        }

        public ReportGenerationRequestModelRichDTO GetRichReportGenerationRequestModel(Guid reportIdentity)
        {
            return base.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var results = this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View)
                    .GetSecureQueryable()
                    .Where(z => z.Id == reportIdentity)
                    .SelectMany(z => z.Parameters)
                    .ToList();

                var parameters = results.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData));

                var typeHashSet = parameters.Select(z => z.TypeName).ToHashSet();
                var typeMetadatas = typeHashSet.Select(z => this.typeMetadataDictLazy.Value[z]).ToList();


                return new ReportGenerationRequestModelRichDTO() { Parameters = parameters, TypeMetadatas = typeMetadatas };
            });
        }

        public SelectOperationResult<ReportParameterValueSimpleDTO> GetSimpleReportParameterValues(ReportParameterIdentityDTO identity, string odataQueryString)
        {
            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var context = evaludatedData.Context.Configuration;

                var parameter = context.Logics.ReportParameter.GetById(identity.Id, true, z => z.Select(q => q.Report));

                this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View).CheckAccess(parameter.Report);

                var selectOperation = context.StandartExpressionBuilder.ToTyped<ReportParameterValue>(SelectOperation.Parse(odataQueryString));

                var reportServiceContext = this.GetReportServiceContext(evaludatedData.Context);

                var result = this.ServiceEnvironment.ReportParameterValueService.GetParameterValuesBy(reportServiceContext, parameter, selectOperation);

                return new SelectOperationResult<ReportParameterValueSimpleDTO>(result.Items.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData)), result.TotalCount);
            });
        }

        public SelectOperationResult<ReportParameterValueSimpleDTO> GetSimpleReportParameterValuesByTypeName(string typeName, string odataQueryString)
        {
            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var context = evaludatedData.Context.Configuration;

                var selectOperation = context.StandartExpressionBuilder.ToTyped<ReportParameterValue>(SelectOperation.Parse(odataQueryString));

                var reportServiceContext = this.GetReportServiceContext(evaludatedData.Context);

                var result = this.ServiceEnvironment.ReportParameterValueService.GetParameterValuesBy(reportServiceContext, typeName, selectOperation);

                return new SelectOperationResult<ReportParameterValueSimpleDTO>(result.Items.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData)), result.TotalCount);
            });
        }

        public IList<int> GetReportParameterValuePositions(IEnumerable<ReportParameterValueStrictDTO> parameterValuesDto)
        {
            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var context = evaludatedData.Context.Configuration;

                var parameterDtoValues = parameterValuesDto.ToArray();

                var parameterIdent = parameterDtoValues.Select(z => z.ReportParameter.Id).Distinct().Single();

                var parameter = context.Logics.ReportParameter.GetById(parameterIdent, true, z => z.Select(q => q.Report));

                this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View).CheckAccess(parameter.Report);

                var parameterValues = parameterDtoValues
                .Select(z => new ReportParameterValue()
                {
                    DesignValue = z.DesignValue,
                    Value = z.Value,
                    ReportParameter = parameter
                }).ToList();

                var reportServiceContext = this.GetReportServiceContext(evaludatedData.Context);

                var result = this.ServiceEnvironment.ReportParameterValueService.GetParameterValuePositions(reportServiceContext, parameterValues);

                return result;
            });
        }

        public int GetReportParameterValuePosition(ReportParameterValueStrictDTO parameterValueStrictDTO)
        {
            return this.GetReportParameterValuePositions(new[] { parameterValueStrictDTO }).FirstOrDefault();
        }

        /// <summary> Получить позицию значения параметра по типу параметра </summary>
        /// <param name="typeName">Название типа параметра</param>
        /// <param name="id">ИД значения параметра</param>
        /// <param name="odataQueryString">OData запрос (как правило предназначен для сортировки)</param>
        /// <returns>Позизция значения параметра, если значение не было найдено, то возвращается -1</returns>
        public int GetReportParameterValuePositionByTypeName(string typeName, Guid id, string odataQueryString)
        {
            return this.GetReportParameterValuePositionsByTypeName(typeName, new[] { id }, odataQueryString).Single();
        }

        /// <summary> Получить позиции значений параметров по типу параметра </summary>
        /// <param name="typeName">Название типа параметра</param>
        /// <param name="ids">ИД значений параметров</param>
        /// <param name="odataQueryString">OData запрос (как правило предназначен для сортировки)</param>
        /// <returns>Список позиций значений параметра, в том порядке, в котором идут ИД значений в списке ids (если значение не было найдено, то в позции указывается -1)</returns>
        public IEnumerable<int> GetReportParameterValuePositionsByTypeName(string typeName, IEnumerable<Guid> ids, string odataQueryString)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var context = evaludatedData.Context.Configuration;

                var selectOperation = context.StandartExpressionBuilder.ToTyped<ReportParameterValue>(SelectOperation.Parse(odataQueryString));

                var reportServiceContext = this.GetReportServiceContext(evaludatedData.Context);

                var parameterValues = this.ServiceEnvironment.ReportParameterValueService.GetParameterValuesBy(reportServiceContext, typeName, selectOperation);

                var values = parameterValues.Items.Select((item, index) => new { Id = Guid.Parse(item.Value), Index = index });

                return from id in ids
                       join value in values on id equals value.Id into tmpValue
                       from value in tmpValue.DefaultIfEmpty()
                       select value != null ? value.Index : -1;
            });
        }

        public ReportIdentityDTO SaveReport(ReportStrictDTO reportStrict)
        {
            return this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var bll = this.CreateReportLogic(evaluateData.Context, BLLSecurityMode.Edit);

                var domainObject = bll.GetByIdOrCreate(reportStrict.Id);

                reportStrict.MapToDomainObject(this.GetConfigurationMappingService(evaluateData), domainObject);

                bll.Save(domainObject);

                return domainObject.ToIdentityDTO();
            });
        }

        public void RemoveReport(ReportIdentityDTO reportIdent)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var bll = this.CreateReportLogic(evaluateData.Context, BLLSecurityMode.Edit);

                var domainObject = bll.GetById(reportIdent.Id, true);

                bll.Remove(domainObject);
            });
        }

        public ReportRichDTO GetRichReport(ReportIdentityDTO reportIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var bll = this.CreateReportLogic(evaluateData.Context, BLLSecurityMode.View);

                var domainObject = bll.GetById(reportIdentity.Id, true, evaluateData.Context.Configuration.FetchService.GetContainer<Configuration.Domain.Reports.Report>(MainDTOType.FullDTO));

                return domainObject.ToRichDTO(this.GetConfigurationMappingService(evaluateData));
            });
        }

        public virtual FileStreamResult GetStream(ReportGenerationModelStrictDTO modelDTO)
        {
            return this.Evaluate(DBSessionMode.Read, eval =>
            {
                var context = eval.Context;

                var reportServiceContext = this.GetReportServiceContext(context);

                var model = modelDTO.ToDomainObject(this.GetConfigurationMappingService(context.Configuration));

                this.CreateReportLogic(context, BLLSecurityMode.View).CheckAccess(model.Report);

                model.PredefineGenerationValues = this.GetPredefineFilterParameters(model, reportServiceContext);

                var reportStream = this.ServiceEnvironment.ReportService.GetReportStream(model, reportServiceContext);

                return this.GetReportResult(reportStream);
            });
        }

        protected virtual IEnumerable<ReportGenerationPredefineValue> GetPredefineFilterParameters(ReportGenerationModel model, ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext)
        {
            if (model.Report.Filters.Any(z => !z.IsValueFromParameters))
            {
                var reportDomainType = this.ServiceEnvironment.SystemMetadataTypeBuilder.TypeResolver.Resolve(new TypeHeader(model.Report.DomainTypeName));

                foreach (var item in model.Report.Filters.Where(z => !z.IsValueFromParameters))
                {
                    var lastProperty = reportDomainType.ToPropertyInfoChain(item.Property.GetPropertyNameChain()).Last();

                    var propertyTypeHeader = new TypeHeader(lastProperty.PropertyType);

                    var found = this.typeMetadataDictLazy.Value.TryGetValue(lastProperty.PropertyType.Name, out var propertyDomainType);

                    if (found && propertyDomainType.Role == TypeRole.Domain)
                    {
                        if (!Guid.TryParse(item.Value, out var expectedId))
                        {
                            throw new BusinessLogicException($"Expected Guid in filter:{item.Property}");
                        }

                        yield return new ReportGenerationPredefineValue(propertyDomainType.Type.Name, this.ServiceEnvironment.ReportParameterValueService.GetDesignValue(reportServiceContext, expectedId, propertyTypeHeader));
                    }
                    else
                    {
                        yield return new ReportGenerationPredefineValue(lastProperty.Name, item.Value);
                    }
                }
            }
        }

        protected virtual string GetContentDisposition(string name, string extension)
        {
            var fileName = $"{name} {DateTimeService.Default.Now.ToShortDateString()}.{extension}";
            return $"{HttpUtility.UrlPathEncode(fileName)}";
        }

        protected virtual ConfigurationServerPrimitiveDTOMappingService GetConfigurationMappingService(IConfigurationBLLContext configurationContext)
        {
            return new ConfigurationServerPrimitiveDTOMappingService(configurationContext);
        }

        private IReportBLL CreateReportLogic(TBLLContext context, BLLSecurityMode securityMode)
        {
            var configurationContext = context.Configuration;

            var securityProvider = configurationContext.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.Report>(securityMode);

            if (BLLSecurityMode.View == securityMode)
            {
                securityProvider = securityProvider.Or(new ReportSecurityProvider(configurationContext), configurationContext.AccessDeniedExceptionService);
            }

            return configurationContext.Logics.ReportFactory.Create(securityProvider);
        }

        private ConfigurationServerPrimitiveDTOMappingService GetConfigurationMappingService(EvaluatedData<TBLLContext> evaludatedData)
        {
            return this.GetConfigurationMappingService(evaludatedData.Context.Configuration);
        }

        public IEnumerable<TypeMetadata> GetTypeMetadatas()
        {
            return (IEnumerable<TypeMetadata>)this.ServiceEnvironment.SystemMetadataTypeBuilder.SystemMetadata.Types;
        }

        private void InitDict()
        {
            this.typeMetadataDictLazy = new Lazy<Dictionary<string, TypeMetadata>>(() => this.GetTypeMetadataDict(), true);
        }

        private Dictionary<string, TypeMetadata> GetTypeMetadataDict()
        {
            return this.ServiceEnvironment.SystemMetadataTypeBuilder.SystemMetadata.Types.ToDictionary(z => z.Type.Name, z => z);
        }
    }
}
