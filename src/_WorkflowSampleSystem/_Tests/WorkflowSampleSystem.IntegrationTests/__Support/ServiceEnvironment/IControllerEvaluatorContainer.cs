using System;

namespace WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public interface IControllerEvaluatorContainer
{
    IServiceProvider RootServiceProvider { get; }
}
