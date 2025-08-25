namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class CompanyLegalEntityQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get CompanyLegalEntities (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.CompanyLegalEntity> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.CompanyLegalEntity>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.CompanyLegalEntity> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.CompanyLegalEntity> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.CompanyLegalEntity>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.CompanyLegalEntity> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.CompanyLegalEntity> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.CompanyLegalEntity>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.CompanyLegalEntity> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get CustomCompanyLegalEntities (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO> GetCustomCompanyLegalEntitiesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCustomCompanyLegalEntitiesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO> GetCustomCompanyLegalEntitiesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICustomCompanyLegalEntityBLL bll = evaluateData.Context.Logics.CustomCompanyLegalEntityFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.CustomCompanyLegalEntity> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.CustomCompanyLegalEntity>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Projections.CustomCompanyLegalEntity> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.CustomCompanyLegalEntity>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
