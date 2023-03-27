using System.Linq.Expressions;

using Framework.Core;

using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.ExpressionParsers;

public abstract class LambdaObjectExpressionParser<TLambdaObject> : LambdaObjectExpressionParser<TLambdaObject, Delegate, LambdaExpression>
        where TLambdaObject : class, ILambdaObject
{
    protected LambdaObjectExpressionParser(INativeExpressionParser parser)
            : base(parser, expr => expr.Compile(LambdaCompileCache))
    {

    }

    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache();
}

public abstract class LambdaObjectExpressionParser<TLambdaObject, TDelegate> : LambdaObjectExpressionParser<TLambdaObject, TDelegate, Expression<TDelegate>>
        where TLambdaObject : class, ILambdaObject
        where TDelegate : class
{
    protected LambdaObjectExpressionParser(INativeExpressionParser parser)
            : base(parser, expr => expr.Compile(LambdaCompileCache))
    {

    }

    protected override Expression<TDelegate> GetInternalExpression(string value)
    {
        return this.Parser.Parse<TDelegate>(value);
    }


    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache();
}

public abstract class LambdaObjectExpressionParser<TLambdaObject, TDelegate, TExpression> : ExpressionParser<TDelegate, TExpression>,

    IExpressionParser<TLambdaObject, TDelegate, TExpression>

        where TLambdaObject : class, ILambdaObject
        where TDelegate : class
        where TExpression : LambdaExpression
{

    protected LambdaObjectExpressionParser(INativeExpressionParser parser, Func<TExpression, TDelegate> compileFunc)
            : base(parser, compileFunc)
    {
    }


    protected TResult TryWrapOperation<TResult>(TLambdaObject lambda, bool wrapError, Func<TResult> getResult)
    {
        if (lambda == null) throw new ArgumentNullException(nameof(lambda));
        if (getResult == null) throw new ArgumentNullException(nameof(getResult));

        try
        {
            return getResult();
        }
        catch (Exception ex)
        {
            if (wrapError)
            {
                throw new BusinessLogicException(ex, $"Can't parse lambda \"{lambda.Name}\" with value: \"{lambda.Value}\". Expected format: {this.ExpectedFormat}");
            }
            else
            {
                throw;
            }
        }
    }



    public TExpression GetExpression(TLambdaObject lambda, bool wrapError = true)
    {
        return this.TryWrapOperation(lambda, wrapError, () => this.GetExpression(lambda.Value, false));
    }

    public TDelegate GetDelegate(TLambdaObject lambda, bool wrapError = true)
    {
        return this.TryWrapOperation(lambda, wrapError, () => this.GetDelegate(lambda.Value, false));
    }

    public void Validate(TLambdaObject lambda)
    {
        this.GetExpression(lambda);
    }
}
