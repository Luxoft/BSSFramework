using System;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public interface IRootServiceProviderContainer
{
    IServiceProvider RootServiceProvider { get; }
}
