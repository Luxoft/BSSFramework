namespace SampleSystem.WebApiCore.Controllers.Integration
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class IntegrationVersionContainer1Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Remove IntegrationVersionContainer1
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveIntegrationVersionContainer1")]
        public virtual void RemoveIntegrationVersionContainer1([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO integrationVersionContainer1Ident)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveIntegrationVersionContainer1Internal(integrationVersionContainer1Ident, evaluateData));
        }
        
        protected virtual void RemoveIntegrationVersionContainer1Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO integrationVersionContainer1Ident, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIntegrationVersionContainer1BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer1;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, SampleSystem.BLL.SampleSystemSecurityOperation.SystemIntegration);
            SampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer1 domainObject = bll.GetById(integrationVersionContainer1Ident.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save IntegrationVersionContainer1
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveIntegrationVersionContainer1")]
        public virtual SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO SaveIntegrationVersionContainer1([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IntegrationVersionContainer1IntegrationRichDTO integrationVersionContainer1)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveIntegrationVersionContainer1Internal(integrationVersionContainer1, evaluateData));
        }
        
        /// <summary>
        /// Save IntegrationVersionContainer1 by model IntegrationVersionContainer1CustomIntegrationSaveModel
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveIntegrationVersionContainer1ByCustom")]
        public virtual SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO SaveIntegrationVersionContainer1ByCustom([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IntegrationVersionContainer1CustomIntegrationSaveModelIntegrationRichDTO integrationVersionContainer1IntegrationSaveModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveIntegrationVersionContainer1ByCustomInternal(integrationVersionContainer1IntegrationSaveModel, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO SaveIntegrationVersionContainer1ByCustomInternal(SampleSystem.Generated.DTO.IntegrationVersionContainer1CustomIntegrationSaveModelIntegrationRichDTO integrationVersionContainer1IntegrationSaveModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIntegrationVersionContainer1BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer1;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, SampleSystem.BLL.SampleSystemSecurityOperation.SystemIntegration);
            SampleSystem.Domain.IntegrationVersionContainer1CustomIntegrationSaveModel integrationSaveModel = integrationVersionContainer1IntegrationSaveModel.ToDomainObject(evaluateData.MappingService);
            SampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer1 domainObject = integrationSaveModel.SavingObject;
            if ((domainObject.IntegrationVersion < integrationVersionContainer1IntegrationSaveModel.SavingObject.IntegrationVersion))
            {
                domainObject.IntegrationVersion = integrationVersionContainer1IntegrationSaveModel.SavingObject.IntegrationVersion;
                bll.IntegrationSave(integrationSaveModel);
            }
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO SaveIntegrationVersionContainer1Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer1IntegrationRichDTO integrationVersionContainer1, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIntegrationVersionContainer1BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer1;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, SampleSystem.BLL.SampleSystemSecurityOperation.SystemIntegration);
            return this.SaveIntegrationVersionContainer1Internal(integrationVersionContainer1, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO SaveIntegrationVersionContainer1Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer1IntegrationRichDTO integrationVersionContainer1, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IIntegrationVersionContainer1BLL bll)
        {
            SampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer1 domainObject = bll.GetById(integrationVersionContainer1.Id, false, null, Framework.DomainDriven.LockRole.Update);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new SampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer1();
            }
            if ((domainObject.IntegrationVersion < integrationVersionContainer1.IntegrationVersion))
            {
                integrationVersionContainer1.MapToDomainObject(evaluateData.MappingService, domainObject);
                bll.Insert(domainObject, integrationVersionContainer1.Id);
            }
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Save IntegrationVersionContainer1s
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveIntegrationVersionContainer1s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO> SaveIntegrationVersionContainer1s([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IntegrationVersionContainer1IntegrationRichDTO[] integrationVersionContainer1s)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveIntegrationVersionContainer1sInternal(integrationVersionContainer1s, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.IntegrationVersionContainer1IdentityDTO> SaveIntegrationVersionContainer1sInternal(SampleSystem.Generated.DTO.IntegrationVersionContainer1IntegrationRichDTO[] integrationVersionContainer1s, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIntegrationVersionContainer1BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer1;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, SampleSystem.BLL.SampleSystemSecurityOperation.SystemIntegration);
            return Framework.Core.EnumerableExtensions.ToList(integrationVersionContainer1s, integrationVersionContainer1 => this.SaveIntegrationVersionContainer1Internal(integrationVersionContainer1, evaluateData, bll));
        }
    }
}
