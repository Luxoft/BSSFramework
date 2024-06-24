namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]")]
    public partial class TestSecuritySubObjItem2QueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem2sByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem2sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2sByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.TestSecuritySubObjItem2> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.TestSecuritySubObjItem2>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.TestSecuritySubObjItem2> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem2sByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem2sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2sByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.TestSecuritySubObjItem2> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.TestSecuritySubObjItem2>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.TestSecuritySubObjItem2> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem2sByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem2sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2sByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.TestSecuritySubObjItem2> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.TestSecuritySubObjItem2>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.TestSecuritySubObjItem2> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
