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
    public abstract class GenericReportControllerBase<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode, TIdent, TMappingService> : ApiControllerBase<TBLLContext, EvaluatedData<TBLLContext, TMappingService>>

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
        private readonly ISystemMetadataTypeBuilder systemMetadataTypeBuilder;

        private readonly IReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> reportParameterValueService;

        private readonly IReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> reportService;

        private readonly ISecurityOperationCodeProvider<TSecurityOperationCode> securityOperationCodeProvider;

        private Lazy<Dictionary<string, TypeMetadata>> typeMetadataDictLazy;


        protected GenericReportControllerBase(
                ISystemMetadataTypeBuilder systemMetadataTypeBuilder,
                IReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> reportParameterValueService,
                IReportService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> reportService,
                ISecurityOperationCodeProvider<TSecurityOperationCode> securityOperationCodeProvider)
        {
            this.systemMetadataTypeBuilder = systemMetadataTypeBuilder;
            this.reportParameterValueService = reportParameterValueService;
            this.reportService = reportService;
            this.securityOperationCodeProvider = securityOperationCodeProvider;

            this.InitDict();
        }


        [HttpPost(nameof(DoHealthCheck))]
        public string DoHealthCheck()
        {
            return PerformanceHelper.GetHealthStatus(() => this.EvaluateRead((evaluatedData) => evaluatedData.Context.Configuration.Logics.Report.GetUnsecureQueryable().FirstOrDefault()));
        }

        [HttpPost(nameof(GetSimpleReports))]
        public SelectOperationResult<ReportSimpleDTO> GetSimpleReports([FromBody]string odataQueryString)
        {
            return base.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var selectOperation = SelectOperation.Parse(odataQueryString);

                var selectOperationResult = this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View).GetObjectsByOData(selectOperation);

                return new SelectOperationResult<ReportSimpleDTO>(selectOperationResult.Items.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData)));
            });
        }

        [HttpPost(nameof(GetSimpleReportProperties))]
        public IEnumerable<ReportPropertySimpleDTO> GetSimpleReportProperties([FromBody] Guid reportIdentity)
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

        [HttpPost(nameof(GetSimpleReportFilters))]
        public IEnumerable<ReportFilterSimpleDTO> GetSimpleReportFilters([FromBody] Guid reportIdentity)
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

        [HttpPost(nameof(GetSimpleReportParameters))]

        public IEnumerable<ReportParameterSimpleDTO> GetSimpleReportParameters([FromBody] Guid reportIdentity)
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

        [HttpPost(nameof(GetRichReportGenerationRequestModel))]
        public ReportGenerationRequestModelRichDTO GetRichReportGenerationRequestModel([FromBody] Guid reportIdentity)
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

        [HttpPost(nameof(GetSimpleReportParameterValues))]
        public SelectOperationResult<ReportParameterValueSimpleDTO> GetSimpleReportParameterValues(GetSimpleReportParameterValuesRequest request)
        {
            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var context = evaludatedData.Context.Configuration;

                var parameter = context.Logics.ReportParameter.GetById(request.identity.Id, true, z => z.Select(q => q.Report));

                this.CreateReportLogic(evaludatedData.Context, BLLSecurityMode.View).CheckAccess(parameter.Report);

                var selectOperation = context.StandartExpressionBuilder.ToTyped<ReportParameterValue>(SelectOperation.Parse(request.odataQueryString));

                var reportServiceContext = this.GetReportServiceContext(evaludatedData.Context);

                var result = this.reportParameterValueService.GetParameterValuesBy(reportServiceContext, parameter, selectOperation);

                return new SelectOperationResult<ReportParameterValueSimpleDTO>(result.Items.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData)), result.TotalCount);
            });
        }

        [HttpPost(nameof(GetSimpleReportParameterValuesByTypeName))]
        public SelectOperationResult<ReportParameterValueSimpleDTO> GetSimpleReportParameterValuesByTypeName(GetSimpleReportParameterValuesByTypeNameRequest request)
        {
            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var context = evaludatedData.Context.Configuration;

                var selectOperation = context.StandartExpressionBuilder.ToTyped<ReportParameterValue>(SelectOperation.Parse(request.odataQueryString));

                var reportServiceContext = this.GetReportServiceContext(evaludatedData.Context);

                var result = this.reportParameterValueService.GetParameterValuesBy(reportServiceContext, request.typeName, selectOperation);

                return new SelectOperationResult<ReportParameterValueSimpleDTO>(result.Items.ToSimpleDTOList(this.GetConfigurationMappingService(evaludatedData)), result.TotalCount);
            });
        }

        [HttpPost(nameof(GetReportParameterValuePositions))]
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

                var result = this.reportParameterValueService.GetParameterValuePositions(reportServiceContext, parameterValues);

                return result;
            });
        }

        [HttpPost(nameof(GetReportParameterValuePosition))]
        public int GetReportParameterValuePosition(ReportParameterValueStrictDTO parameterValueStrictDTO)
        {
            return this.GetReportParameterValuePositions(new[] { parameterValueStrictDTO }).FirstOrDefault();
        }

        /// <summary> Получить позицию значения параметра по типу параметра </summary>
        /// <param name="typeName">Название типа параметра</param>
        /// <param name="id">ИД значения параметра</param>
        /// <param name="odataQueryString">OData запрос (как правило предназначен для сортировки)</param>
        /// <returns>Позизция значения параметра, если значение не было найдено, то возвращается -1</returns>
        [HttpPost(nameof(GetReportParameterValuePositionByTypeName))]
        public int GetReportParameterValuePositionByTypeName(GetReportParameterValuePositionByTypeNameRequest request)
        {
            return this.GetReportParameterValuePositionsByTypeName(new GetReportParameterValuePositionsByTypeNameRequest
            {
                typeName = request.typeName,
                ids = new[] { request.id },
                odataQueryString = request.odataQueryString
            }).Single();
        }

        /// <summary> Получить позиции значений параметров по типу параметра </summary>
        /// <param name="typeName">Название типа параметра</param>
        /// <param name="ids">ИД значений параметров</param>
        /// <param name="odataQueryString">OData запрос (как правило предназначен для сортировки)</param>
        /// <returns>Список позиций значений параметра, в том порядке, в котором идут ИД значений в списке ids (если значение не было найдено, то в позции указывается -1)</returns>
        [HttpPost(nameof(GetReportParameterValuePositionsByTypeName))]
        public IEnumerable<int> GetReportParameterValuePositionsByTypeName(GetReportParameterValuePositionsByTypeNameRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return this.Evaluate(DBSessionMode.Read, evaludatedData =>
            {
                var context = evaludatedData.Context.Configuration;

                var selectOperation = context.StandartExpressionBuilder.ToTyped<ReportParameterValue>(SelectOperation.Parse(request.odataQueryString));

                var reportServiceContext = this.GetReportServiceContext(evaludatedData.Context);

                var parameterValues = this.reportParameterValueService.GetParameterValuesBy(reportServiceContext, request.typeName, selectOperation);

                var values = parameterValues.Items.Select((item, index) => new { Id = Guid.Parse(item.Value), Index = index });

                return from id in request.ids
                       join value in values on id equals value.Id into tmpValue
                       from value in tmpValue.DefaultIfEmpty()
                       select value != null ? value.Index : -1;
            });
        }

        [HttpPost(nameof(SaveReport))]
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

        [HttpPost(nameof(RemoveReport))]
        public void RemoveReport(ReportIdentityDTO reportIdent)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var bll = this.CreateReportLogic(evaluateData.Context, BLLSecurityMode.Edit);

                var domainObject = bll.GetById(reportIdent.Id, true);

                bll.Remove(domainObject);
            });
        }

        [HttpPost(nameof(GetRichReport))]
        public ReportRichDTO GetRichReport(ReportIdentityDTO reportIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var bll = this.CreateReportLogic(evaluateData.Context, BLLSecurityMode.View);

                var domainObject = bll.GetById(reportIdentity.Id, true, evaluateData.Context.Configuration.FetchService.GetContainer<Configuration.Domain.Reports.Report>(MainDTOType.FullDTO));

                return domainObject.ToRichDTO(this.GetConfigurationMappingService(evaluateData));
            });
        }

        [HttpPost(nameof(GetStream))]
        public virtual FileStreamResult GetStream(ReportGenerationModelStrictDTO modelDTO)
        {
            return this.Evaluate(DBSessionMode.Read, eval =>
            {
                var context = eval.Context;

                var reportServiceContext = this.GetReportServiceContext(context);

                var model = modelDTO.ToDomainObject(this.GetConfigurationMappingService(context.Configuration));

                this.CreateReportLogic(context, BLLSecurityMode.View).CheckAccess(model.Report);

                model.PredefineGenerationValues = this.GetPredefineFilterParameters(model, reportServiceContext);

                var reportStream = this.reportService.GetReportStream(model, reportServiceContext);

                return this.GetReportResult(reportStream);
            });
        }

        [HttpPost(nameof(GetTypeMetadatas))]
        public IEnumerable<TypeMetadata> GetTypeMetadatas()
        {
            return this.systemMetadataTypeBuilder.SystemMetadata.Types;
        }


        protected virtual IEnumerable<ReportGenerationPredefineValue> GetPredefineFilterParameters(ReportGenerationModel model, ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext)
        {
            if (model.Report.Filters.Any(z => !z.IsValueFromParameters))
            {
                var reportDomainType = this.systemMetadataTypeBuilder.TypeResolver.Resolve(new TypeHeader(model.Report.DomainTypeName));

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

                        yield return new ReportGenerationPredefineValue(propertyDomainType.Type.Name, this.reportParameterValueService.GetDesignValue(reportServiceContext, expectedId, propertyTypeHeader));
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

        private void InitDict()
        {
            this.typeMetadataDictLazy = new Lazy<Dictionary<string, TypeMetadata>>(() => this.GetTypeMetadataDict(), true);
        }

        private ReportServiceContext<TBLLContext, TSecurityOperationCode> GetReportServiceContext(TBLLContext context)
        {
            return new ReportServiceContext<TBLLContext, TSecurityOperationCode>(context, this.systemMetadataTypeBuilder, this.securityOperationCodeProvider);
        }

        private Dictionary<string, TypeMetadata> GetTypeMetadataDict()
        {
            return this.systemMetadataTypeBuilder.SystemMetadata.Types.ToDictionary(z => z.Type.Name, z => z);
        }
    }
}
