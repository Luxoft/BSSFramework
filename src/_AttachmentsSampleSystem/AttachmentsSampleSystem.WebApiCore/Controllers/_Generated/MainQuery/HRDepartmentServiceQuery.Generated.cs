namespace AttachmentsSampleSystem.WebApiCore.Controllers.MainQuery
{
    using AttachmentsSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class HRDepartmentQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext>, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>>
    {
        
        public HRDepartmentQueryController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>(session, context, new AttachmentsSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get HRDepartments (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.HRDepartment> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.HRDepartment>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.HRDepartment> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get HRDepartments (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.HRDepartment> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.HRDepartment>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.HRDepartment> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get HRDepartments (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<AttachmentsSampleSystem.Domain.HRDepartment> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<AttachmentsSampleSystem.Domain.HRDepartment>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Domain.HRDepartment> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO>(AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
