using System;
using System.Linq.Expressions;

using FluentAssertions;

using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

[TestFixture]
public class ExpressionExtensionsTests
{
    [Test]
    public void Optimize_EqNullExpressionLeftNull_ExecutionResultTrue()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => o == null;
        object x = null;

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    [Test]
    public void Optimize_CompareWithSelfNull_ExecutionResultTrue()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => o == o;
        object x = null;

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    [Test]
    public void Optimize_EqNullExpressionLeftNullConstLeft_ExecutionResultTrue()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => null == o;
        object x = null;

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    [Test]
    public void Optimize_EqNullExpressionLeftNotNull_ExecutionResultFalse()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => o == null;
        object x = new object();

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeFalse();
    }

    [Test]
    public void Optimize_CompareSelfNotNull_ExecutionResultTrue()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => o == o;
        object x = new object();

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    [Test]
    public void Optimize_EqNullExpressionLeftNotNullConstLeft_ExecutionResultFalse()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => null == o;
        object x = new object();

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeFalse();
    }

    [Test]
    public void Optimize_MethodCall_ExecutionResultFalse()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => o.ToString() == null;
        object x = new object();

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeFalse();
    }

    [Test]
    public void Optimize_MethodCallConstLeft_ExecutionResultFalse()
    {
        // Arrange
        Expression<Func<object, bool>> expr = o => null == o.ToString();
        object x = new object();

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeFalse();
    }

    [Test]
    public void Optimize_PropertyConstLeft_ExecutionResultTrue()
    {
        // Arrange
        Expression<Func<string, bool>> expr = o => 0 == o.Length;
        var x = string.Empty;

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    [Test]
    public void Optimize_OrInPredicate_ExecutionResultTrue()
    {
        // Arrange
        Expression<Func<string, bool>> expr = o => o.Length == 0 || o.Contains("z");
        var x = string.Empty;

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    [Test]
    public void Optimize_AndInPredicate_ExecutionResultFalse()
    {
        // Arrange
        Expression<Func<string, bool>> expr = o => o.Length == 0 && o.Contains("z");
        var x = string.Empty;

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeFalse();
    }

    [Test]
    public void Optimize_AndInPredicateResultTrue_ExecutionResultTrue()
    {
        // Arrange
        Expression<Func<string, bool>> expr = o => o.Length > 0 && o.Contains("z");
        var x = "z";

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-1191 Методы GetObjectsBy некорректно обрабатывают фильтры где выполняется условие с правой частью являющейся null
    /// </summary>
    [Test]
    public void Optimize_ComplexRightValueWhereCallShouldCauseNullReference_NullReferenceExceptionExpectedDuringExecution()
    {
        // Arrange
        A a = null;
        Expression<Func<string, bool>> expr = o => o == a.BProp.Prop;
        var x = "z";

        // Act
        var result = expr.Optimize();

        // Assert
        Action actiion = () => result.Compile()(x);
        actiion.Should().Throw<NullReferenceException>();
    }

    /// <summary>
    /// IADFRAME-1191 Методы GetObjectsBy некорректно обрабатывают фильтры где выполняется условие с правой частью являющейся null
    /// </summary>
    [Test]
    public void Optimize_ComplexRightValueWhereCallShouldNotCauseNullReference_ExecutionResultTrue()
    {
        // Arrange
        A a = new A { BProp = new B() };
        Expression<Func<string, bool>> expr = o => o == a.BProp.Prop;
        string x = null;

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-1191 Методы GetObjectsBy некорректно обрабатывают фильтры где выполняется условие с правой частью являющейся null
    /// </summary>
    [Test]
    public void Optimize_ComplexRightValue_ExecutionResultTrue()
    {
        // Arrange
        A a = new A { BProp = new B { Prop = "z" } };
        Expression<Func<string, bool>> expr = o => o == a.BProp.Prop;
        string x = "z";

        // Act
        var result = expr.Optimize();

        // Assert
        result.Compile()(x).Should().BeTrue();
    }

    private class A
    {
        public B BProp { get; set; }
    }

    private class B
    {
        public string Prop { get; set; }
    }
}
