namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class IMRequestController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check IMRequest access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckIMRequestAccess")]
        public virtual void CheckIMRequestAccess(CheckIMRequestAccessAutoRequest checkIMRequestAccessAutoRequest)
        {
            string securityOperationName = checkIMRequestAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdent = checkIMRequestAccessAutoRequest.iMRequestIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckIMRequestAccessInternal(iMRequestIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckIMRequestAccessInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequest;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(SampleSystem.SampleSystemSecurityOperation), securityOperationName);
            SampleSystem.Domain.IMRequest domainObject = bll.GetById(iMRequestIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.IMRequest>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get IMRequest (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullIMRequest")]
        public virtual SampleSystem.Generated.DTO.IMRequestFullDTO GetFullIMRequest([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullIMRequestInternal(iMRequestIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get IMRequest (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullIMRequestByName")]
        public virtual SampleSystem.Generated.DTO.IMRequestFullDTO GetFullIMRequestByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string iMRequestName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullIMRequestByNameInternal(iMRequestName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestFullDTO GetFullIMRequestByNameInternal(string iMRequestName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, iMRequestName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestFullDTO GetFullIMRequestInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetById(iMRequestIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of IMRequests (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullIMRequests")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestFullDTO> GetFullIMRequests()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullIMRequestsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get IMRequests (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullIMRequestsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestFullDTO> GetFullIMRequestsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO[] iMRequestIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullIMRequestsByIdentsInternal(iMRequestIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestFullDTO> GetFullIMRequestsByIdentsInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO[] iMRequestIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(iMRequestIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestFullDTO> GetFullIMRequestsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get IMRequest (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichIMRequest")]
        public virtual SampleSystem.Generated.DTO.IMRequestRichDTO GetRichIMRequest([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichIMRequestInternal(iMRequestIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get IMRequest (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichIMRequestByName")]
        public virtual SampleSystem.Generated.DTO.IMRequestRichDTO GetRichIMRequestByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string iMRequestName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichIMRequestByNameInternal(iMRequestName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestRichDTO GetRichIMRequestByNameInternal(string iMRequestName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, iMRequestName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestRichDTO GetRichIMRequestInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetById(iMRequestIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get IMRequest (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleIMRequest")]
        public virtual SampleSystem.Generated.DTO.IMRequestSimpleDTO GetSimpleIMRequest([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleIMRequestInternal(iMRequestIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get IMRequest (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleIMRequestByName")]
        public virtual SampleSystem.Generated.DTO.IMRequestSimpleDTO GetSimpleIMRequestByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string iMRequestName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleIMRequestByNameInternal(iMRequestName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestSimpleDTO GetSimpleIMRequestByNameInternal(string iMRequestName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, iMRequestName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestSimpleDTO GetSimpleIMRequestInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetById(iMRequestIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of IMRequests (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleIMRequests")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestSimpleDTO> GetSimpleIMRequests()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleIMRequestsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get IMRequests (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleIMRequestsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestSimpleDTO> GetSimpleIMRequestsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO[] iMRequestIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleIMRequestsByIdentsInternal(iMRequestIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestSimpleDTO> GetSimpleIMRequestsByIdentsInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO[] iMRequestIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(iMRequestIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestSimpleDTO> GetSimpleIMRequestsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get IMRequest (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualIMRequest")]
        public virtual SampleSystem.Generated.DTO.IMRequestVisualDTO GetVisualIMRequest([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualIMRequestInternal(iMRequestIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get IMRequest (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualIMRequestByName")]
        public virtual SampleSystem.Generated.DTO.IMRequestVisualDTO GetVisualIMRequestByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string iMRequestName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualIMRequestByNameInternal(iMRequestName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestVisualDTO GetVisualIMRequestByNameInternal(string iMRequestName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, iMRequestName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestVisualDTO GetVisualIMRequestInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetById(iMRequestIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of IMRequests (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualIMRequests")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestVisualDTO> GetVisualIMRequests()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualIMRequestsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get IMRequests (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualIMRequestsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestVisualDTO> GetVisualIMRequestsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO[] iMRequestIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualIMRequestsByIdentsInternal(iMRequestIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestVisualDTO> GetVisualIMRequestsByIdentsInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO[] iMRequestIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(iMRequestIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IMRequestVisualDTO> GetVisualIMRequestsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.IMRequest>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for IMRequest
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasIMRequestAccess")]
        public virtual bool HasIMRequestAccess(HasIMRequestAccessAutoRequest hasIMRequestAccessAutoRequest)
        {
            string securityOperationName = hasIMRequestAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdent = hasIMRequestAccessAutoRequest.iMRequestIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasIMRequestAccessInternal(iMRequestIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasIMRequestAccessInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequest;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(SampleSystem.SampleSystemSecurityOperation), securityOperationName);
            SampleSystem.Domain.IMRequest domainObject = bll.GetById(iMRequestIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.IMRequest>(operation).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Get TestIMRequest (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestIMRequest")]
        public virtual SampleSystem.Generated.DTO.TestIMRequestProjectionDTO GetTestIMRequest([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO testIMRequestIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestIMRequestInternal(testIMRequestIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestIMRequestProjectionDTO GetTestIMRequestInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO testIMRequestIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestIMRequestBLL bll = evaluateData.Context.Logics.TestIMRequestFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Projections.TestIMRequest domainObject = bll.GetById(testIMRequestIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestIMRequest>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckIMRequestAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasIMRequestAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
