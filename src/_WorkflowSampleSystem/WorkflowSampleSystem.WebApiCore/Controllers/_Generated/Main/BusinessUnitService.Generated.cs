namespace WorkflowSampleSystem.WebApiCore.Controllers.Main
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public BusinessUnitController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check BusinessUnit access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckBusinessUnitAccess")]
        public virtual void CheckBusinessUnitAccess(CheckBusinessUnitAccessAutoRequest checkBusinessUnitAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = checkBusinessUnitAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdent = checkBusinessUnitAccessAutoRequest.businessUnitIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckBusinessUnitAccessInternal(businessUnitIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckBusinessUnitAccessInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnit;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = bll.GetById(businessUnitIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.BusinessUnit>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get BusinessUnit (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnit")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO GetFullBusinessUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitInternal(businessUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnit (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO GetFullBusinessUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitByNameInternal(businessUnitName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO GetFullBusinessUnitByNameInternal(string businessUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO GetFullBusinessUnitInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = bll.GetById(businessUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnits (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnits")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnits()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnits (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO[] businessUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitsByIdentsInternal(businessUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByIdentsInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO[] businessUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(businessUnitIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnits (FullDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsByOperationInternal(WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode>(securityOperationCode));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitFullDTO> GetFullBusinessUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnit (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnit")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitRichDTO GetRichBusinessUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitInternal(businessUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnit (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitRichDTO GetRichBusinessUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitByNameInternal(businessUnitName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitRichDTO GetRichBusinessUnitByNameInternal(string businessUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitRichDTO GetRichBusinessUnitInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = bll.GetById(businessUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnit (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnit")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO GetSimpleBusinessUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitInternal(businessUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnit (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO GetSimpleBusinessUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitByNameInternal(businessUnitName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO GetSimpleBusinessUnitByNameInternal(string businessUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO GetSimpleBusinessUnitInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = bll.GetById(businessUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnits (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnits")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnits()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnits (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO[] businessUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitsByIdentsInternal(businessUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByIdentsInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO[] businessUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(businessUnitIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnits (SimpleDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsByOperationInternal(WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode>(securityOperationCode));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitSimpleDTO> GetSimpleBusinessUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnit (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnit")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO GetVisualBusinessUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitInternal(businessUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnit (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO GetVisualBusinessUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitByNameInternal(businessUnitName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO GetVisualBusinessUnitByNameInternal(string businessUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO GetVisualBusinessUnitInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = bll.GetById(businessUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnits (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnits")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnits()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnits (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO[] businessUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitsByIdentsInternal(businessUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByIdentsInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO[] businessUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(businessUnitIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnits (VisualDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsByOperationInternal(WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemBusinessUnitSecurityOperationCode, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode>(securityOperationCode));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.BusinessUnitVisualDTO> GetVisualBusinessUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.BusinessUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for BusinessUnit
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasBusinessUnitAccess")]
        public virtual bool HasBusinessUnitAccess(HasBusinessUnitAccessAutoRequest hasBusinessUnitAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = hasBusinessUnitAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdent = hasBusinessUnitAccessAutoRequest.businessUnitIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasBusinessUnitAccessInternal(businessUnitIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasBusinessUnitAccessInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnit;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = bll.GetById(businessUnitIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.BusinessUnit>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save BusinessUnits
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveBusinessUnit")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO SaveBusinessUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitStrictDTO businessUnitStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveBusinessUnitInternal(businessUnitStrict, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO SaveBusinessUnitInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitStrictDTO businessUnitStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveBusinessUnitInternal(businessUnitStrict, evaluateData, bll);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO SaveBusinessUnitInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitStrictDTO businessUnitStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData, WorkflowSampleSystem.BLL.IBusinessUnitBLL bll)
        {
            WorkflowSampleSystem.Domain.BusinessUnit domainObject = bll.GetById(businessUnitStrict.Id, true);
            businessUnitStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckBusinessUnitAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasBusinessUnitAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
}
