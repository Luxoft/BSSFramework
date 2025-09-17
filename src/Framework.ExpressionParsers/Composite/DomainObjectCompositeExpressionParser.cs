using System.Collections.ObjectModel;
using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;

using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.ExpressionParsers;

public abstract class DomainObjectCompositeExpressionParser<TDomainObject, TLambdaObject> : DomainObjectExpressionParser<TDomainObject, TLambdaObject>
        where TDomainObject : class
        where TLambdaObject : class, ILambdaObject
{
    private readonly Lazy<ReadOnlyCollection<IExpressionParser<string, Delegate, LambdaExpression>>> _lazyParsers;

    private readonly Lazy<string> _lazyExpectedFormat;


    protected DomainObjectCompositeExpressionParser(INativeExpressionParser parser, Expression<Func<TDomainObject, TLambdaObject>> lambdaPath)
            : base(parser, lambdaPath)
    {
        this._lazyParsers = LazyHelper.Create(() => this.GetParsers().ToReadOnlyCollection());

        this._lazyExpectedFormat = LazyHelper.Create(() => this._lazyParsers.Value.Join(" or ", p => p.ExpectedFormat));
    }


    public override string ExpectedFormat
    {
        get { return this._lazyExpectedFormat.Value; }
    }


    protected abstract IEnumerable<IExpressionParser<string, Delegate, LambdaExpression>> GetParsers();


    protected sealed override LambdaExpression GetInternalExpression(string lambdaValue)
    {
        return this._lazyParsers.Value.Evaluate(parser => parser.GetExpression(lambdaValue, false), errors => errors.Aggregate());
    }
}
