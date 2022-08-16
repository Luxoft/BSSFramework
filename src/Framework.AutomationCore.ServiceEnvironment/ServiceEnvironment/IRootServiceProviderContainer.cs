using System;

namespace Automation.ServiceEnvironment;

public interface IRootServiceProviderContainer
{
    IServiceProvider RootServiceProvider { get; }
}
