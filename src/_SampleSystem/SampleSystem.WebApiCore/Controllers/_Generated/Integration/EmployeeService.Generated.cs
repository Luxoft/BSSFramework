namespace SampleSystem.WebApiCore.Controllers.Integration
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Save Employee
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveEmployee")]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveEmployeeInternal(employee, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.Employee;
            evaluateData.Context.Authorization.AuthorizationSystem.CheckAccess(Framework.Core.BssSecurityOperation.SystemIntegration);
            return this.SaveEmployeeInternal(employee, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IEmployeeBLL bll)
        {
            SampleSystem.Domain.Employee domainObject = bll.GetById(employee.Id, false);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new SampleSystem.Domain.Employee();
            }
            employee.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Insert(domainObject, employee.Id);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
