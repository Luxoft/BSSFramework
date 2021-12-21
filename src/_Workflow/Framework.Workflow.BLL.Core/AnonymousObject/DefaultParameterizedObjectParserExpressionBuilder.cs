using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    internal class DefaultParameterizedObjectParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase, TParameterizedObject>
        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TParameterizedObject : IParametersContainer<IParameterInstanceBase<Parameter>>
    {
        protected readonly Type AnonType;


        public DefaultParameterizedObjectParserExpressionBuilder(Type anonType)
        {
            if (anonType == null) throw new ArgumentNullException(nameof(anonType));

            this.AnonType = anonType;
        }



        public LambdaExpression GetLambdaParseExpression()
        {
            var contextParameterExpr = Expression.Parameter(typeof(TBLLContext));
            var parameterizedObjectParameterExpr = Expression.Parameter(typeof(TParameterizedObject));

            var initExpr = this.GetParseExpression(contextParameterExpr, parameterizedObjectParameterExpr);

            return Expression.Lambda(initExpr, contextParameterExpr, parameterizedObjectParameterExpr);
        }


        protected Expression GetParseExpression(Expression contextExpr, Expression parameterizedObjectExpr)
        {
            return ExpressionHelper.Create((TParameterizedObject parameterizedObject) => parameterizedObject.GetParameters())
                                   .GetBodyWithOverrideParameters(parameterizedObjectExpr)
                                   .WithPipe(parametersExpr =>
            {
                var bindings = from property in this.AnonType.GetProperties()

                               let bindExpr = this.GetMemberBinding(contextExpr, parameterizedObjectExpr, property, parametersExpr)

                               select Expression.Bind(property, bindExpr);

                return Expression.MemberInit(Expression.New(this.AnonType), bindings);
            });
        }

        protected virtual Expression GetMemberBinding(Expression contextExpr, Expression parameterizedObjectExpr, PropertyInfo property, Expression parametersExpr)
        {
            var propertyName = property.Name;

            var valueExpression = ExpressionHelper.Create((Dictionary<string, string> wfParameters) => wfParameters.GetValue(propertyName, () => new BusinessLogicException("Property {0} not exists in dict.", propertyName)))
                .ExpandConst()
                .GetBodyWithOverrideParameters(parametersExpr);

            if (property.PropertyType == typeof(string))
            {
                return valueExpression;
            }
            else if (typeof(TPersistentDomainObjectBase).IsAssignableFrom(property.PropertyType))
            {
                var method = new Func<Expression, Expression, Expression>(this.GetDomainObjectMemberBinding<TPersistentDomainObjectBase>)
                    .CreateGenericMethod(property.PropertyType);

                return method.Invoke<Expression>(this, contextExpr, valueExpression);
            }
            else
            {
                return this.GetParseExpression(property.PropertyType, valueExpression);
            }
        }

        private Expression GetDomainObjectMemberBinding<TDomainObject>(Expression contextExpr, Expression valueExpression)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var guidExpr = this.GetParseExpression(typeof(Guid), valueExpression);

            var getObjectByIdExpr = ExpressionHelper.Create((TBLLContext context, Guid id) => context.Logics.Default.Create<TDomainObject>().GetById(id, IdCheckMode.SkipEmpty, null, LockRole.None));

            return getObjectByIdExpr.GetBodyWithOverrideParameters(contextExpr, guidExpr);
        }

        private Expression GetParseExpression(Type type, Expression valueExpression)
        {
            return ParserHelper.GetParseExpression(type).GetBodyWithOverrideParameters(valueExpression);
        }
    }
}
