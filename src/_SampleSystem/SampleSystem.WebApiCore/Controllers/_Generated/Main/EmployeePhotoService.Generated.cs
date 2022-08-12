namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeePhotoController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check EmployeePhoto access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckEmployeePhotoAccess")]
        public virtual void CheckEmployeePhotoAccess(CheckEmployeePhotoAccessAutoRequest checkEmployeePhotoAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkEmployeePhotoAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdent = checkEmployeePhotoAccessAutoRequest.employeePhotoIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckEmployeePhotoAccessInternal(employeePhotoIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckEmployeePhotoAccessInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhoto;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EmployeePhoto>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Get EmployeePhoto (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePhoto")]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhoto([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotoInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhotoInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeePhotos (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePhotos")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotos()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotosInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePhotos (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePhotosByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotosByIdentsInternal(employeePhotoIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosByIdentsInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeePhotoIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeePhoto")]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhoto([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeePhotoInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhotoInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePhoto")]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhoto([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotoInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhotoInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeePhotos (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePhotos")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotos()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotosInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePhotos (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePhotosByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotosByIdentsInternal(employeePhotoIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosByIdentsInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeePhotoIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for EmployeePhoto
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasEmployeePhotoAccess")]
        public virtual bool HasEmployeePhotoAccess(HasEmployeePhotoAccessAutoRequest hasEmployeePhotoAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasEmployeePhotoAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdent = hasEmployeePhotoAccessAutoRequest.employeePhotoIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasEmployeePhotoAccessInternal(employeePhotoIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasEmployeePhotoAccessInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhoto;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EmployeePhoto>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckEmployeePhotoAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasEmployeePhotoAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
