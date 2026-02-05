namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class TestSecuritySubObjItem3Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3Internal(testSecuritySubObjItem3Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem3Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3ByNameInternal(testSecuritySubObjItem3Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3ByNameInternal(string testSecuritySubObjItem3Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem3Name, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetById(testSecuritySubObjItem3Identity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecuritySubObjItem3s (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO> GetFullTestSecuritySubObjItem3s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3s (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO> GetFullTestSecuritySubObjItem3sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO[] testSecuritySubObjItem3Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3sByIdentsInternal(testSecuritySubObjItem3Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO> GetFullTestSecuritySubObjItem3sByIdentsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO[] testSecuritySubObjItem3Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testSecuritySubObjItem3Idents, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO> GetFullTestSecuritySubObjItem3sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem3Internal(testSecuritySubObjItem3Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem3Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem3ByNameInternal(testSecuritySubObjItem3Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3ByNameInternal(string testSecuritySubObjItem3Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem3Name, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetById(testSecuritySubObjItem3Identity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3Internal(testSecuritySubObjItem3Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem3Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3ByNameInternal(testSecuritySubObjItem3Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3ByNameInternal(string testSecuritySubObjItem3Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem3Name, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetById(testSecuritySubObjItem3Identity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecuritySubObjItem3s (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO> GetSimpleTestSecuritySubObjItem3s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3s (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO> GetSimpleTestSecuritySubObjItem3sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO[] testSecuritySubObjItem3Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3sByIdentsInternal(testSecuritySubObjItem3Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO> GetSimpleTestSecuritySubObjItem3sByIdentsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO[] testSecuritySubObjItem3Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testSecuritySubObjItem3Idents, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO> GetSimpleTestSecuritySubObjItem3sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3Internal(testSecuritySubObjItem3Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem3Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3ByNameInternal(testSecuritySubObjItem3Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3ByNameInternal(string testSecuritySubObjItem3Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem3Name, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetById(testSecuritySubObjItem3Identity.Id, true, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecuritySubObjItem3s (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO> GetVisualTestSecuritySubObjItem3s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3s (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO> GetVisualTestSecuritySubObjItem3sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO[] testSecuritySubObjItem3Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3sByIdentsInternal(testSecuritySubObjItem3Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO> GetVisualTestSecuritySubObjItem3sByIdentsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO[] testSecuritySubObjItem3Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testSecuritySubObjItem3Idents, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO> GetVisualTestSecuritySubObjItem3sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.TestSecuritySubObjItem3>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
