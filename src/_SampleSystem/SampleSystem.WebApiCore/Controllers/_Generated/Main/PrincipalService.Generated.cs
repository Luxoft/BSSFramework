namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class PrincipalController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check Principal access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckPrincipalAccess")]
        public virtual void CheckPrincipalAccess(CheckPrincipalAccessAutoRequest checkPrincipalAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkPrincipalAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdent = checkPrincipalAccessAutoRequest.principalIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckPrincipalAccessInternal(principalIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckPrincipalAccessInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.Principal;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.Principal>(securityOperationCode), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get Principal (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipal")]
        public virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipals")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipalsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdentsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPrincipal")]
        public virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipal")]
        public virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipals")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipalsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdentsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Principal
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasPrincipalAccess")]
        public virtual bool HasPrincipalAccess(HasPrincipalAccessAutoRequest hasPrincipalAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasPrincipalAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdent = hasPrincipalAccessAutoRequest.principalIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasPrincipalAccessInternal(principalIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasPrincipalAccessInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.Principal;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.Principal>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save Principals
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SavePrincipal")]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SavePrincipalInternal(principalStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SavePrincipalInternal(principalStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IPrincipalBLL bll)
        {
            SampleSystem.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, principalStrict.Id);
            principalStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckPrincipalAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasPrincipalAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
