namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class LocationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Location (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationFullDTO GetFullLocation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocationInternal(locationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Location (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationFullDTO GetFullLocationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string locationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocationByNameInternal(locationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationFullDTO GetFullLocationByNameInternal(string locationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, locationName, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationFullDTO GetFullLocationInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetById(locationIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Locations (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationFullDTO> GetFullLocations()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Locations (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationFullDTO> GetFullLocationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO[] locationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocationsByIdentsInternal(locationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationFullDTO> GetFullLocationsByIdentsInternal(SampleSystem.Generated.DTO.LocationIdentityDTO[] locationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(locationIdents, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationFullDTO> GetFullLocationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationRichDTO GetRichLocation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichLocationInternal(locationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Location (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationRichDTO GetRichLocationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string locationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichLocationByNameInternal(locationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationRichDTO GetRichLocationByNameInternal(string locationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, locationName, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationRichDTO GetRichLocationInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetById(locationIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationSimpleDTO GetSimpleLocation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocationInternal(locationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Location (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationSimpleDTO GetSimpleLocationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string locationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocationByNameInternal(locationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationSimpleDTO GetSimpleLocationByNameInternal(string locationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, locationName, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationSimpleDTO GetSimpleLocationInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetById(locationIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Locations (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocations()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Locations (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO[] locationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocationsByIdentsInternal(locationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocationsByIdentsInternal(SampleSystem.Generated.DTO.LocationIdentityDTO[] locationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(locationIdents, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationVisualDTO GetVisualLocation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocationInternal(locationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Location (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationVisualDTO GetVisualLocationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string locationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocationByNameInternal(locationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationVisualDTO GetVisualLocationByNameInternal(string locationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, locationName, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationVisualDTO GetVisualLocationInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetById(locationIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Locations (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocations()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Locations (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO[] locationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocationsByIdentsInternal(locationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocationsByIdentsInternal(SampleSystem.Generated.DTO.LocationIdentityDTO[] locationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(locationIdents, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove Location
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemoveLocation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO locationIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveLocationInternal(locationIdent, evaluateData));
        }
        
        protected virtual void RemoveLocationInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.Edit);
            this.RemoveLocationInternal(locationIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveLocationInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ILocationBLL bll)
        {
            SampleSystem.Domain.Location domainObject = bll.GetById(locationIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Locations
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.LocationIdentityDTO SaveLocation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationStrictDTO locationStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveLocationInternal(locationStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationIdentityDTO SaveLocationInternal(SampleSystem.Generated.DTO.LocationStrictDTO locationStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(SecuritySystem.SecurityRule.Edit);
            return this.SaveLocationInternal(locationStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationIdentityDTO SaveLocationInternal(SampleSystem.Generated.DTO.LocationStrictDTO locationStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ILocationBLL bll)
        {
            SampleSystem.Domain.Location domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, locationStrict.Id);
            locationStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Get TestLocation (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestLocationProjectionDTO GetTestLocation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO testLocationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestLocationInternal(testLocationIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestLocationProjectionDTO GetTestLocationInternal(SampleSystem.Generated.DTO.LocationIdentityDTO testLocationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationBLL bll = evaluateData.Context.Logics.TestLocationFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Projections.TestLocation domainObject = bll.GetById(testLocationIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Projections.TestLocation>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestLocationCollectionProperties (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO GetTestLocationCollectionProperties([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO testLocationCollectionPropertiesIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestLocationCollectionPropertiesInternal(testLocationCollectionPropertiesIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO GetTestLocationCollectionPropertiesInternal(SampleSystem.Generated.DTO.LocationIdentityDTO testLocationCollectionPropertiesIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationCollectionPropertiesBLL bll = evaluateData.Context.Logics.TestLocationCollectionPropertiesFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Projections.TestLocationCollectionProperties domainObject = bll.GetById(testLocationCollectionPropertiesIdentity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Projections.TestLocationCollectionProperties>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
    }
}
