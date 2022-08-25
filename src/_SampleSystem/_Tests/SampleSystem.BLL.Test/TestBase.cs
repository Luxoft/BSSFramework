using System;
using Automation;
using Framework.Core;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.BLL.Test;

public abstract class TestBase : IntegrationTestContextEvaluator<ISampleSystemBLLContext>
{
    protected TestBase() : base(LazyInterfaceImplementHelper.CreateNotImplemented<IServiceProvider>())
    {
    }
}
