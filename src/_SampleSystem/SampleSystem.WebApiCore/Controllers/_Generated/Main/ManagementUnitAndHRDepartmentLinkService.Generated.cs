namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class ManagementUnitAndHRDepartmentLinkController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitAndHRDepartmentLinkName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinkByNameInternal(managementUnitAndHRDepartmentLinkName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkByNameInternal(string managementUnitAndHRDepartmentLinkName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitAndHRDepartmentLinkName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnitAndHRDepartmentLinks (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinks()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinksInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinksByIdentsInternal(managementUnitAndHRDepartmentLinkIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(managementUnitAndHRDepartmentLinkIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitAndHRDepartmentLinkName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndHRDepartmentLinkByNameInternal(managementUnitAndHRDepartmentLinkName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkByNameInternal(string managementUnitAndHRDepartmentLinkName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitAndHRDepartmentLinkName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitAndHRDepartmentLinkName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinkByNameInternal(managementUnitAndHRDepartmentLinkName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkByNameInternal(string managementUnitAndHRDepartmentLinkName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitAndHRDepartmentLinkName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnitAndHRDepartmentLinks (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinks()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinksInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinksByIdentsInternal(managementUnitAndHRDepartmentLinkIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(managementUnitAndHRDepartmentLinkIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove ManagementUnitAndHRDepartmentLink
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemoveManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent)
        {
            this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.RemoveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdent, evaluateData));
        }
        
        protected virtual void RemoveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            this.RemoveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll)
        {
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save ManagementUnitAndHRDepartmentLinks
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO SaveManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkStrictDTO managementUnitAndHRDepartmentLinkStrict)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.SaveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO SaveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkStrictDTO managementUnitAndHRDepartmentLinkStrict, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            return this.SaveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO SaveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkStrictDTO managementUnitAndHRDepartmentLinkStrict, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll)
        {
            SampleSystem.Domain.MU.ManagementUnitAndHRDepartmentLink domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, managementUnitAndHRDepartmentLinkStrict.Id);
            managementUnitAndHRDepartmentLinkStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
