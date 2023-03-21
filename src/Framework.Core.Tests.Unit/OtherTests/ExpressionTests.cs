using System;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core.ExpressionComparers;
using Framework.Core.Serialization;

using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

[TestFixture]
public class ExpressionTests
{
    [Test]
    public void TestPropertyPaths()
    {
        var serializer = Serializer.GetDefault<int, DayOfWeek>();


        var val = serializer.Serialize(DayOfWeek.Monday);

        return;


        //var prop1 = typeof(Obj1).GetProperty("Value");

        //Expression<Func<Obj1, int>> exp = obj => obj.Value;

        //var fixExpr = exp.UpdateBody(FixPropertySourceVisitor.Value);

        //var prop2 = (exp.Body as MemberExpression).Member as PropertyInfo;

        //var prop3 = (fixExpr.Body as MemberExpression).Member as PropertyInfo;


        //var prop4 = Expression.Property(Expression.Parameter(typeof(Obj1)), "Value").Member as PropertyInfo;

        return;

        //Expression<Func<Obj1, int>> expr1 = obj => obj.Value;

        //var path1 = expr1.GetPropertyPath(false);
        //var path2 = expr1.GetPropertyPath(true);




        //var expr2 = Expression.Parameter(typeof(Obj1)).Pipe(parameter =>
        //    Expression.Lambda<Func<Obj1, int>>(Expression.Property(parameter, path2[0]), parameter));

        //var path3 = expr2.GetPropertyPath(false);


        //var eq = ExpressionComparer.Value.Equals(expr1, expr2);

        return;
    }
}

public class Obj1 : Obj2
{
    //public Obj2 Obj2 { get; }
}

public class Obj2
{
    public int Value { get; }
}
