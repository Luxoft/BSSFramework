using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Framework.Core;

namespace Framework.DomainDriven.UnitTest.Mock.StubProxy;

internal class OverrideMethodBuilder<TSource> : IOverrideMethodBuilder<TSource>
{
    private readonly IList<IOverrideMethodInfo> _overrideMethods;

    public OverrideMethodBuilder()
    {
        this._overrideMethods = new List<IOverrideMethodInfo>();
    }

    public IEnumerable<IOverrideMethodInfo> OverrideMethods
    {
        get { return this._overrideMethods; }
    }

    public void OverrideProperty<TValue>(Expression<Func<TSource, TValue>> propertyExpression, TValue result)
    {
        var propertyName = propertyExpression.ToPath();
        var propertyInfo = typeof(TSource).GetProperty(propertyName);

        var info = new OverrideMethodInfo<TSource, TValue>(propertyInfo.GetGetMethod(), result);

        this._overrideMethods.Add(info);
    }

    public void OverrideMethod<TValue>(Expression<Func<TSource, TValue>> methodExpression, TValue result)
    {
        var memberException = methodExpression.Body as MemberExpression;

        var methodName = methodExpression.ToPath();
        methodName = methodName.TrimEnd(')').TrimEnd('(');

        var methodInfo = typeof(TSource).GetMethods().First(z => z.Name == methodName);

        var info = new OverrideMethodInfo<TSource, TValue>(methodInfo, result);

        this._overrideMethods.Add(info);
    }

    class OverrideMethodInfo<TSource, TValue> : IOverrideMethodInfo
    {
        public OverrideMethodInfo(MethodInfo methodBase, TValue returnValue)
        {
            this.ReturnValue = returnValue;
            this.MethodBase = methodBase;
        }

        public MethodInfo MethodBase { get; private set; }
        public object ReturnValue { get; private set; }
    }

}
