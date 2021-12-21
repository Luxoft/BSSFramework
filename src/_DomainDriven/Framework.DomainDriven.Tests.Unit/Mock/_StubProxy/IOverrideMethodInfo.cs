using System.Reflection;

namespace Framework.DomainDriven.UnitTest.Mock.StubProxy
{
    public interface IOverrideMethodInfo
    {
        MethodInfo MethodBase { get; }
        object ReturnValue { get; }
    }
}