namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class EmployeeRegistrationTypeQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.Employee.EmployeeRegistrationType> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Employee.EmployeeRegistrationType>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.Employee.EmployeeRegistrationType> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.EmployeeRegistrationType>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.Employee.EmployeeRegistrationType> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Employee.EmployeeRegistrationType>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.Employee.EmployeeRegistrationType> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.EmployeeRegistrationType>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.Employee.EmployeeRegistrationType> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Employee.EmployeeRegistrationType>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.Employee.EmployeeRegistrationType> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.EmployeeRegistrationType>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
