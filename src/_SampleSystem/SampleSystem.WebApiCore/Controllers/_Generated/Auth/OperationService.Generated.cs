namespace Authorization.WebApi.Controllers
{
    using Framework.Authorization.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("authApi/v{version:apiVersion}/[controller]")]
    public partial class OperationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>>
    {
        
        /// <summary>
        /// Check Operation access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckOperationAccess")]
        public virtual void CheckOperationAccess(CheckOperationAccessAutoRequest checkOperationAccessAutoRequest)
        {
            Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode = checkOperationAccessAutoRequest.securityOperationCode;
            Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdent = checkOperationAccessAutoRequest.operationIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckOperationAccessInternal(operationIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckOperationAccessInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdent, Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.Operation;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Authorization.Domain.Operation domainObject = bll.GetById(operationIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.Operation>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Get Operation (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullOperation")]
        public virtual Framework.Authorization.Generated.DTO.OperationFullDTO GetFullOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullOperationInternal(operationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Operation (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullOperationByName")]
        public virtual Framework.Authorization.Generated.DTO.OperationFullDTO GetFullOperationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string operationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullOperationByNameInternal(operationName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationFullDTO GetFullOperationByNameInternal(string operationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, operationName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationFullDTO GetFullOperationInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = bll.GetById(operationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Operations (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullOperations")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationFullDTO> GetFullOperations()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullOperationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Operations (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullOperationsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationFullDTO> GetFullOperationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationIdentityDTO[] operationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullOperationsByIdentsInternal(operationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationFullDTO> GetFullOperationsByIdentsInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO[] operationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(operationIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Operations (FullDTO) by filter (Framework.Authorization.Domain.OperationRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullOperationsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationFullDTO> GetFullOperationsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullOperationsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationFullDTO> GetFullOperationsByRootFilterInternal(Framework.Authorization.Generated.DTO.OperationRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.OperationRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationFullDTO> GetFullOperationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Operation (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichOperation")]
        public virtual Framework.Authorization.Generated.DTO.OperationRichDTO GetRichOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichOperationInternal(operationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Operation (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichOperationByName")]
        public virtual Framework.Authorization.Generated.DTO.OperationRichDTO GetRichOperationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string operationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichOperationByNameInternal(operationName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationRichDTO GetRichOperationByNameInternal(string operationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, operationName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationRichDTO GetRichOperationInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = bll.GetById(operationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Operation (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleOperation")]
        public virtual Framework.Authorization.Generated.DTO.OperationSimpleDTO GetSimpleOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleOperationInternal(operationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Operation (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleOperationByName")]
        public virtual Framework.Authorization.Generated.DTO.OperationSimpleDTO GetSimpleOperationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string operationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleOperationByNameInternal(operationName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationSimpleDTO GetSimpleOperationByNameInternal(string operationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, operationName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationSimpleDTO GetSimpleOperationInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = bll.GetById(operationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Operations (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleOperations")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationSimpleDTO> GetSimpleOperations()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleOperationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Operations (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleOperationsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationSimpleDTO> GetSimpleOperationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationIdentityDTO[] operationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleOperationsByIdentsInternal(operationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationSimpleDTO> GetSimpleOperationsByIdentsInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO[] operationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(operationIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Operations (SimpleDTO) by filter (Framework.Authorization.Domain.OperationRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleOperationsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationSimpleDTO> GetSimpleOperationsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleOperationsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationSimpleDTO> GetSimpleOperationsByRootFilterInternal(Framework.Authorization.Generated.DTO.OperationRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.OperationRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationSimpleDTO> GetSimpleOperationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Operation (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualOperation")]
        public virtual Framework.Authorization.Generated.DTO.OperationVisualDTO GetVisualOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualOperationInternal(operationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Operation (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualOperationByName")]
        public virtual Framework.Authorization.Generated.DTO.OperationVisualDTO GetVisualOperationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string operationName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualOperationByNameInternal(operationName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationVisualDTO GetVisualOperationByNameInternal(string operationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, operationName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationVisualDTO GetVisualOperationInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Operation domainObject = bll.GetById(operationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Operations (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualOperations")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationVisualDTO> GetVisualOperations()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualOperationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Operations (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualOperationsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationVisualDTO> GetVisualOperationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationIdentityDTO[] operationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualOperationsByIdentsInternal(operationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationVisualDTO> GetVisualOperationsByIdentsInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO[] operationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(operationIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Operations (VisualDTO) by filter (Framework.Authorization.Domain.OperationRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualOperationsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationVisualDTO> GetVisualOperationsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualOperationsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationVisualDTO> GetVisualOperationsByRootFilterInternal(Framework.Authorization.Generated.DTO.OperationRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.OperationRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.OperationVisualDTO> GetVisualOperationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Operation>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasOperationAccess")]
        public virtual bool HasOperationAccess(HasOperationAccessAutoRequest hasOperationAccessAutoRequest)
        {
            Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode = hasOperationAccessAutoRequest.securityOperationCode;
            Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdent = hasOperationAccessAutoRequest.operationIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasOperationAccessInternal(operationIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasOperationAccessInternal(Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdent, Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.Operation;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Authorization.Domain.Operation domainObject = bll.GetById(operationIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.Operation>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save Operations
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveOperation")]
        public virtual Framework.Authorization.Generated.DTO.OperationIdentityDTO SaveOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.OperationStrictDTO operationStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveOperationInternal(operationStrict, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationIdentityDTO SaveOperationInternal(Framework.Authorization.Generated.DTO.OperationStrictDTO operationStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IOperationBLL bll = evaluateData.Context.Logics.OperationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveOperationInternal(operationStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.OperationIdentityDTO SaveOperationInternal(Framework.Authorization.Generated.DTO.OperationStrictDTO operationStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IOperationBLL bll)
        {
            Framework.Authorization.Domain.Operation domainObject = bll.GetById(operationStrict.Id, true);
            operationStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckOperationAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasOperationAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.OperationIdentityDTO operationIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode;
    }
}
