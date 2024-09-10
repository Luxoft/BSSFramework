namespace Authorization.WebApi.Controllers
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("authApi/[controller]/[action]")]
    public partial class PrincipalController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>
    {
        
        /// <summary>
        /// Get Principal (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdentsInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdentsInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsByIdentsInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove Principal
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemovePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemovePrincipalInternal(principalIdent, evaluateData));
        }
        
        protected virtual void RemovePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            this.RemovePrincipalInternal(principalIdent, evaluateData, bll);
        }
        
        protected virtual void RemovePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IPrincipalBLL bll)
        {
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Principals
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalStrictDTO principalStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SavePrincipalInternal(principalStrict, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            return this.SavePrincipalInternal(principalStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IPrincipalBLL bll)
        {
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, principalStrict.Id);
            principalStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
