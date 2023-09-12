namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class Location1676Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check Location1676 access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckLocation1676Access")]
        public virtual void CheckLocation1676Access(CheckLocation1676AccessAutoRequest checkLocation1676AccessAutoRequest)
        {
            string securityOperationName = checkLocation1676AccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Ident = checkLocation1676AccessAutoRequest.location1676Ident;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckLocation1676AccessInternal(location1676Ident, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckLocation1676AccessInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Ident, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(SampleSystem.SampleSystemSecurityOperation), securityOperationName);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetById(location1676Ident.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EnversBug1676.Location1676>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get Location1676 (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocation1676")]
        public virtual SampleSystem.Generated.DTO.Location1676FullDTO GetFullLocation1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocation1676Internal(location1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get Location1676 (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocation1676ByName")]
        public virtual SampleSystem.Generated.DTO.Location1676FullDTO GetFullLocation1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string location1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocation1676ByNameInternal(location1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676FullDTO GetFullLocation1676ByNameInternal(string location1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, location1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676FullDTO GetFullLocation1676Internal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetById(location1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Location1676s (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocation1676s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676FullDTO> GetFullLocation1676s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocation1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Location1676s (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocation1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676FullDTO> GetFullLocation1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO[] location1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocation1676sByIdentsInternal(location1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676FullDTO> GetFullLocation1676sByIdentsInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO[] location1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(location1676Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676FullDTO> GetFullLocation1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location1676 (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichLocation1676")]
        public virtual SampleSystem.Generated.DTO.Location1676RichDTO GetRichLocation1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichLocation1676Internal(location1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get Location1676 (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichLocation1676ByName")]
        public virtual SampleSystem.Generated.DTO.Location1676RichDTO GetRichLocation1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string location1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichLocation1676ByNameInternal(location1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676RichDTO GetRichLocation1676ByNameInternal(string location1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, location1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676RichDTO GetRichLocation1676Internal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetById(location1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location1676 (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocation1676")]
        public virtual SampleSystem.Generated.DTO.Location1676SimpleDTO GetSimpleLocation1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocation1676Internal(location1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get Location1676 (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocation1676ByName")]
        public virtual SampleSystem.Generated.DTO.Location1676SimpleDTO GetSimpleLocation1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string location1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocation1676ByNameInternal(location1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676SimpleDTO GetSimpleLocation1676ByNameInternal(string location1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, location1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676SimpleDTO GetSimpleLocation1676Internal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetById(location1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Location1676s (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocation1676s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676SimpleDTO> GetSimpleLocation1676s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocation1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Location1676s (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocation1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676SimpleDTO> GetSimpleLocation1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO[] location1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocation1676sByIdentsInternal(location1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676SimpleDTO> GetSimpleLocation1676sByIdentsInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO[] location1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(location1676Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676SimpleDTO> GetSimpleLocation1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location1676 (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocation1676")]
        public virtual SampleSystem.Generated.DTO.Location1676VisualDTO GetVisualLocation1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocation1676Internal(location1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get Location1676 (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocation1676ByName")]
        public virtual SampleSystem.Generated.DTO.Location1676VisualDTO GetVisualLocation1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string location1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocation1676ByNameInternal(location1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676VisualDTO GetVisualLocation1676ByNameInternal(string location1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, location1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676VisualDTO GetVisualLocation1676Internal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetById(location1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Location1676s (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocation1676s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676VisualDTO> GetVisualLocation1676s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocation1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Location1676s (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocation1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676VisualDTO> GetVisualLocation1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO[] location1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocation1676sByIdentsInternal(location1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676VisualDTO> GetVisualLocation1676sByIdentsInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO[] location1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(location1676Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Location1676VisualDTO> GetVisualLocation1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.Location1676>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Location1676
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasLocation1676Access")]
        public virtual bool HasLocation1676Access(HasLocation1676AccessAutoRequest hasLocation1676AccessAutoRequest)
        {
            string securityOperationName = hasLocation1676AccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Ident = hasLocation1676AccessAutoRequest.location1676Ident;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasLocation1676AccessInternal(location1676Ident, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasLocation1676AccessInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Ident, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(SampleSystem.SampleSystemSecurityOperation), securityOperationName);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetById(location1676Ident.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.EnversBug1676.Location1676>(operation).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckLocation1676AccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Ident;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasLocation1676AccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Ident;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
