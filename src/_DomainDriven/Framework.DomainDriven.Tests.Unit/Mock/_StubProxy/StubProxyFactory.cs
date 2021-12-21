using System;
using Framework.Core;

using Unity.Interception;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

namespace Framework.DomainDriven.UnitTest.Mock.StubProxy
{
    public static class StubProxyFactory
    {
        public static T CreateStub<T>(T defaultSource, params Action<IOverrideMethodBuilder<T>>[] overrideMethods) where T : class
        {
            var overrideMethoBuilder = new OverrideMethodBuilder<T>();
            overrideMethods.Foreach(z => z(overrideMethoBuilder));

            var result = Intercept.ThroughProxy(
                defaultSource,
                new InterfaceInterceptor(),
                new[] { new OverrideMethodInterceptor(overrideMethoBuilder.OverrideMethods) });

            return result;
        }
    }
}
