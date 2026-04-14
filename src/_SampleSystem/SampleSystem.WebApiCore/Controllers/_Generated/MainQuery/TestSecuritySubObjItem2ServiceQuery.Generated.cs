namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class TestSecuritySubObjItem2QueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem2sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem2sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem2sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem2>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
