using System;

namespace SampleSystem.IntegrationTests;

public interface IRootServiceProviderContainer
{
    IServiceProvider RootServiceProvider { get; }
}
