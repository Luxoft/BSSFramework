namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class CompanyLegalEntityQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get CompanyLegalEntities (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.CompanyLegalEntity> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.CompanyLegalEntity>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.CompanyLegalEntity> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.CompanyLegalEntity>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.CompanyLegalEntity> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.CompanyLegalEntity>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.CompanyLegalEntity> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.CompanyLegalEntity>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.CompanyLegalEntity> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.CompanyLegalEntity>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.CompanyLegalEntity> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.CompanyLegalEntity>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get CustomCompanyLegalEntities (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO> GetCustomCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetCustomCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO> GetCustomCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICustomCompanyLegalEntityBLL bll = evaluateData.Context.Logics.CustomCompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.Projections.CustomCompanyLegalEntity> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Projections.CustomCompanyLegalEntity>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.Projections.CustomCompanyLegalEntity> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Projections.CustomCompanyLegalEntity>(Framework.BLL.Domain.DTO.ViewDTOType.ProjectionDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
