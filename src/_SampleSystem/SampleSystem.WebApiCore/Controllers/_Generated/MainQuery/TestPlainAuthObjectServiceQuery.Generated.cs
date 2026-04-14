namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class TestPlainAuthObjectQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestPlainAuthObjects (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO> GetFullTestPlainAuthObjectsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestPlainAuthObjectsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO> GetFullTestPlainAuthObjectsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestPlainAuthObjects (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO> GetSimpleTestPlainAuthObjectsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestPlainAuthObjectsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO> GetSimpleTestPlainAuthObjectsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestPlainAuthObjects (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO> GetVisualTestPlainAuthObjectsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestPlainAuthObjectsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO> GetVisualTestPlainAuthObjectsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDeserializedAuth.TestPlainAuthObject>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
