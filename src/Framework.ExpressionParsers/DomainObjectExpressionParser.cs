using System.Linq.Expressions;
using Framework.Core;

using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.ExpressionParsers;

public abstract class DomainObjectExpressionParser<TDomainObject, TLambdaObject> : DomainObjectExpressionParser<TDomainObject, TLambdaObject, Delegate, LambdaExpression>
        where TDomainObject : class
        where TLambdaObject : class, ILambdaObject
{
    protected DomainObjectExpressionParser(INativeExpressionParser parser, Expression<Func<TDomainObject, TLambdaObject>> lambdaPath)
            : base(parser, expr => expr.Compile(), lambdaPath)
    {

    }
}

public abstract class DomainObjectExpressionParser<TDomainObject, TLambdaObject, TDelegate> : DomainObjectExpressionParser<TDomainObject, TLambdaObject, TDelegate, Expression<TDelegate>>
        where TDomainObject : class
        where TLambdaObject : class, ILambdaObject
        where TDelegate : class
{
    protected DomainObjectExpressionParser(INativeExpressionParser parser, Expression<Func<TDomainObject, TLambdaObject>> lambdaPath)
            : base(parser, expr => expr.Compile(), lambdaPath)
    {

    }


    protected override Expression<TDelegate> GetInternalExpression(string lambdaValue)
    {
        return this.Parser.Parse<TDelegate>(lambdaValue);
    }
}

public abstract class DomainObjectExpressionParser<TDomainObject, TLambdaObject, TDelegate, TExpression> :

        LambdaObjectExpressionParser<TLambdaObject, TDelegate, TExpression>,
        IExpressionParser<TDomainObject, TDelegate, TExpression>

        where TDomainObject : class
        where TExpression : LambdaExpression
        where TLambdaObject : class, ILambdaObject where TDelegate : class
{
    private readonly Func<TDomainObject, TLambdaObject> _getLambdaFunc;

    private readonly string _membeName;


    protected DomainObjectExpressionParser(INativeExpressionParser parser, Func<TExpression, TDelegate> compileFunc, Expression<Func<TDomainObject, TLambdaObject>> lambdaPath)
            : base(parser, compileFunc)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));
        if (compileFunc == null) throw new ArgumentNullException(nameof(compileFunc));


        this._getLambdaFunc = lambdaPath.Compile(this.CompileCache);

        this._membeName = lambdaPath.GetMemberName();
    }


    protected virtual string GetFormattedDomainObject(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        return typeof(TDomainObject).Name;
    }

    protected TResult TryWrapOperation<TResult>(TDomainObject domainObject, bool wrapError, Func<TLambdaObject, TResult> getResult)
    {
        if (getResult == null) throw new ArgumentNullException(nameof(getResult));

        var subscriptionLambda = this._getLambdaFunc(domainObject);

        if (subscriptionLambda == null) throw new ArgumentException($"{this._membeName} not initilialized", nameof(domainObject));

        try
        {
            return getResult(subscriptionLambda);
        }
        catch (Exception ex)
        {
            if (wrapError)
            {
                throw new BusinessLogicException(ex,
                                                 $"Can't parse lambda \"{subscriptionLambda.Name}\" with value: \"{subscriptionLambda.Value}\". {this._membeName} of {this.GetFormattedDomainObject(domainObject)} expected format: {this.ExpectedFormat}");
            }
            else
            {
                throw;
            }
        }
    }


    public TExpression GetExpression(TDomainObject domainObject, bool wrapError = true)
    {
        return this.TryWrapOperation(domainObject, wrapError, lambda => this.GetExpression(lambda, false));
    }

    public TDelegate GetDelegate(TDomainObject domainObject, bool wrapError = true)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        return this.TryWrapOperation(domainObject, wrapError, lambda => this.GetDelegate(lambda, false));
    }

    public void Validate(TDomainObject domainObject)
    {
        this.GetExpression(domainObject);
    }



    private readonly ILambdaCompileCache CompileCache = new LambdaCompileCache();
}
