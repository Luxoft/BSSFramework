using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.Security;

namespace Framework.DomainDriven.SerializeMetadata;

public class AnonTypeConverter<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode, TSource, TTarget> : ObjectConverter<TSource, TTarget>
        where TSecurityOperationCode : struct, Enum
        where TPersistentDomainObjectBase : class
{
    private readonly IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> _securityService;


    public AnonTypeConverter(ILambdaCompileCache compileCache, IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> securityService)
            : base(compileCache)
    {
        if (securityService == null) throw new ArgumentNullException(nameof(securityService));

        this._securityService = securityService;
    }


    protected override ExpressionConverter<TSubSource, TSubTarget> GetSubConverter<TSubSource, TSubTarget>()
    {
        return new AnonTypeConverter<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode, TSubSource, TSubTarget>(this.CompileCache, this._securityService);
    }


    protected override MemberExpression GetSourceMemberExpression(Expression source, MemberInfo member)
    {
        var sourcePropAttr = member.GetCustomAttribute<SourcePropertyNameAttribute>();

        return Expression.PropertyOrField(source, sourcePropAttr.BaseName);
    }

    protected override Expression GetMemberBindExpression(MemberExpression sourcePropExpr, Type targetPropType)
    {
        if (targetPropType.IsGenericTypeImplementation(typeof(Maybe<>)))
        {
            var viewDomainObjectAttribute = (sourcePropExpr.Member as PropertyInfo).GetViewDomainObjectAttribute();

            var domainType = sourcePropExpr.Expression.Type;

            LambdaExpression lambda;

            if (null == viewDomainObjectAttribute)
            {
                var method = new Func<Expression>(this.GetNoneSecurityMemberBindExpression<TPersistentDomainObjectBase, object>)
                             .Method
                             .GetGenericMethodDefinition()
                             .MakeGenericMethod(domainType, sourcePropExpr.Type);

                lambda = (LambdaExpression)method.Invoke(this, new object[0]);

            }
            else
            {
                var operation = (TSecurityOperationCode)(object)viewDomainObjectAttribute.SecurityOperationCode;

                var method = new Func<TSecurityOperationCode, Expression>(this.GetSecurityMemberBindExpression<TPersistentDomainObjectBase, object>)
                             .Method
                             .GetGenericMethodDefinition()
                             .MakeGenericMethod(domainType, sourcePropExpr.Type);

                lambda = (LambdaExpression)method.Invoke(this, new object[] { operation });
            }

            var domainObjParam = lambda.Parameters[0];
            var propParam = lambda.Parameters[1];

            var expr = lambda.Body.Override(domainObjParam, sourcePropExpr.Expression)
                             .Override(propParam, sourcePropExpr);


            var internalTargetType = targetPropType.GetMaybeElementType();


            return internalTargetType == sourcePropExpr.Type ? expr
                           : expr.WithSelect(this.GetSubConverterBase(sourcePropExpr.Type, internalTargetType).GetConvertExpressionBase());
        }

        return base.GetMemberBindExpression(sourcePropExpr, targetPropType);
    }

    private Expression<Func<TDomainObject, TPropValue, Maybe<TPropValue>>> GetSecurityMemberBindExpression<TDomainObject, TPropValue>(TSecurityOperationCode securityOperation)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return (domainObject, propValue) =>

                       Maybe.OfCondition(this._securityService.GetSecurityProvider<TDomainObject>(securityOperation).HasAccess(domainObject), () => propValue);
    }

    private Expression<Func<TDomainObject, TPropValue, Maybe<TPropValue>>> GetNoneSecurityMemberBindExpression<TDomainObject, TPropValue>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return (domainObject, propValue) => Maybe.Return(propValue);
    }
}
