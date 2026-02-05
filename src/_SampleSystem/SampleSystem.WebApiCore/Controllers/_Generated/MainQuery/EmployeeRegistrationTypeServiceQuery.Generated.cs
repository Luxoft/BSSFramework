namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class EmployeeRegistrationTypeQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.EmployeeRegistrationType> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.EmployeeRegistrationType>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.EmployeeRegistrationType> preResult = bll.GetObjectsByOData(typedSelectOperation, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.EmployeeRegistrationType> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.EmployeeRegistrationType>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.EmployeeRegistrationType> preResult = bll.GetObjectsByOData(typedSelectOperation, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.EmployeeRegistrationType> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.EmployeeRegistrationType>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.EmployeeRegistrationType> preResult = bll.GetObjectsByOData(typedSelectOperation, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
