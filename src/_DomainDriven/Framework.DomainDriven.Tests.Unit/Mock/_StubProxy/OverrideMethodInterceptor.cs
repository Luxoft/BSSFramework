using Framework.Core;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Framework.DomainDriven.UnitTest.Mock.StubProxy;

internal class OverrideMethodInterceptor : IInterceptionBehavior
{
    private readonly IList<IOverrideMethodInfo> _overrideMethodInfos;

    public OverrideMethodInterceptor(IEnumerable<IOverrideMethodInfo> overrideMethodInfos)
    {
        this._overrideMethodInfos = overrideMethodInfos.ToList();
    }
    [System.Diagnostics.DebuggerStepThrough]
    public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    {
        var overrideMethodInfo = this._overrideMethodInfos.SingleOrDefault(z =>
                                                                           {
                                                                               var nameEqual = z.MethodBase == input.MethodBase;

                                                                               if (nameEqual)
                                                                               {
                                                                                   return true;
                                                                               }

                                                                               var isEqual = (z.MethodBase.IsGenericMethod && input.MethodBase.IsGenericMethod)
                                                                                   && z.MethodBase.GetGenericMethodDefinition().Name == input.MethodBase.Name;


                                                                               return isEqual;
                                                                           });

        return overrideMethodInfo.Maybe(z=>new VirtualMethodReturn(input, z.ReturnValue, new object[0]))
               ?? getNext()(input, getNext);
    }

    public IEnumerable<Type> GetRequiredInterfaces()
    {
        yield break;
    }

    public bool WillExecute
    {
        get { return true; }
    }
}
