namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeeSpecializationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public EmployeeSpecializationController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check EmployeeSpecialization access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckEmployeeSpecializationAccess")]
        public virtual void CheckEmployeeSpecializationAccess(CheckEmployeeSpecializationAccessAutoRequest checkEmployeeSpecializationAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkEmployeeSpecializationAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdent = checkEmployeeSpecializationAccessAutoRequest.employeeSpecializationIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckEmployeeSpecializationAccessInternal(employeeSpecializationIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckEmployeeSpecializationAccessInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecialization;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetById(employeeSpecializationIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EmployeeSpecialization>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeSpecialization")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecialization([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeSpecializationInternal(employeeSpecializationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeSpecializationByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeSpecializationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeSpecializationByNameInternal(employeeSpecializationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationByNameInternal(string employeeSpecializationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeSpecializationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetById(employeeSpecializationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeSpecializations (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeSpecializations")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO> GetFullEmployeeSpecializations()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeSpecializationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeSpecializations (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeSpecializationsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO> GetFullEmployeeSpecializationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO[] employeeSpecializationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeSpecializationsByIdentsInternal(employeeSpecializationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO> GetFullEmployeeSpecializationsByIdentsInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO[] employeeSpecializationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeeSpecializationIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO> GetFullEmployeeSpecializationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeSpecialization")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecialization([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeSpecializationInternal(employeeSpecializationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeSpecializationByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeSpecializationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeSpecializationByNameInternal(employeeSpecializationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationByNameInternal(string employeeSpecializationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeSpecializationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetById(employeeSpecializationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeSpecialization")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecialization([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeSpecializationInternal(employeeSpecializationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeSpecializationByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeSpecializationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeSpecializationByNameInternal(employeeSpecializationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationByNameInternal(string employeeSpecializationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeSpecializationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetById(employeeSpecializationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeSpecializations (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeSpecializations")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO> GetSimpleEmployeeSpecializations()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeSpecializationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeSpecializations (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeSpecializationsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO> GetSimpleEmployeeSpecializationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO[] employeeSpecializationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeSpecializationsByIdentsInternal(employeeSpecializationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO> GetSimpleEmployeeSpecializationsByIdentsInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO[] employeeSpecializationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeeSpecializationIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO> GetSimpleEmployeeSpecializationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeSpecialization")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecialization([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeSpecializationInternal(employeeSpecializationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeSpecializationByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeSpecializationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeSpecializationByNameInternal(employeeSpecializationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationByNameInternal(string employeeSpecializationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeSpecializationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetById(employeeSpecializationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeSpecializations (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeSpecializations")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO> GetVisualEmployeeSpecializations()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeSpecializationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeSpecializations (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeSpecializationsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO> GetVisualEmployeeSpecializationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO[] employeeSpecializationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeSpecializationsByIdentsInternal(employeeSpecializationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO> GetVisualEmployeeSpecializationsByIdentsInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO[] employeeSpecializationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(employeeSpecializationIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO> GetVisualEmployeeSpecializationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeSpecialization>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for EmployeeSpecialization
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasEmployeeSpecializationAccess")]
        public virtual bool HasEmployeeSpecializationAccess(HasEmployeeSpecializationAccessAutoRequest hasEmployeeSpecializationAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasEmployeeSpecializationAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdent = hasEmployeeSpecializationAccessAutoRequest.employeeSpecializationIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasEmployeeSpecializationAccessInternal(employeeSpecializationIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasEmployeeSpecializationAccessInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecialization;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetById(employeeSpecializationIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EmployeeSpecialization>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckEmployeeSpecializationAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasEmployeeSpecializationAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
