using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core.ExpressionComparers;

namespace Framework.Core;

public class LambdaCompileCache : ILambdaCompileCache
{
    private readonly LambdaCompileMode _mode;


    private readonly Dictionary<LambdaExpression, Delegate> _cache = new Dictionary<LambdaExpression, Delegate>(LambdaComparer.Value);

    private readonly object _locker = new object();


    public LambdaCompileCache(LambdaCompileMode mode = LambdaCompileMode.None)
    {
        this._mode = mode;
    }


    public TDelegate GetFunc<TDelegate>(Expression<TDelegate> lambdaExpression)
    {
        if (lambdaExpression == null) throw new ArgumentNullException(nameof(lambdaExpression));

        ReadOnlyCollection<Tuple<ParameterExpression, ConstantExpression>> args = null;

        var newLambdaExpression =

                lambdaExpression.Pipe(this._mode.HasFlag(LambdaCompileMode.OptimizeBooleanLogic), lambda => lambda.Optimize())
                                .Pipe(lambda => ConstantToParameters(lambda, out args));

        var getDelegateFunc = this.GetGetDelegate(newLambdaExpression, args.Select(v => v.Item1));

        return (TDelegate)getDelegateFunc.DynamicInvoke(args.Select(v => v.Item2.Value).ToArray());
    }

    private Delegate GetGetDelegate(LambdaExpression expr, IEnumerable<ParameterExpression> parameters)
    {
        lock (this._locker)
        {
            return (this._cache.GetValueOrCreate(expr, () =>
                                                               expr.Pipe(this._mode.HasFlag(LambdaCompileMode.IgnoreStringCase), lambda => lambda.UpdateBodyBase(new OverrideStringEqualityExpressionVisitor(StringComparison.CurrentCultureIgnoreCase)))
                                                                   .Pipe(this._mode.HasFlag(LambdaCompileMode.InjectMaybe), InjectMaybeVisitor.Value.VisitAndGetValueOrDefaultBase)
                                                                   .Pipe(body => Expression.Lambda(body, parameters))
                                                                   .Compile()));
        }
    }


    private static LambdaExpression ConstantToParameters(LambdaExpression lambdaExpression, out ReadOnlyCollection<Tuple<ParameterExpression, ConstantExpression>> args)
    {
        var listArgs = new List<Tuple<ParameterExpression, ConstantExpression>>();

        var newExpression = lambdaExpression.UpdateBase(new ConstantToParameterExpressionVisitor(listArgs));

        args = listArgs.ToReadOnlyCollection();

        return (LambdaExpression)newExpression;
    }

    private class ConstantToParameterExpressionVisitor : ExpressionVisitor
    {
        private readonly List<Tuple<ParameterExpression, ConstantExpression>> _args;


        public ConstantToParameterExpressionVisitor(List<Tuple<ParameterExpression, ConstantExpression>> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            this._args = args;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var newParameter = Expression.Parameter(node.Type, "OverrideConst_" + this._args.Count);

            this._args.Add(Tuple.Create(newParameter, node));

            return newParameter;
        }
    }

    public static readonly LambdaCompileCache Default = new LambdaCompileCache();
}
