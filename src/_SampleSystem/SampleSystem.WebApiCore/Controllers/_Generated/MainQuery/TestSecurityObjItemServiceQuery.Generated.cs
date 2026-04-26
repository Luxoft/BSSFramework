namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class TestSecurityObjItemQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecurityObjItems (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO> GetFullTestSecurityObjItemsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestSecurityObjItemsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO> GetFullTestSecurityObjItemsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecurityObjItem> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecurityObjItem>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecurityObjItem> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecurityObjItem>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecurityObjItems (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO> GetSimpleTestSecurityObjItemsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecurityObjItemsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO> GetSimpleTestSecurityObjItemsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecurityObjItem> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecurityObjItem>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecurityObjItem> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecurityObjItem>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecurityObjItems (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO> GetVisualTestSecurityObjItemsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestSecurityObjItemsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO> GetVisualTestSecurityObjItemsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.TestDependency.TestSecurityObjItem> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.TestDependency.TestSecurityObjItem>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.TestDependency.TestSecurityObjItem> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestSecurityObjItem>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestSecurityObjItemProjections (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemProjectionDTO> GetTestSecurityObjItemProjectionsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemProjectionsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemProjectionDTO> GetTestSecurityObjItemProjectionsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemProjectionBLL bll = evaluateData.Context.Logics.TestSecurityObjItemProjectionFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.Projections.TestSecurityObjItemProjection> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Projections.TestSecurityObjItemProjection>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.Projections.TestSecurityObjItemProjection> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Projections.TestSecurityObjItemProjection>(Framework.BLL.Domain.DTO.ViewDTOType.ProjectionDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestSecurityObjItemProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
