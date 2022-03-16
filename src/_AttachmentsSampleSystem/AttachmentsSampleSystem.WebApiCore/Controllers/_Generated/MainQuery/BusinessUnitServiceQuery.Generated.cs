namespace AttachmentsSampleSystem.WebApiCore.Controllers.MainQuery
{
    using AttachmentsSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext>, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>>
    {
        
        public BusinessUnitQueryController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>(session, context, new AttachmentsSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get BusinessUnits (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitFullDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnits (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitSimpleDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnits (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.BusinessUnitVisualDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
