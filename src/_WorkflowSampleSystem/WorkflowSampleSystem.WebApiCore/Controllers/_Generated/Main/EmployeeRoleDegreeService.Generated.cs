namespace WorkflowSampleSystem.WebApiCore.Controllers.Main
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeeRoleDegreeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public EmployeeRoleDegreeController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check EmployeeRoleDegree access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckEmployeeRoleDegreeAccess")]
        public virtual void CheckEmployeeRoleDegreeAccess(CheckEmployeeRoleDegreeAccessAutoRequest checkEmployeeRoleDegreeAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = checkEmployeeRoleDegreeAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdent = checkEmployeeRoleDegreeAccessAutoRequest.employeeRoleDegreeIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckEmployeeRoleDegreeAccessInternal(employeeRoleDegreeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckEmployeeRoleDegreeAccessInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegree;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegree")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegreeByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRoleDegrees (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegrees")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegrees()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegrees (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegreesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegreesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreesByIdentsInternal(employeeRoleDegreeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegreesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeeRoleDegreeIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegreesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRoleDegree")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRoleDegreeByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegree")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegreeByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRoleDegrees (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegrees")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegrees()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegrees (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegreesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegreesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreesByIdentsInternal(employeeRoleDegreeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegreesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeeRoleDegreeIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegreesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegree")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegreeByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRoleDegrees (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegrees")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegrees()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegrees (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegreesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegreesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreesByIdentsInternal(employeeRoleDegreeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegreesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(employeeRoleDegreeIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegreesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for EmployeeRoleDegree
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasEmployeeRoleDegreeAccess")]
        public virtual bool HasEmployeeRoleDegreeAccess(HasEmployeeRoleDegreeAccessAutoRequest hasEmployeeRoleDegreeAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = hasEmployeeRoleDegreeAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdent = hasEmployeeRoleDegreeAccessAutoRequest.employeeRoleDegreeIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasEmployeeRoleDegreeAccessInternal(employeeRoleDegreeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasEmployeeRoleDegreeAccessInternal(WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegree;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.EmployeeRoleDegree>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckEmployeeRoleDegreeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasEmployeeRoleDegreeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
}
