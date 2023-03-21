using System;
using Framework.Configuration.Core;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Lambdas;

internal class LambdaWrapper<TContext, TDomainObject, TResult>
        where TDomainObject : class
{
    private readonly Func<TContext, DomainObjectVersions<TDomainObject>, TResult> typedLambda;

    public LambdaWrapper(Func<TContext, DomainObjectVersions<TDomainObject>, TResult> typedLambda)
    {
        this.typedLambda = typedLambda;
    }

    public Func<object, object, object> UntypedLambda
    {
        get
        {
            return
                    (context, versions) =>
                            this.typedLambda((TContext) context, (DomainObjectVersions<TDomainObject>) versions);
        }
    }
}
