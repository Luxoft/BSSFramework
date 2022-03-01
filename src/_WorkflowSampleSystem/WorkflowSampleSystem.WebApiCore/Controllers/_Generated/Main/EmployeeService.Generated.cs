namespace WorkflowSampleSystem.WebApiCore.Controllers.Main
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public EmployeeController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check Employee access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckEmployeeAccess")]
        public virtual void CheckEmployeeAccess(CheckEmployeeAccessAutoRequest checkEmployeeAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = checkEmployeeAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent = checkEmployeeAccessAutoRequest.employeeIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckEmployeeAccessInternal(employeeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckEmployeeAccessInternal(WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.Employee;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.Employee>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get Employee (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployee")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeFullDTO GetFullEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeInternal(employeeIdentity, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeFullDTO GetFullEmployeeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Employees (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployees")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployees()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Employees (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeesByIdentsInternal(employeeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeeIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Employee (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployee")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeSimpleDTO GetSimpleEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeInternal(employeeIdentity, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeSimpleDTO GetSimpleEmployeeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Employees (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployees")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployees()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Employees (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesByIdentsInternal(employeeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeeIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Employee
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasEmployeeAccess")]
        public virtual bool HasEmployeeAccess(HasEmployeeAccessAutoRequest hasEmployeeAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = hasEmployeeAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent = hasEmployeeAccessAutoRequest.employeeIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasEmployeeAccessInternal(employeeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasEmployeeAccessInternal(WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.Employee;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.Employee>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save Employees
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveEmployee")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeStrictDTO employeeStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveEmployeeInternal(employeeStrict, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeStrictDTO employeeStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveEmployeeInternal(employeeStrict, evaluateData, bll);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeStrictDTO employeeStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData, WorkflowSampleSystem.BLL.IEmployeeBLL bll)
        {
            WorkflowSampleSystem.Domain.Employee domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, employeeStrict.Id);
            employeeStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Update Employee
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("UpdateEmployee")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO UpdateEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeUpdateDTO employeeUpdate)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.UpdateEmployeeInternal(employeeUpdate, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO UpdateEmployeeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeUpdateDTO employeeUpdate, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            WorkflowSampleSystem.Domain.Employee domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, employeeUpdate.Id);
            employeeUpdate.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckEmployeeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasEmployeeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
}
