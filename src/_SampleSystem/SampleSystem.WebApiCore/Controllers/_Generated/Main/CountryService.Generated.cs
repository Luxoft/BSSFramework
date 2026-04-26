namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class CountryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get full list of Countries (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountries()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullCountriesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Countries (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullCountriesByIdentsInternal(countryIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByIdentsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(countryIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByCodeInternal(string countryCode, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryByNameInternal(string countryName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = bll.GetById(countryIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByCodeInternal(string countryCode, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryByNameInternal(string countryName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = bll.GetById(countryIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Countries (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountries()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleCountriesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Countries (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleCountriesByIdentsInternal(countryIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByIdentsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(countryIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByCodeInternal(string countryCode, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryByNameInternal(string countryName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = bll.GetById(countryIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Countries (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountries()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualCountriesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Countries (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualCountriesByIdentsInternal(countryIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByIdentsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO[] countryIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(countryIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualCountryInternal(countryIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Country (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryCode)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualCountryByCodeInternal(countryCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByCodeInternal(string countryCode, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, countryCode, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string countryName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualCountryByNameInternal(countryName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryByNameInternal(string countryName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, countryName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.Country domainObject = bll.GetById(countryIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove Country
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemoveCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent)
        {
            this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.RemoveCountryInternal(countryIdent, evaluateData));
        }
        
        protected virtual void RemoveCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            this.RemoveCountryInternal(countryIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveCountryInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdent, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ICountryBLL bll)
        {
            SampleSystem.Domain.Directories.Country domainObject = bll.GetById(countryIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Countries
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO SaveCountry([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryStrictDTO countryStrict)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.SaveCountryInternal(countryStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryIdentityDTO SaveCountryInternal(SampleSystem.Generated.DTO.CountryStrictDTO countryStrict, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            return this.SaveCountryInternal(countryStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryIdentityDTO SaveCountryInternal(SampleSystem.Generated.DTO.CountryStrictDTO countryStrict, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ICountryBLL bll)
        {
            SampleSystem.Domain.Directories.Country domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, countryStrict.Id);
            countryStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
