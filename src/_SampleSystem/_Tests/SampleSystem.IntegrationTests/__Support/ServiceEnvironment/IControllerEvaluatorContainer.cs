using System;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public interface IControllerEvaluatorContainer
{
    IServiceProvider RootServiceProvider { get; }
}
