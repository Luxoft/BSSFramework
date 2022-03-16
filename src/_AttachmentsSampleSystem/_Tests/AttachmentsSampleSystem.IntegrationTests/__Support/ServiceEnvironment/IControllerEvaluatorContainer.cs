using System;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public interface IControllerEvaluatorContainer
{
    IServiceProvider RootServiceProvider { get; }
}
