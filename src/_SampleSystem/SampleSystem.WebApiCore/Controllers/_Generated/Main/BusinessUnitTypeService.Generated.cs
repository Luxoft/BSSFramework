namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class BusinessUnitTypeController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnitType (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitTypes (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypes()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitTypes (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypesByIdentsInternal(businessUnitTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypesByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(businessUnitTypeIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypesInternal(Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitTypes (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypes()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitTypes (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypesByIdentsInternal(businessUnitTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypesByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(businessUnitTypeIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypesInternal(Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitTypes (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypes()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitTypes (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypesByIdentsInternal(businessUnitTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypesByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(businessUnitTypeIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypesInternal(Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.BusinessUnitType>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save BusinessUnitTypes
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO SaveBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeStrictDTO businessUnitTypeStrict)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.SaveBusinessUnitTypeInternal(businessUnitTypeStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO SaveBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeStrictDTO businessUnitTypeStrict, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            return this.SaveBusinessUnitTypeInternal(businessUnitTypeStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO SaveBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeStrictDTO businessUnitTypeStrict, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IBusinessUnitTypeBLL bll)
        {
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, businessUnitTypeStrict.Id);
            businessUnitTypeStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
