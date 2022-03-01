using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.WebApi;
using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.TypeScriptGenerate.Configurations.Environments;

namespace SampleSystem.TypeScriptGenerate.Configurations.Services
{
    public class AuthorizationServiceFacadeConfiguration : BaseFacadeGenerationConfiguration<AuthorizationGenerationEnvironment>
    {
        internal AuthorizationServiceFacadeConfiguration(AuthorizationGenerationEnvironment environment)
            : base(environment)
        {
        }

        protected override ITypeScriptMethodPolicy CreateGeneratePolicy()
        {
            var policy1 = new TypeScriptMethodPolicyBuilder<Authorization.WebApi.Controllers.PrincipalController>();
            policy1.Add(m => m.GetCurrentPrincipal());

            var policy2 = new TypeScriptMethodPolicyBuilder<Authorization.WebApi.Controllers.OperationController>();
            policy2.Add(m => m.GetSecurityOperations());

            return policy1.Add(policy2);
        }

        public override IEnumerable<Type> GetFacadeTypes()
        {
            var t = typeof(Authorization.WebApi.Controllers.PermissionController);

            return t.Assembly.GetTypes().Where(v => typeof(ControllerBase).IsAssignableFrom(v) && v.Namespace == t.Namespace);
        }

        public override IEnumerable<RequireJsModule> GetModules()
        {
            return base.GetModules()
                       .Concat(
                               new List<RequireJsModule>
                               {
                                   new RequireJsModule(
                                                       "* as dto",
                                                       "../dto/authorization.generated",
                                                       "Framework.Authorization",
                                                       "Framework.Authorization.Generated",
                                                       "Framework.Authorization.Generated.DTO"),
                                   new RequireJsModule(
                                                       "* as mockdto",
                                                       "../../mocked-dto",
                                                       "Framework.Notification.DTO"),
                               });
        }

        public override string GetGenericFacadeMethodInvocation(bool isPrimitiveType)
        {
            return isPrimitiveType ? "createAuthSimpleService" : "createAuthService";
        }
    }
}
