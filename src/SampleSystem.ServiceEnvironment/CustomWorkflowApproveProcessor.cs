using System;

using Framework.Authorization.BLL;

using JetBrains.Annotations;

using SampleSystem.BLL;

namespace SampleSystem.ServiceEnvironment
{
    public class CustomWorkflowApproveProcessor : WorkflowApproveProcessor
    {
        public CustomWorkflowApproveProcessor([NotNull] IAuthorizationBLLContext authContext, [NotNull] ISampleSystemBLLContext mainContext)
            : base(authContext)
        {
            if (mainContext == null)
            {
                throw new ArgumentNullException(nameof(mainContext));
            }
        }
    }
}
