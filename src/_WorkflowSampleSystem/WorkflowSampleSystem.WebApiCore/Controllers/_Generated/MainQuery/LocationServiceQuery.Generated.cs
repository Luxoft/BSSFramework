namespace WorkflowSampleSystem.WebApiCore.Controllers.MainQuery
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class LocationQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public LocationQueryController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get Locations (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocationsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationFullDTO> GetFullLocationsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullLocationsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationFullDTO> GetFullLocationsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.Location> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.Location>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.Location> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationFullDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Locations (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocationsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocationsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleLocationsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocationsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.Location> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.Location>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.Location> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationSimpleDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Locations (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocationsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocationsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualLocationsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocationsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<WorkflowSampleSystem.Domain.Location> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<WorkflowSampleSystem.Domain.Location>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<WorkflowSampleSystem.Domain.Location> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<WorkflowSampleSystem.Generated.DTO.LocationVisualDTO>(WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
