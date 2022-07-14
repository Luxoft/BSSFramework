namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public EmployeeController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Change Employee by model (EmployeeComplexChangeModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("ChangeEmployeeByComplex")]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO ChangeEmployeeByComplex([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeComplexChangeModelStrictDTO employeeChangeModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.ChangeEmployeeByComplexInternal(employeeChangeModel, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO ChangeEmployeeByComplexInternal(SampleSystem.Generated.DTO.EmployeeComplexChangeModelStrictDTO employeeChangeModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            SampleSystem.Domain.EmployeeComplexChangeModel changeModel = employeeChangeModel.ToDomainObject(evaluateData.MappingService);
            bll.CheckAccess(changeModel.PrimaryChangingObject);
            foreach(var secondaryDomainObject in changeModel.SecondaryChangingObjects)
            {
                bll.CheckAccess(secondaryDomainObject);
            }
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(bll.ComplexChange(changeModel));
        }
        
        /// <summary>
        /// Change Employee by model (EmployeeEmailChangeModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("ChangeEmployeeByEmail")]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO ChangeEmployeeByEmail([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeEmailChangeModelStrictDTO employeeChangeModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.ChangeEmployeeByEmailInternal(employeeChangeModel, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO ChangeEmployeeByEmailInternal(SampleSystem.Generated.DTO.EmployeeEmailChangeModelStrictDTO employeeChangeModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            SampleSystem.Domain.EmployeeEmailChangeModel changeModel = employeeChangeModel.ToDomainObject(evaluateData.MappingService);
            bll.CheckAccess(changeModel.ChangingObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(bll.ChangeByEmail(changeModel));
        }
        
        /// <summary>
        /// Check Employee access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckEmployeeAccess")]
        public virtual void CheckEmployeeAccess(CheckEmployeeAccessAutoRequest checkEmployeeAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkEmployeeAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent = checkEmployeeAccessAutoRequest.employeeIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckEmployeeAccessInternal(employeeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckEmployeeAccessInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.Employee;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.Employee>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get Employee (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployee")]
        public virtual SampleSystem.Generated.DTO.EmployeeFullDTO GetFullEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeInternal(employeeIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeFullDTO GetFullEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Employees (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployees")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployees()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Employees (FullDTO) by filter (SampleSystem.Domain.Models.Filters.EmployeeFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeesByFilter")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeesByFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByFilterInternal(SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Models.Filters.EmployeeFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Employees (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeesByIdentsInternal(employeeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Employees (FullDTO) by filter (SampleSystem.Domain.EmployeeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByRootFilterInternal(SampleSystem.Generated.DTO.EmployeeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Mass Employee change model (EmployeeEmailMassChangeModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetMassChangeEmployeeByEmail")]
        public virtual SampleSystem.Generated.DTO.EmployeeEmailMassChangeModelRichDTO GetMassChangeEmployeeByEmail([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdent)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetMassChangeEmployeeByEmailInternal(employeeIdent, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeEmailMassChangeModelRichDTO GetMassChangeEmployeeByEmailInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            System.Collections.Generic.List<SampleSystem.Domain.Employee> domainObjects = bll.GetListByIdents(employeeIdent);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(bll.GetMassChangeByEmail(domainObjects), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Employee (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployee")]
        public virtual SampleSystem.Generated.DTO.EmployeeSimpleDTO GetSimpleEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeInternal(employeeIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSimpleDTO GetSimpleEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Employees (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployees")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployees()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Employees (SimpleDTO) by filter (SampleSystem.Domain.Models.Filters.EmployeeFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeesByFilter")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesByFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByFilterInternal(SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Models.Filters.EmployeeFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Employees (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesByIdentsInternal(employeeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO[] employeeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Employees (SimpleDTO) by filter (SampleSystem.Domain.EmployeeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByRootFilterInternal(SampleSystem.Generated.DTO.EmployeeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Employee>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Employee
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasEmployeeAccess")]
        public virtual bool HasEmployeeAccess(HasEmployeeAccessAutoRequest hasEmployeeAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasEmployeeAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent = hasEmployeeAccessAutoRequest.employeeIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasEmployeeAccessInternal(employeeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasEmployeeAccessInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.Employee;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.Employee domainObject = bll.GetById(employeeIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.Employee>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Mass change Employee by model (EmployeeEmailMassChangeModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("MassChangeEmployeeByEmail")]
        public virtual System.Collections.Generic.List<SampleSystem.Generated.DTO.EmployeeIdentityDTO> MassChangeEmployeeByEmail([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeEmailMassChangeModelStrictDTO employeeChangeModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.MassChangeEmployeeByEmailInternal(employeeChangeModel, evaluateData));
        }
        
        protected virtual System.Collections.Generic.List<SampleSystem.Generated.DTO.EmployeeIdentityDTO> MassChangeEmployeeByEmailInternal(SampleSystem.Generated.DTO.EmployeeEmailMassChangeModelStrictDTO employeeChangeModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            SampleSystem.Domain.EmployeeEmailMassChangeModel changeModel = employeeChangeModel.ToDomainObject(evaluateData.MappingService);
            foreach(var domainObject in changeModel.ChangingObjects)
            {
                bll.CheckAccess(domainObject);
            }
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTOList(bll.MassChangeByEmail(changeModel));
        }
        
        /// <summary>
        /// Save Employees
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveEmployee")]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeStrictDTO employeeStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveEmployeeInternal(employeeStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeStrictDTO employeeStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveEmployeeInternal(employeeStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeStrictDTO employeeStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IEmployeeBLL bll)
        {
            SampleSystem.Domain.Employee domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, employeeStrict.Id);
            employeeStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Update Employee
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("UpdateEmployee")]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO UpdateEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeUpdateDTO employeeUpdate)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.UpdateEmployeeInternal(employeeUpdate, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO UpdateEmployeeInternal(SampleSystem.Generated.DTO.EmployeeUpdateDTO employeeUpdate, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            SampleSystem.Domain.Employee domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, employeeUpdate.Id);
            employeeUpdate.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Get TestManualEmployeeProjection (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestManualEmployeeProjection")]
        public virtual SampleSystem.Generated.DTO.TestManualEmployeeProjectionDTO GetTestManualEmployeeProjection([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO testManualEmployeeProjectionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestManualEmployeeProjectionInternal(testManualEmployeeProjectionIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestManualEmployeeProjectionDTO GetTestManualEmployeeProjectionInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO testManualEmployeeProjectionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestManualEmployeeProjectionBLL bll = evaluateData.Context.Logics.TestManualEmployeeProjectionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManualProjections.TestManualEmployeeProjection domainObject = bll.GetById(testManualEmployeeProjectionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManualProjections.TestManualEmployeeProjection>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestEmployee (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestEmployee")]
        public virtual SampleSystem.Generated.DTO.TestEmployeeProjectionDTO GetTestEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO testEmployeeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestEmployeeInternal(testEmployeeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestEmployees (ProjectionDTO) by filter (SampleSystem.Domain.Models.Filters.SingleEmployeeFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestEmployeeBySingleEmployeeFilter")]
        public virtual SampleSystem.Generated.DTO.TestEmployeeProjectionDTO GetTestEmployeeBySingleEmployeeFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SingleEmployeeFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestEmployeeBySingleEmployeeFilterInternal(filter, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestEmployeeProjectionDTO GetTestEmployeeBySingleEmployeeFilterInternal(SampleSystem.Generated.DTO.SingleEmployeeFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestEmployeeBLL bll = evaluateData.Context.Logics.TestEmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Models.Filters.SingleEmployeeFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(bll.GetObjectBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestEmployee>(Framework.Transfering.ViewDTOType.ProjectionDTO)), evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestEmployeeProjectionDTO GetTestEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO testEmployeeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestEmployeeBLL bll = evaluateData.Context.Logics.TestEmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Projections.TestEmployee domainObject = bll.GetById(testEmployeeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestEmployee>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestEmployees (ProjectionDTO) by filter (SampleSystem.Domain.Models.Filters.EmployeeFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestEmployeesByEmployeeFilter")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByEmployeeFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestEmployeesByEmployeeFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByEmployeeFilterInternal(SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestEmployeeBLL bll = evaluateData.Context.Logics.TestEmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Models.Filters.EmployeeFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestEmployee>(Framework.Transfering.ViewDTOType.ProjectionDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestEmployees (ProjectionDTO) by filter (SampleSystem.Domain.TestEmployeeFilter)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestEmployeesByFilter")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestEmployeeFilterStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestEmployeesByFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByFilterInternal(SampleSystem.Generated.DTO.TestEmployeeFilterStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestEmployeeBLL bll = evaluateData.Context.Logics.TestEmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestEmployeeFilter typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestEmployee>(Framework.Transfering.ViewDTOType.ProjectionDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestLegacyEmployee (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLegacyEmployee")]
        public virtual SampleSystem.Generated.DTO.TestLegacyEmployeeProjectionDTO GetTestLegacyEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO testLegacyEmployeeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLegacyEmployeeInternal(testLegacyEmployeeIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestLegacyEmployeeProjectionDTO GetTestLegacyEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO testLegacyEmployeeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLegacyEmployeeBLL bll = evaluateData.Context.Logics.TestLegacyEmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Projections.TestLegacyEmployee domainObject = bll.GetById(testLegacyEmployeeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLegacyEmployee>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestLegacyEmployees (ProjectionDTO) by filter (SampleSystem.Domain.Models.Filters.EmployeeFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLegacyEmployeesByEmployeeFilter")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestLegacyEmployeeProjectionDTO> GetTestLegacyEmployeesByEmployeeFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLegacyEmployeesByEmployeeFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestLegacyEmployeeProjectionDTO> GetTestLegacyEmployeesByEmployeeFilterInternal(SampleSystem.Generated.DTO.EmployeeFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLegacyEmployeeBLL bll = evaluateData.Context.Logics.TestLegacyEmployeeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Models.Filters.EmployeeFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLegacyEmployee>(Framework.Transfering.ViewDTOType.ProjectionDTO)), evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckEmployeeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasEmployeeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
