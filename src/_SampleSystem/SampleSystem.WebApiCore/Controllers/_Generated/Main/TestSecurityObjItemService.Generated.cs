namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestSecurityObjItemController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecurityObjItem")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestSecurityObjItemInternal(testSecurityObjItemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecurityObjItemByName")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecurityObjItemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestSecurityObjItemByNameInternal(testSecurityObjItemName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItemByNameInternal(string testSecurityObjItemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecurityObjItemName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItemInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetById(testSecurityObjItemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecurityObjItems (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecurityObjItems")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO> GetFullTestSecurityObjItems()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestSecurityObjItemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecurityObjItems (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecurityObjItemsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO> GetFullTestSecurityObjItemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO[] testSecurityObjItemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestSecurityObjItemsByIdentsInternal(testSecurityObjItemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO> GetFullTestSecurityObjItemsByIdentsInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO[] testSecurityObjItemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testSecurityObjItemIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO> GetFullTestSecurityObjItemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecurityObjItem")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestSecurityObjItemInternal(testSecurityObjItemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecurityObjItemByName")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecurityObjItemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestSecurityObjItemByNameInternal(testSecurityObjItemName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItemByNameInternal(string testSecurityObjItemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecurityObjItemName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItemInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetById(testSecurityObjItemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecurityObjItem")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecurityObjItemInternal(testSecurityObjItemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecurityObjItemByName")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecurityObjItemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecurityObjItemByNameInternal(testSecurityObjItemName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItemByNameInternal(string testSecurityObjItemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecurityObjItemName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItemInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetById(testSecurityObjItemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecurityObjItems (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecurityObjItems")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO> GetSimpleTestSecurityObjItems()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecurityObjItemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecurityObjItems (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecurityObjItemsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO> GetSimpleTestSecurityObjItemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO[] testSecurityObjItemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecurityObjItemsByIdentsInternal(testSecurityObjItemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO> GetSimpleTestSecurityObjItemsByIdentsInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO[] testSecurityObjItemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testSecurityObjItemIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO> GetSimpleTestSecurityObjItemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecurityObjItem")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestSecurityObjItemInternal(testSecurityObjItemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecurityObjItemByName")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testSecurityObjItemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestSecurityObjItemByNameInternal(testSecurityObjItemName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItemByNameInternal(string testSecurityObjItemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testSecurityObjItemName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItemInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetById(testSecurityObjItemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecurityObjItems (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecurityObjItems")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO> GetVisualTestSecurityObjItems()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestSecurityObjItemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestSecurityObjItems (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecurityObjItemsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO> GetVisualTestSecurityObjItemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO[] testSecurityObjItemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestSecurityObjItemsByIdentsInternal(testSecurityObjItemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO> GetVisualTestSecurityObjItemsByIdentsInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO[] testSecurityObjItemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testSecurityObjItemIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO> GetVisualTestSecurityObjItemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestSecurityObjItem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItemProjection (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecurityObjItemProjection")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemProjectionDTO GetTestSecurityObjItemProjection([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemProjectionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemProjectionInternal(testSecurityObjItemProjectionIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemProjectionDTO GetTestSecurityObjItemProjectionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemProjectionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemProjectionBLL bll = evaluateData.Context.Logics.TestSecurityObjItemProjectionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Projections.TestSecurityObjItemProjection domainObject = bll.GetById(testSecurityObjItemProjectionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestSecurityObjItemProjection>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestSecurityObjItemProjections (ProjectionDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecurityObjItemProjections")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemProjectionDTO> GetTestSecurityObjItemProjections()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemProjectionsInternal(evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestSecurityObjItemProjectionDTO> GetTestSecurityObjItemProjectionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemProjectionBLL bll = evaluateData.Context.Logics.TestSecurityObjItemProjectionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestSecurityObjItemProjection>(Framework.Transfering.ViewDTOType.ProjectionDTO)), evaluateData.MappingService);
        }
    }
}
