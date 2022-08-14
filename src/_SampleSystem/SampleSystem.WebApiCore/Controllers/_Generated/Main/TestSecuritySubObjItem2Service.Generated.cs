namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestSecuritySubObjItem2Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem2")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO GetFullTestSecuritySubObjItem2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem2Internal(testSecuritySubObjItem2Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem2ByName")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO GetFullTestSecuritySubObjItem2ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem2Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem2ByNameInternal(testSecuritySubObjItem2Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO GetFullTestSecuritySubObjItem2ByNameInternal(string testSecuritySubObjItem2Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem2Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO GetFullTestSecuritySubObjItem2Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetById(testSecuritySubObjItem2Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecuritySubObjItem2s (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem2s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem2sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem2sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO[] testSecuritySubObjItem2Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem2sByIdentsInternal(testSecuritySubObjItem2Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2sByIdentsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO[] testSecuritySubObjItem2Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testSecuritySubObjItem2Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO> GetFullTestSecuritySubObjItem2sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecuritySubObjItem2")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2RichDTO GetRichTestSecuritySubObjItem2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem2Internal(testSecuritySubObjItem2Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecuritySubObjItem2ByName")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2RichDTO GetRichTestSecuritySubObjItem2ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem2Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem2ByNameInternal(testSecuritySubObjItem2Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2RichDTO GetRichTestSecuritySubObjItem2ByNameInternal(string testSecuritySubObjItem2Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem2Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2RichDTO GetRichTestSecuritySubObjItem2Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetById(testSecuritySubObjItem2Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem2")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO GetSimpleTestSecuritySubObjItem2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem2Internal(testSecuritySubObjItem2Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem2ByName")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO GetSimpleTestSecuritySubObjItem2ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem2Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem2ByNameInternal(testSecuritySubObjItem2Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO GetSimpleTestSecuritySubObjItem2ByNameInternal(string testSecuritySubObjItem2Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem2Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO GetSimpleTestSecuritySubObjItem2Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetById(testSecuritySubObjItem2Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecuritySubObjItem2s (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem2s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem2sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem2sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO[] testSecuritySubObjItem2Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem2sByIdentsInternal(testSecuritySubObjItem2Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2sByIdentsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO[] testSecuritySubObjItem2Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testSecuritySubObjItem2Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO> GetSimpleTestSecuritySubObjItem2sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem2")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO GetVisualTestSecuritySubObjItem2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem2Internal(testSecuritySubObjItem2Identity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem2ByName")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO GetVisualTestSecuritySubObjItem2ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecuritySubObjItem2Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem2ByNameInternal(testSecuritySubObjItem2Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO GetVisualTestSecuritySubObjItem2ByNameInternal(string testSecuritySubObjItem2Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecuritySubObjItem2Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO GetVisualTestSecuritySubObjItem2Internal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetById(testSecuritySubObjItem2Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecuritySubObjItem2s (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem2s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem2sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2s (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem2sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO[] testSecuritySubObjItem2Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem2sByIdentsInternal(testSecuritySubObjItem2Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2sByIdentsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO[] testSecuritySubObjItem2Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testSecuritySubObjItem2Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO> GetVisualTestSecuritySubObjItem2sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecuritySubObjItem2>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
