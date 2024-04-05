namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestCustomContextSecurityObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestCustomContextSecurityObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjsByIdentsInternal(testCustomContextSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testCustomContextSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestCustomContextSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestCustomContextSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestCustomContextSecurityObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjsByIdentsInternal(testCustomContextSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testCustomContextSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestCustomContextSecurityObjs (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjs (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjsByIdentsInternal(testCustomContextSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testCustomContextSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjProjection (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjProjection")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjProjectionDTO GetTestCustomContextSecurityObjProjection([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjProjectionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjProjectionInternal(testCustomContextSecurityObjProjectionIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjProjectionDTO GetTestCustomContextSecurityObjProjectionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjProjectionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjProjectionBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjProjectionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Projections.TestCustomContextSecurityObjProjection domainObject = bll.GetById(testCustomContextSecurityObjProjectionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestCustomContextSecurityObjProjection>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
    }
}
