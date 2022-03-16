namespace AttachmentsSampleSystem.WebApiCore.Controllers.Main
{
    using AttachmentsSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class HRDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext>, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>>
    {
        
        public HRDepartmentController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check HRDepartment access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckHRDepartmentAccess")]
        public virtual void CheckHRDepartmentAccess(CheckHRDepartmentAccessAutoRequest checkHRDepartmentAccessAutoRequest)
        {
            AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode securityOperationCode = checkHRDepartmentAccessAutoRequest.securityOperationCode;
            AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent = checkHRDepartmentAccessAutoRequest.hRDepartmentIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckHRDepartmentAccessInternal(hRDepartmentIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckHRDepartmentAccessInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent, AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartment;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<AttachmentsSampleSystem.Domain.HRDepartment>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService>(session, context, new AttachmentsSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartment")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentByCode")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentByName")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of HRDepartments (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartments")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartments (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentsByIdentsInternal(hRDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByIdentsInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(hRDepartmentIdents, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartments (FullDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByOperationInternal(AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.Security.TransferEnumHelper.Convert<AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode, AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode>(securityOperationCode));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichHRDepartment")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichHRDepartmentByCode")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichHRDepartmentByName")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartment")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentByCode")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentByName")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of HRDepartments (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartments")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartments (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentsByIdentsInternal(hRDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByIdentsInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(hRDepartmentIdents, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartments (SimpleDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByOperationInternal(AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.Security.TransferEnumHelper.Convert<AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode, AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode>(securityOperationCode));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartment")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentByCode")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentByName")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of HRDepartments (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartments")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartments (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentsByIdentsInternal(hRDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByIdentsInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(hRDepartmentIdents, evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartments (VisualDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByOperationInternal(AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.Security.TransferEnumHelper.Convert<AttachmentsSampleSystem.Generated.DTO.AttachmentsSampleSystemHRDepartmentSecurityOperationCode, AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode>(securityOperationCode));
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<AttachmentsSampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<AttachmentsSampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for HRDepartment
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasHRDepartmentAccess")]
        public virtual bool HasHRDepartmentAccess(HasHRDepartmentAccessAutoRequest hasHRDepartmentAccessAutoRequest)
        {
            AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode securityOperationCode = hasHRDepartmentAccessAutoRequest.securityOperationCode;
            AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent = hasHRDepartmentAccessAutoRequest.hRDepartmentIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasHRDepartmentAccessInternal(hRDepartmentIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasHRDepartmentAccessInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent, AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartment;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<AttachmentsSampleSystem.Domain.HRDepartment>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove HRDepartment
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveHRDepartment")]
        public virtual void RemoveHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveHRDepartmentInternal(hRDepartmentIdent, evaluateData));
        }
        
        protected virtual void RemoveHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveHRDepartmentInternal(hRDepartmentIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData, AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll)
        {
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save HRDepartments
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveHRDepartment")]
        public virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO SaveHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] AttachmentsSampleSystem.Generated.DTO.HRDepartmentStrictDTO hRDepartmentStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveHRDepartmentInternal(hRDepartmentStrict, evaluateData));
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO SaveHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentStrictDTO hRDepartmentStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData)
        {
            AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveHRDepartmentInternal(hRDepartmentStrict, evaluateData, bll);
        }
        
        protected virtual AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO SaveHRDepartmentInternal(AttachmentsSampleSystem.Generated.DTO.HRDepartmentStrictDTO hRDepartmentStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<AttachmentsSampleSystem.BLL.IAttachmentsSampleSystemBLLContext, AttachmentsSampleSystem.Generated.DTO.IAttachmentsSampleSystemDTOMappingService> evaluateData, AttachmentsSampleSystem.BLL.IHRDepartmentBLL bll)
        {
            AttachmentsSampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentStrict.Id, true);
            hRDepartmentStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return AttachmentsSampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckHRDepartmentAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasHRDepartmentAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public AttachmentsSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public AttachmentsSampleSystem.AttachmentsSampleSystemSecurityOperationCode securityOperationCode;
    }
}
