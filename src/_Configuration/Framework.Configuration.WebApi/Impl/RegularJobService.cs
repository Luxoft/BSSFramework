using System.Collections.Generic;

using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Serilog.Context;

namespace Framework.Configuration.WebApi
{
    public partial class ConfigSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetFullRegularJobRevisionModelsBy))]
        public IEnumerable<RegularJobRevisionModelFullDTO> GetFullRegularJobRevisionModelsBy(RegularJobRevisionFilterModelStrictDTO filter)
        {
            return this.Evaluate(
                DBSessionMode.Read,
                evaluateData =>
                {
                    var regularJobRevisionFilterModel = filter.ToDomainObject(evaluateData.MappingService);
                    var result = evaluateData.Context.Logics.RegularJobFactory.Create(BLLSecurityMode.View)
                                             .GetRegularJobRevisionModelsBy(regularJobRevisionFilterModel);
                    return result.ToFullDTOList(evaluateData.MappingService);
                });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(SyncPulseJobs))]
        public void SyncPulseJobs()
        {
            this.EvaluateC(
                DBSessionMode.Read,
                context =>
                {
                    using (LogContext.PushProperty("Method", "SyncPulseJobs"))
                    {
                        context.Authorization.CheckAccess(ConfigurationSecurityOperation.SystemIntegration);

                        context.Logics.RegularJob.SyncRunAll();
                    }
                });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ForceRegularJobs))]
        public void ForceRegularJobs(ForceRegularJobsRequest request)
        {
            this.EvaluateRead(evaluateData =>
            {
                using (LogContext.PushProperty("Method", "ForceRegularJobs"))
                {
                    evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.RegularJobForce);

                    evaluateData.Context.Logics.RegularJob.SyncRun(request.RegularJobs.ToList(z => z.ToDomainObject(evaluateData.MappingService)), request.Mode);
                }
            });
        }
    }
}
