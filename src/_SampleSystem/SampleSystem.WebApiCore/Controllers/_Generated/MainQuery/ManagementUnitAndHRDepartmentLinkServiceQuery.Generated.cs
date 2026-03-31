namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class ManagementUnitAndHRDepartmentLinkQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
