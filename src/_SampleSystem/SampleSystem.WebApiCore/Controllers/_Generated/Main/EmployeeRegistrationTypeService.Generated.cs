namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeeRegistrationTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check EmployeeRegistrationType access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckEmployeeRegistrationTypeAccess")]
        public virtual void CheckEmployeeRegistrationTypeAccess(CheckEmployeeRegistrationTypeAccessAutoRequest checkEmployeeRegistrationTypeAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkEmployeeRegistrationTypeAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdent = checkEmployeeRegistrationTypeAccessAutoRequest.employeeRegistrationTypeIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckEmployeeRegistrationTypeAccessInternal(employeeRegistrationTypeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckEmployeeRegistrationTypeAccessInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationType;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetById(employeeRegistrationTypeIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EmployeeRegistrationType>(securityOperationCode), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRegistrationType")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypeInternal(employeeRegistrationTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRegistrationTypeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRegistrationTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypeByNameInternal(employeeRegistrationTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationTypeByNameInternal(string employeeRegistrationTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRegistrationTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationTypeInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetById(employeeRegistrationTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRegistrationTypes (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRegistrationTypes")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRegistrationTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO[] employeeRegistrationTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypesByIdentsInternal(employeeRegistrationTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO[] employeeRegistrationTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeeRegistrationTypeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO> GetFullEmployeeRegistrationTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRegistrationType")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeRichDTO GetRichEmployeeRegistrationType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRegistrationTypeInternal(employeeRegistrationTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRegistrationTypeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeRichDTO GetRichEmployeeRegistrationTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRegistrationTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRegistrationTypeByNameInternal(employeeRegistrationTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeRichDTO GetRichEmployeeRegistrationTypeByNameInternal(string employeeRegistrationTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRegistrationTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeRichDTO GetRichEmployeeRegistrationTypeInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetById(employeeRegistrationTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRegistrationType")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO GetSimpleEmployeeRegistrationType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypeInternal(employeeRegistrationTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRegistrationTypeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO GetSimpleEmployeeRegistrationTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRegistrationTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypeByNameInternal(employeeRegistrationTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO GetSimpleEmployeeRegistrationTypeByNameInternal(string employeeRegistrationTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRegistrationTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO GetSimpleEmployeeRegistrationTypeInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetById(employeeRegistrationTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRegistrationTypes (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRegistrationTypes")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRegistrationTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO[] employeeRegistrationTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypesByIdentsInternal(employeeRegistrationTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO[] employeeRegistrationTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeeRegistrationTypeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO> GetSimpleEmployeeRegistrationTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRegistrationType")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO GetVisualEmployeeRegistrationType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypeInternal(employeeRegistrationTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRegistrationTypeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO GetVisualEmployeeRegistrationTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRegistrationTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypeByNameInternal(employeeRegistrationTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO GetVisualEmployeeRegistrationTypeByNameInternal(string employeeRegistrationTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRegistrationTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO GetVisualEmployeeRegistrationTypeInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetById(employeeRegistrationTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRegistrationTypes (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRegistrationTypes")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationTypes (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRegistrationTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO[] employeeRegistrationTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypesByIdentsInternal(employeeRegistrationTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO[] employeeRegistrationTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(employeeRegistrationTypeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO> GetVisualEmployeeRegistrationTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRegistrationType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for EmployeeRegistrationType
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasEmployeeRegistrationTypeAccess")]
        public virtual bool HasEmployeeRegistrationTypeAccess(HasEmployeeRegistrationTypeAccessAutoRequest hasEmployeeRegistrationTypeAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasEmployeeRegistrationTypeAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdent = hasEmployeeRegistrationTypeAccessAutoRequest.employeeRegistrationTypeIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasEmployeeRegistrationTypeAccessInternal(employeeRegistrationTypeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasEmployeeRegistrationTypeAccessInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationType;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetById(employeeRegistrationTypeIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EmployeeRegistrationType>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckEmployeeRegistrationTypeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasEmployeeRegistrationTypeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
