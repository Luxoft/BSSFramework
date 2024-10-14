namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class PrincipalController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Principal (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdentsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principals (FullDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsByOperationInternal(securityRule, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByOperationInternal(Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(securityRule);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdentsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principals (SimpleDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsByOperationInternal(securityRule, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByOperationInternal(Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(securityRule);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save Principals
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SavePrincipalInternal(principalStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
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
}
