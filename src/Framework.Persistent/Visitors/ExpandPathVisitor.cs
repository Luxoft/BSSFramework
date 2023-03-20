using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

using BinaryExpression = System.Linq.Expressions.BinaryExpression;
using Expression = System.Linq.Expressions.Expression;

namespace Framework.Persistent;

public class ExpandPathVisitor : ExpressionVisitor
{
    private static readonly IDictionaryCache<Tuple<Type, MemberInfo>, PropertyPath> ExpandPathCache = new DictionaryCache<Tuple<Type, MemberInfo>, PropertyPath>(memberPair =>
    {
        var pathsRequest = from property in (memberPair.Item2 as PropertyInfo).ToMaybe()

                           let implementedProperty = memberPair.Item1.GetImplementedProperty(property)

                           from expandPath in implementedProperty.GetExpandPath().ToMaybe()

                           select expandPath;

        return pathsRequest.GetValueOrDefault();
    }).WithLock();


    private ExpandPathVisitor()
    {

    }


    protected override Expression VisitMember(MemberExpression node)
    {
        var request = from expr in node.Expression.ToMaybe()

                      from properties in ExpandPathCache.GetValue(expr.Type, node.Member).ToMaybe()

                      let result = properties.Aggregate(this.Visit(node.Expression), Expression.Property)

                      select node.Type.IsNullable() ? result.TryLiftToNullable() : result;


        return request.GetValueOrDefault(() => base.VisitMember(node));
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (LiftableOperators.Contains(node.NodeType))
        {
            return this.VisitWithLiftToNullable(new[] { node.Left, node.Right },
                                                args => Expression.MakeBinary(node.NodeType, args[0], args[1], false, node.Method, node.Conversion),
                                                () => base.VisitBinary(node));
        }
        else
        {
            return base.VisitBinary(node);
        }
    }

    private Expression VisitWithLiftToNullable(IReadOnlyCollection<Expression> args, Func<List<Expression>, Expression> success, Func<Expression> fault)
    {
        var anyNullable = args.Any(arg => arg.Type.IsNullable());

        return anyNullable ? success(args.ToList(this.Visit).ToList(arg => arg.TryLiftToNullable())) : fault();
    }

    public static readonly ExpandPathVisitor Value = new ExpandPathVisitor();

    private static readonly HashSet<ExpressionType> LiftableOperators = new[]
                                                                        {
                                                                                ExpressionType.Or,
                                                                                ExpressionType.OrElse,
                                                                                ExpressionType.And,
                                                                                ExpressionType.AndAlso,

                                                                                ExpressionType.Add,
                                                                                ExpressionType.Subtract,
                                                                                ExpressionType.Multiply,
                                                                                ExpressionType.Divide,
                                                                                ExpressionType.Power,

                                                                                ExpressionType.Equal,
                                                                                ExpressionType.NotEqual,

                                                                                ExpressionType.GreaterThan,
                                                                                ExpressionType.LessThan,
                                                                                ExpressionType.GreaterThanOrEqual,
                                                                                ExpressionType.LessThanOrEqual
                                                                        }.ToHashSet();
}
