using System;
using System.Linq.Expressions;

namespace Framework.DomainDriven.UnitTest.Mock.StubProxy;

public interface IOverrideMethodBuilder<TSource>
{
    void OverrideProperty<TValue>(Expression<Func<TSource, TValue>> propertyExpression, TValue result);

    void OverrideMethod<TValue>(Expression<Func<TSource, TValue>> methodExpression, TValue result);
}
