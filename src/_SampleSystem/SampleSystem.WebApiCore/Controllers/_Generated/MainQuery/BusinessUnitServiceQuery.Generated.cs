namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class BusinessUnitQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnits (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnits (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnits (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.BusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.BusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.BusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnitProgramClasses (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitProgramClassesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.BusinessUnitProgramClass> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Projections.BusinessUnitProgramClass> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get BusinessUnitProgramClasses (ProjectionDTO) by odata string and filter (BusinessUnitProgramClassFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByODataQueryStringWithFilter(GetBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest getBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest)
        {
            SampleSystem.Generated.DTO.BusinessUnitProgramClassFilterModelStrictDTO filter = getBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest.Filter;
            string odataQueryString = getBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitProgramClassesByODataQueryStringWithFilterInternal(odataQueryString, filter, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByODataQueryStringWithFilterInternal(string odataQueryString, SampleSystem.Generated.DTO.BusinessUnitProgramClassFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.BusinessUnitProgramClass> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            SampleSystem.Domain.Models.Filters.BusinessUnitProgramClassFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Projections.BusinessUnitProgramClass> preResult = bll.GetObjectsByOData(typedSelectOperation, typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestBusinessUnits (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO> GetTestBusinessUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO> GetTestBusinessUnitsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestBusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestBusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Projections.TestBusinessUnit> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestBusinessUnit>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get hierarchical data of type TestBusinessUnits (ProjectionDTO) by odata string and filter (HierarchicalBusinessUnitFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilter(GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest getTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest)
        {
            SampleSystem.Generated.DTO.HierarchicalBusinessUnitFilterModelStrictDTO filter = getTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest.Filter;
            string odataQueryString = getTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterInternal(odataQueryString, filter, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterInternal(string odataQueryString, SampleSystem.Generated.DTO.HierarchicalBusinessUnitFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Models.Filters.HierarchicalBusinessUnitFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestBusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestBusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestBusinessUnit>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.ChangeItem(odataTree, testBusinessUnit => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testBusinessUnit, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get hierarchical data of type TestBusinessUnits (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByOperation(GetTestBusinessUnitTreeByOperationAutoRequest getTestBusinessUnitTreeByOperationAutoRequest)
        {
            Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule = getTestBusinessUnitTreeByOperationAutoRequest.SecurityRule;
            string odataQueryString = getTestBusinessUnitTreeByOperationAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitTreeByOperationInternal(odataQueryString, securityRule, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByOperationInternal(string odataQueryString, Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(securityRule);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestBusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestBusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestBusinessUnit>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.ChangeItem(odataTree, testBusinessUnit => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testBusinessUnit, evaluateData.MappingService));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest
    {
        
        private string odataQueryString;
        
        private SampleSystem.Generated.DTO.BusinessUnitProgramClassFilterModelStrictDTO filter;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual string OdataQueryString
        {
            get
            {
                return this.odataQueryString;
            }
            set
            {
                this.odataQueryString = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitProgramClassFilterModelStrictDTO Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest
    {
        
        private string odataQueryString;
        
        private SampleSystem.Generated.DTO.HierarchicalBusinessUnitFilterModelStrictDTO filter;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual string OdataQueryString
        {
            get
            {
                return this.odataQueryString;
            }
            set
            {
                this.odataQueryString = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual SampleSystem.Generated.DTO.HierarchicalBusinessUnitFilterModelStrictDTO Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestBusinessUnitTreeByOperationAutoRequest
    {
        
        private string odataQueryString;
        
        private Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual string OdataQueryString
        {
            get
            {
                return this.odataQueryString;
            }
            set
            {
                this.odataQueryString = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule SecurityRule
        {
            get
            {
                return this.securityRule;
            }
            set
            {
                this.securityRule = value;
            }
        }
    }
}
