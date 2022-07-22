namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class CountryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check Country access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckCountryAccess")]
        public virtual void CheckCountryAccess(CheckCountryAccessAutoRequest checkCountryAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkCountryAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent = checkCountryAccessAutoRequest.countryIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckCountryAccessInternal(countryIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckCountryAccessInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.Country;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.Country domainObject = bll.GetById(countryIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.Country>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get full list of Countries (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCountries")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountries()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCountriesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Countries (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCountriesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCountriesByIdentsInternal(countryIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByIdentsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(countryIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCountry")]
        public virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCountryByCode")]
        public virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByCodeInternal(string countryCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCountryByName")]
        public virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByNameInternal(string countryName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = bll.GetById(countryIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCountry")]
        public virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCountryByCode")]
        public virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByCodeInternal(string countryCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCountryByName")]
        public virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByNameInternal(string countryName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = bll.GetById(countryIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Countries (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCountries")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountries()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCountriesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Countries (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCountriesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCountriesByIdentsInternal(countryIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByIdentsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(countryIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCountry")]
        public virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCountryByCode")]
        public virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByCodeInternal(string countryCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCountryByName")]
        public virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByNameInternal(string countryName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = bll.GetById(countryIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Countries (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCountries")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountries()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCountriesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Countries (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCountriesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCountriesByIdentsInternal(countryIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByIdentsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(countryIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCountry")]
        public virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCountryByCode")]
        public virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByCodeInternal(string countryCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCountryByName")]
        public virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByNameInternal(string countryName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Country domainObject = bll.GetById(countryIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Country
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasCountryAccess")]
        public virtual bool HasCountryAccess(HasCountryAccessAutoRequest hasCountryAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasCountryAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent = hasCountryAccessAutoRequest.countryIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasCountryAccessInternal(countryIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasCountryAccessInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.Country;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.Country domainObject = bll.GetById(countryIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.Country>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove Country
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveCountry")]
        public virtual void RemoveCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveCountryInternal(countryIdent, evaluateData));
        }
        
        protected virtual void RemoveCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveCountryInternal(countryIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ICountryBLL bll)
        {
            SampleSystem.Domain.Country domainObject = bll.GetById(countryIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Countries
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveCountry")]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO SaveCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryStrictDTO countryStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveCountryInternal(countryStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryIdentityDTO SaveCountryInternal(SampleSystem.Generated.DTO.CountryStrictDTO countryStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveCountryInternal(countryStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryIdentityDTO SaveCountryInternal(SampleSystem.Generated.DTO.CountryStrictDTO countryStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ICountryBLL bll)
        {
            SampleSystem.Domain.Country domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, countryStrict.Id);
            countryStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckCountryAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasCountryAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
