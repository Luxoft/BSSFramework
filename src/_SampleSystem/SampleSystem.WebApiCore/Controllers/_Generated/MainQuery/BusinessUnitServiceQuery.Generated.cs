namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class BusinessUnitQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnits (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitsByODataQueryString")]
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitsByODataQueryString")]
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitsByODataQueryString")]
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitProgramClassesByODataQueryString")]
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitProgramClassesByODataQueryStringWithFilter")]
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
        /// Get hierarchical data of type BusinessUnitProgramClasses (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitProgramClassesByOperation")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByOperation(GetBusinessUnitProgramClassesByOperationAutoRequest getBusinessUnitProgramClassesByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode = getBusinessUnitProgramClassesByOperationAutoRequest.SecurityRuleCode;
            string odataQueryString = getBusinessUnitProgramClassesByOperationAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitProgramClassesByOperationInternal(odataQueryString, securityRuleCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(evaluateData.Context.SecurityRuleParser.Parse<SampleSystem.Domain.BusinessUnit>(securityRuleCode.ToString()));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.BusinessUnitProgramClass> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataList = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.Select(odataList, businessUnitProgramClass => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(businessUnitProgramClass, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get hierarchical data of type BusinessUnitProgramClasses (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitProgramClassTreeByOperation")]
        public virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO, System.Guid>> GetBusinessUnitProgramClassTreeByOperation(GetBusinessUnitProgramClassTreeByOperationAutoRequest getBusinessUnitProgramClassTreeByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode = getBusinessUnitProgramClassTreeByOperationAutoRequest.SecurityRuleCode;
            string odataQueryString = getBusinessUnitProgramClassTreeByOperationAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitProgramClassTreeByOperationInternal(odataQueryString, securityRuleCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO, System.Guid>> GetBusinessUnitProgramClassTreeByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(evaluateData.Context.SecurityRuleParser.Parse<SampleSystem.Domain.BusinessUnit>(securityRuleCode.ToString()));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.BusinessUnitProgramClass> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.ChangeItem(odataTree, businessUnitProgramClass => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(businessUnitProgramClass, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get TestBusinessUnits (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestBusinessUnitsByODataQueryString")]
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
        /// Get hierarchical data of type TestBusinessUnits (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestBusinessUnitsByOperation")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO> GetTestBusinessUnitsByOperation(GetTestBusinessUnitsByOperationAutoRequest getTestBusinessUnitsByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode = getTestBusinessUnitsByOperationAutoRequest.SecurityRuleCode;
            string odataQueryString = getTestBusinessUnitsByOperationAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitsByOperationInternal(odataQueryString, securityRuleCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO> GetTestBusinessUnitsByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(evaluateData.Context.SecurityRuleParser.Parse<SampleSystem.Domain.BusinessUnit>(securityRuleCode.ToString()));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestBusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestBusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataList = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestBusinessUnit>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.Select(odataList, testBusinessUnit => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testBusinessUnit, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get hierarchical data of type TestBusinessUnits (ProjectionDTO) by odata string and filter (HierarchicalBusinessUnitFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilter")]
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestBusinessUnitTreeByOperation")]
        public virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByOperation(GetTestBusinessUnitTreeByOperationAutoRequest getTestBusinessUnitTreeByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode = getTestBusinessUnitTreeByOperationAutoRequest.SecurityRuleCode;
            string odataQueryString = getTestBusinessUnitTreeByOperationAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitTreeByOperationInternal(odataQueryString, securityRuleCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(evaluateData.Context.SecurityRuleParser.Parse<SampleSystem.Domain.BusinessUnit>(securityRuleCode.ToString()));
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
    public partial class GetBusinessUnitProgramClassesByOperationAutoRequest
    {
        
        private string odataQueryString;
        
        private SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode;
        
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
        public virtual SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode SecurityRuleCode
        {
            get
            {
                return this.securityRuleCode;
            }
            set
            {
                this.securityRuleCode = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitProgramClassTreeByOperationAutoRequest
    {
        
        private string odataQueryString;
        
        private SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode;
        
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
        public virtual SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode SecurityRuleCode
        {
            get
            {
                return this.securityRuleCode;
            }
            set
            {
                this.securityRuleCode = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestBusinessUnitsByOperationAutoRequest
    {
        
        private string odataQueryString;
        
        private SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode;
        
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
        public virtual SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode SecurityRuleCode
        {
            get
            {
                return this.securityRuleCode;
            }
            set
            {
                this.securityRuleCode = value;
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
        
        private SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode securityRuleCode;
        
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
        public virtual SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityRuleCode SecurityRuleCode
        {
            get
            {
                return this.securityRuleCode;
            }
            set
            {
                this.securityRuleCode = value;
            }
        }
    }
}
