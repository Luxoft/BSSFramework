namespace AttachmentsSampleSystem.WebApiCore.Controllers.MainQuery
{
    using AttachmentsSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext>, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>>
    {
        
        public EmployeeQueryController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>(session, context, new AttachmentsSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get Employees (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeesByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.Employee> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.Employee>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.Employee> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.EmployeeFullDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Employees (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeesByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.Employee> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.Employee>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.Employee> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.EmployeeSimpleDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
