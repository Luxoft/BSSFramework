using System;

using Automation.ServiceEnvironment;

using Framework.Core;

namespace SampleSystem.BLL.Test;

public abstract class TestBase : RootServiceProviderContainer<ISampleSystemBLLContext>
{
    protected TestBase() : base(LazyInterfaceImplementHelper.CreateNotImplemented<IServiceProvider>())
    {
    }
}
