namespace Authorization.WebApi.Controllers
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("authApi/[controller]/[action]")]
    public partial class SecurityContextTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>
    {
        
        /// <summary>
        /// Get SecurityContextType (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO GetFullSecurityContextType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSecurityContextTypeInternal(securityContextTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SecurityContextType (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO GetFullSecurityContextTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string securityContextTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSecurityContextTypeByNameInternal(securityContextTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO GetFullSecurityContextTypeByNameInternal(string securityContextTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, securityContextTypeName, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO GetFullSecurityContextTypeInternal(Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = bll.GetById(securityContextTypeIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SecurityContextTypes (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO> GetFullSecurityContextTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSecurityContextTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SecurityContextTypes (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO> GetFullSecurityContextTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO[] securityContextTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSecurityContextTypesByIdentsInternal(securityContextTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO> GetFullSecurityContextTypesByIdentsInternal(Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO[] securityContextTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(securityContextTypeIdents, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeFullDTO> GetFullSecurityContextTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SecurityContextType (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeRichDTO GetRichSecurityContextType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSecurityContextTypeInternal(securityContextTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SecurityContextType (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeRichDTO GetRichSecurityContextTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string securityContextTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSecurityContextTypeByNameInternal(securityContextTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeRichDTO GetRichSecurityContextTypeByNameInternal(string securityContextTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, securityContextTypeName, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeRichDTO GetRichSecurityContextTypeInternal(Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = bll.GetById(securityContextTypeIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SecurityContextType (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO GetSimpleSecurityContextType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSecurityContextTypeInternal(securityContextTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SecurityContextType (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO GetSimpleSecurityContextTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string securityContextTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSecurityContextTypeByNameInternal(securityContextTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO GetSimpleSecurityContextTypeByNameInternal(string securityContextTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, securityContextTypeName, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO GetSimpleSecurityContextTypeInternal(Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = bll.GetById(securityContextTypeIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SecurityContextTypes (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO> GetSimpleSecurityContextTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSecurityContextTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SecurityContextTypes (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO> GetSimpleSecurityContextTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO[] securityContextTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSecurityContextTypesByIdentsInternal(securityContextTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO> GetSimpleSecurityContextTypesByIdentsInternal(Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO[] securityContextTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(securityContextTypeIdents, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeSimpleDTO> GetSimpleSecurityContextTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SecurityContextType (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO GetVisualSecurityContextType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSecurityContextTypeInternal(securityContextTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SecurityContextType (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO GetVisualSecurityContextTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string securityContextTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSecurityContextTypeByNameInternal(securityContextTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO GetVisualSecurityContextTypeByNameInternal(string securityContextTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, securityContextTypeName, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO GetVisualSecurityContextTypeInternal(Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO securityContextTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.Authorization.Domain.SecurityContextType domainObject = bll.GetById(securityContextTypeIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SecurityContextTypes (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO> GetVisualSecurityContextTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSecurityContextTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SecurityContextTypes (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO> GetVisualSecurityContextTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO[] securityContextTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSecurityContextTypesByIdentsInternal(securityContextTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO> GetVisualSecurityContextTypesByIdentsInternal(Framework.Authorization.Generated.DTO.SecurityContextTypeIdentityDTO[] securityContextTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(securityContextTypeIdents, new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.SecurityContextTypeVisualDTO> GetVisualSecurityContextTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.ISecurityContextTypeBLL bll = evaluateData.Context.Logics.SecurityContextTypeFactory.Create(SecuritySystem.SecurityRule.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<Framework.Authorization.Domain.SecurityContextType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
