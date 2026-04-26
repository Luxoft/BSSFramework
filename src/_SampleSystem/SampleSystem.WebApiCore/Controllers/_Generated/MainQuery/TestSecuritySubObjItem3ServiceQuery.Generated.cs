namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class TestSecuritySubObjItem3QueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem3s (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO> GetFullTestSecuritySubObjItem3sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO> GetFullTestSecuritySubObjItem3sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3s (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO> GetSimpleTestSecuritySubObjItem3sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO> GetSimpleTestSecuritySubObjItem3sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3s (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO> GetVisualTestSecuritySubObjItem3sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO> GetVisualTestSecuritySubObjItem3sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecuritySubObjItem3>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
