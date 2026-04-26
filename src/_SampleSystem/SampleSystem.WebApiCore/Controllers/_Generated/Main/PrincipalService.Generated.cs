namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class PrincipalController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Principal (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullPrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ExternalPrincipal.Principal domainObject = bll.GetById(principalIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ExternalPrincipal.Principal>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipals()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdentsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(principalIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ExternalPrincipal.Principal>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsInternal(Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ExternalPrincipal.Principal>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichPrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ExternalPrincipal.Principal domainObject = bll.GetById(principalIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ExternalPrincipal.Principal>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalInternal(principalIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ExternalPrincipal.Principal domainObject = bll.GetById(principalIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ExternalPrincipal.Principal>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipals()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdentsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(principalIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ExternalPrincipal.Principal>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsInternal(Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ExternalPrincipal.Principal>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save Principals
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.SavePrincipalInternal(principalStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            return this.SavePrincipalInternal(principalStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(SampleSystem.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IPrincipalBLL bll)
        {
            SampleSystem.Domain.ExternalPrincipal.Principal domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, principalStrict.Id);
            principalStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
