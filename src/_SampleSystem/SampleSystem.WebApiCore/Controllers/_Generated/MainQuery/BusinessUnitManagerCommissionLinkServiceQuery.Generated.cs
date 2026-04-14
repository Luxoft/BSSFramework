namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class BusinessUnitManagerCommissionLinkQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLinks (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO> GetFullBusinessUnitManagerCommissionLinksByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitManagerCommissionLinksByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO> GetFullBusinessUnitManagerCommissionLinksByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLinks (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO> GetSimpleBusinessUnitManagerCommissionLinksByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitManagerCommissionLinksByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO> GetSimpleBusinessUnitManagerCommissionLinksByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.BU.BusinessUnitManagerCommissionLink>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
