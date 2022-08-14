namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
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
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            SampleSystem.Generated.DTO.BusinessUnitProgramClassFilterModelStrictDTO filter = getBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest.filter;
            string odataQueryString = getBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitProgramClassesByODataQueryStringWithFilterInternal(odataQueryString, filter, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByODataQueryStringWithFilterInternal(string odataQueryString, SampleSystem.Generated.DTO.BusinessUnitProgramClassFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode = getBusinessUnitProgramClassesByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getBusinessUnitProgramClassesByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitProgramClassesByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO> GetBusinessUnitProgramClassesByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
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
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode = getBusinessUnitProgramClassTreeByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getBusinessUnitProgramClassTreeByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitProgramClassTreeByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.BusinessUnitProgramClassProjectionDTO, System.Guid>> GetBusinessUnitProgramClassTreeByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitProgramClassBLL bll = evaluateData.Context.Logics.BusinessUnitProgramClassFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.BusinessUnitProgramClass> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.BusinessUnitProgramClass>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.SelectN(odataTree, businessUnitProgramClass => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(businessUnitProgramClass, evaluateData.MappingService));
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
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode = getTestBusinessUnitsByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getTestBusinessUnitsByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitsByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO> GetTestBusinessUnitsByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
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
            SampleSystem.Generated.DTO.HierarchicalBusinessUnitFilterModelStrictDTO filter = getTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest.filter;
            string odataQueryString = getTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterInternal(odataQueryString, filter, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterInternal(string odataQueryString, SampleSystem.Generated.DTO.HierarchicalBusinessUnitFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Models.Filters.HierarchicalBusinessUnitFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestBusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestBusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestBusinessUnit>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.SelectN(odataTree, testBusinessUnit => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testBusinessUnit, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get hierarchical data of type TestBusinessUnits (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestBusinessUnitTreeByOperation")]
        public virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByOperation(GetTestBusinessUnitTreeByOperationAutoRequest getTestBusinessUnitTreeByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode = getTestBusinessUnitTreeByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getTestBusinessUnitTreeByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestBusinessUnitTreeByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestBusinessUnitProjectionDTO, System.Guid>> GetTestBusinessUnitTreeByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestBusinessUnitBLL bll = evaluateData.Context.Logics.TestBusinessUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestBusinessUnit> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestBusinessUnit>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestBusinessUnit>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.SelectN(odataTree, testBusinessUnit => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testBusinessUnit, evaluateData.MappingService));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.BusinessUnitProgramClassFilterModelStrictDTO filter;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitProgramClassesByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitProgramClassTreeByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestBusinessUnitsByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestBusinessUnitTreeByODataQueryStringWithHierarchicalBusinessUnitFilterAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.HierarchicalBusinessUnitFilterModelStrictDTO filter;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestBusinessUnitTreeByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemBusinessUnitSecurityOperationCode securityOperationCode;
    }
}
