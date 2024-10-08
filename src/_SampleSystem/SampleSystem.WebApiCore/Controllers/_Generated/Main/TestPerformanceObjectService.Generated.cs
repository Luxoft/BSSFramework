﻿namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class TestPerformanceObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestPerformanceObject (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPerformanceObjects (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObjects (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectsByIdentsInternal(testPerformanceObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testPerformanceObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPerformanceObjects (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObjects (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectsByIdentsInternal(testPerformanceObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testPerformanceObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPerformanceObjects (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObjects (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectsByIdentsInternal(testPerformanceObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testPerformanceObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
