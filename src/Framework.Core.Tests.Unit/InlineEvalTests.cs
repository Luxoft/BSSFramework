using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Core.ExpressionComparers;

using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

[TestFixture]
public class InlineEvalTests
{
    [Test]
    public void MulIncExpr_InlineEval_ExprEquals()
    {
        // Arrange
        Expression<Func<int, int>> incExpr = x => x + 1;
        Expression<Func<int, int>> testExr = y => y * y;
        Expression<Func<int, int>> baseTestExpr = z => testExr.Eval(incExpr.Eval(z));

        Expression<Func<int, int>> expectedTestExpr = z => (z + 1) * (z + 1);

        // Act
        var inlinedTestExpr = baseTestExpr.InlineEval();

        // Assert
        var equals = ExpressionComparer.Value.Equals(inlinedTestExpr, expectedTestExpr);
        equals.Should().Be(true);
    }

    [Test]
    public void DeepQueryable_Case1_ExprEquals()
    {
        // Arrange
        Expression<Func<int, int>> incExpr = x => x + 1;
        Expression<Func<IQueryable<TestObject>, IQueryable<TestObject>>> baseTestExpr = q => q.Where(testObject => incExpr.Eval(testObject.Value) > 0);

        Expression<Func<IQueryable<TestObject>, IQueryable<TestObject>>> expectedTestExpr = q => q.Where(testObject => testObject.Value + 1 > 0);

        // Act
        var inlinedTestExpr = baseTestExpr.InlineEval();

        // Assert
        var equals = ExpressionComparer.Value.Equals(inlinedTestExpr, expectedTestExpr);
        equals.Should().Be(true);
    }

    [Test]
    public void DeepQueryable_Case2_ExprEquals()
    {
        // Arrange
        Expression<Func<TestObject, IEnumerable<TestObject>>> childExpr = testObject => testObject.Children;

        Expression<Func<int, int>> incExpr = x => x + 1;
        Expression<Func<IQueryable<TestObject>, IQueryable<TestObject>>> baseTestExpr = q => q.Where(rootTestObject => childExpr.Eval(rootTestObject).Any(childTestObject => incExpr.Eval(childTestObject.Value) > 0));

        Expression<Func<IQueryable<TestObject>, IQueryable<TestObject>>> expectedTestExpr = q => q.Where(rootTestObject => rootTestObject.Children.Any(childTestObject => childTestObject.Value + 1 > 0));

        // Act
        var inlinedTestExpr = baseTestExpr.InlineEval();

        // Assert
        var equals = ExpressionComparer.Value.Equals(inlinedTestExpr, expectedTestExpr);
        equals.Should().Be(true);
    }

    //[Test]
    //public void DeepQueryable_Case3_ExprEquals()
    //{
    //    // Arrange

    //    var supportObjectsQ = new[] { new SupportObject() }.AsQueryable();
        
    //    Expression<Func<TestObjectItem, int>> itemValueExpr = item => item.Value;
        
    //    Expression<Func<TestObject, IEnumerable<TestObjectItem>>> itemsExpr = testObject => testObject.Items;

    //    Expression<Func<TestObjectItem, SupportObject, bool>> supportObjectConditionExpr = (item, itemSo) => supportObjectsQ.Any(so => so == itemSo && so.Value == itemValueExpr.Eval(item));

    //    Expression<Func<TestObjectItem, bool>> itemConditionExpr = item => item.SupportObjects.Any(itemSo => supportObjectConditionExpr.Eval(item, itemSo));

    //    Expression<Func<TestObject, bool>> baseTestExpr = testObject => itemsExpr.Eval(testObject).Any(item => itemConditionExpr.Eval(item));

    //    Expression<Func<TestObject, bool>> baseExpectedTestExpr = testObject => testObject.Items.Any(item => item.SupportObjects.Any(itemSo => supportObjectsQ.Any(so => so == itemSo && so.Value == item.Value)));

    //    var expectedTestExpr = baseExpectedTestExpr.ExpandConst();

    //    // Act
    //    var inlinedTestExpr = baseTestExpr.ExpandConst().InlineEval();

    //    // Assert
    //    var equals = ExpressionComparer.Value.Equals(inlinedTestExpr, expectedTestExpr);
    //    equals.Should().Be(true);
    //}

    private class TestObject
    {
        public int Value { get; }

        public IEnumerable<TestObject> Children { get; }

        public IEnumerable<TestObjectItem> Items { get; }
    }

    private class TestObjectItem
    {
        public int Value { get; }

        public IEnumerable<SupportObject> SupportObjects { get; } // BU
    }

    private class SupportObject
    {
        public int Value { get; }
    }
}
