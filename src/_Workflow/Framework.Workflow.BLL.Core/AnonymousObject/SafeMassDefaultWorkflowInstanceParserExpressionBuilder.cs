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
    internal class SafeMassDefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>
        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        protected readonly Type AnonType;


        public SafeMassDefaultWorkflowInstanceParserExpressionBuilder(Type anonType)
        {
            if (anonType == null) throw new ArgumentNullException(nameof(anonType));

            this.AnonType = anonType;
        }



        public LambdaExpression GetLambdaParseExpression()
        {
            var contextParameterExpr = Expression.Parameter(typeof(TBLLContext));
            var workflowInstancesParameterExpr = Expression.Parameter(typeof(IEnumerable<WorkflowInstance>));

            var initExpr = this.GetParseExpression(contextParameterExpr, workflowInstancesParameterExpr);

            return Expression.Lambda(initExpr, contextParameterExpr, workflowInstancesParameterExpr);
        }


        private Expression GetParseExpression(Expression contextExpr, Expression workflowInstancesExpr)
        {
            if (contextExpr == null) throw new ArgumentNullException(nameof(contextExpr));
            if (workflowInstancesExpr == null) throw new ArgumentNullException(nameof(workflowInstancesExpr));

            return ExpressionHelper.Create((IEnumerable<WorkflowInstance> workflowInstance) => workflowInstance.ToParametersCache())
                .GetBodyWithOverrideParameters(workflowInstancesExpr)
                .WithPipe(workflowInstanceWithParametersExpr =>
                {
                    var cacheProp = this.AnonType.GetProperties().Where(property => property.Name == WorkflowParameter.OwnerWorkflowName || typeof(TPersistentDomainObjectBase).IsAssignableFrom(property.PropertyType));

                    return this.GetParseExpressionWithCache(contextExpr, workflowInstanceWithParametersExpr, cacheProp, caches =>
                        workflowInstanceWithParametersExpr.WithSelect(workflowInstancePairExpr =>
                        {
                            var bindingExpressions = this.AnonType.GetProperties()
                                                                  .ToList(property => this.GetMemberBinding(workflowInstancePairExpr, property, caches));


                            return bindingExpressions.WithTryResultSelect(parameters =>
                            {
                                var bindings = this.AnonType.GetProperties().ZipStrong(parameters, Expression.Bind);

                                return Expression.MemberInit(Expression.New(this.AnonType), bindings);
                            });
                        }));
                });
        }

        private Expression GetParseExpressionWithCache(Expression contextExpr, Expression workflowInstanceWithParametersExpr, IEnumerable<PropertyInfo> cachingProperties, Func<Dictionary<PropertyInfo, ParameterExpression>, Expression> getResultExpr)
        {
            if (contextExpr == null) throw new ArgumentNullException(nameof(contextExpr));
            if (workflowInstanceWithParametersExpr == null) throw new ArgumentNullException(nameof(workflowInstanceWithParametersExpr));
            if (cachingProperties == null) throw new ArgumentNullException(nameof(cachingProperties));
            if (getResultExpr == null) throw new ArgumentNullException(nameof(getResultExpr));

            var caches = cachingProperties.ToDictionary(prop => prop, prop =>
            {
                if (prop.Name == WorkflowParameter.OwnerWorkflowName)
                {
                    var method = new Func<Expression, Expression, Expression>(this.GetOwnerCache<object>).CreateGenericMethod(prop.PropertyType);

                    return (Expression)method.Invoke(this, new object[] { contextExpr, workflowInstanceWithParametersExpr });
                }
                else
                {
                    var method = new Func<Expression, PropertyInfo, Expression, Expression>(this.GetDomainObjectCache<TPersistentDomainObjectBase>).CreateGenericMethod(prop.PropertyType);

                    return (Expression)method.Invoke(this, new object[] { contextExpr, prop, workflowInstanceWithParametersExpr });
                }
            });

            return caches.Values.WithPipe(parameters =>
            {
                var newCache = caches.Keys.ZipStrong(parameters, (prop, parameter) => prop.ToKeyValuePair(parameter)).ToDictionary();

                return getResultExpr(newCache);
            });
        }

        private Expression GetMemberBinding(Expression workflowInstancePairExpr, PropertyInfo property, Dictionary<PropertyInfo, ParameterExpression> caches)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (caches == null) throw new ArgumentNullException(nameof(caches));

            return caches.GetMaybeValue(property).Match(cache =>
            {
                var workflowInstanceExpr = Expression.Property(workflowInstancePairExpr, "Key");

                var method = new Func<Expression, PropertyInfo, ParameterExpression, Expression>(this.GetMemberBindingByCache<object>).CreateGenericMethod(property.PropertyType);

                return (Expression)method.Invoke(this, new object[] { workflowInstanceExpr, property, cache });

            }, () =>
            {
                var valueExpr = Expression.Property(workflowInstancePairExpr, "Value");

                var propertyName = property.Name;

                var valueExpression = ExpressionHelper.Create((Dictionary<string, string> wfParameters) => TryResult.Catch(() => wfParameters[propertyName]))
                                                      .ExpandConst()
                                                      .GetBodyWithOverrideParameters(valueExpr);

                if (property.PropertyType == typeof(string))
                {
                    return valueExpression;
                }
                else
                {
                    return this.GetParseExpression(property.PropertyType, valueExpression);
                }
            });
        }

        private Expression GetMemberBindingByCache<TProperty>(Expression workflowInstanceExpr, PropertyInfo property, ParameterExpression cache)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (cache == null) throw new ArgumentNullException(nameof(cache));


            if (property.Name == WorkflowParameter.OwnerWorkflowName)
            {
                return ExpressionHelper.Create((WorkflowInstance wf, Dictionary<WorkflowInstance, ITryResult<TProperty>> typedCache) => typedCache[wf.Owner])
                                       .GetBodyWithOverrideParameters(workflowInstanceExpr, cache);
            }
            else
            {
                return ExpressionHelper.Create((WorkflowInstance wf, Dictionary<WorkflowInstance, ITryResult<TProperty>> typedCache) => typedCache[wf])
                                       .GetBodyWithOverrideParameters(workflowInstanceExpr, cache);
            }
       }


        private Expression GetDomainObjectCache<TDomainObject>(Expression contextExpr, PropertyInfo property, Expression workflowInstanceWithParametersExpr)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var propertyName = property.Name;

            var toDomainTypeCacheExpr = ExpressionHelper.Create((TBLLContext context, Dictionary<WorkflowInstance, Dictionary<string, string>> workflowInstanceWithParameters) =>

                workflowInstanceWithParameters.ToSafeDomainTypeCache(idents => context.Logics.Default.Create<TDomainObject>().GetObjectsByIdentsUnsafe(idents, default(IFetchContainer<TDomainObject>)), propertyName))
                                              .ExpandConst();

            return toDomainTypeCacheExpr.GetBodyWithOverrideParameters(contextExpr, workflowInstanceWithParametersExpr);
        }

        private Expression GetOwnerCache<TOwnerWorkflow>(Expression contextExpr, Expression workflowInstanceWithParametersExpr)
            where TOwnerWorkflow : class
        {
            var ownerBuilder = new SafeMassDefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>(typeof(TOwnerWorkflow));

            var ownerWorkflowInstancesParameterExpr = Expression.Parameter(typeof(IEnumerable<WorkflowInstance>));


            var selectorBody = ownerBuilder.GetLambdaParseExpression().GetBodyWithOverrideParameters(contextExpr, ownerWorkflowInstancesParameterExpr);


            var keys = Expression.Property(workflowInstanceWithParametersExpr, "Keys");

            var toOwnersMethod = new Func<IEnumerable<WorkflowInstance>, Func<IEnumerable<WorkflowInstance>, IEnumerable<ITryResult<TOwnerWorkflow>>>, Dictionary<WorkflowInstance, ITryResult<TOwnerWorkflow>>>(SafeMassWorkflowInstanceExtensions.ToSafeOwners).Method;


            return Expression.Call(toOwnersMethod, keys, Expression.Lambda(selectorBody, ownerWorkflowInstancesParameterExpr));
        }


        private Expression GetParseExpression(Type type, Expression valueExpression)
        {
            var catchMethod = new Func<Func<object>, ITryResult<object>>(TryResult.Catch).CreateGenericMethod(type);

            return Expression.Call(catchMethod, Expression.Lambda(ParserHelper.GetParseExpression(type).GetBodyWithOverrideParameters(valueExpression)));
        }
    }




    internal static class SafeMassWorkflowInstanceExtensions
    {
        public static Dictionary<WorkflowInstance, ITryResult<TOwnerWorkflow>> ToSafeOwners<TOwnerWorkflow>(this IEnumerable<WorkflowInstance> source, Func<IEnumerable<WorkflowInstance>, IEnumerable<ITryResult<TOwnerWorkflow>>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var owners = source.Select(wfInstance => wfInstance.Owner).Distinct().ToList();

            return owners.ZipStrong(selector(owners), (ownerWorkflow, result) => ownerWorkflow.ToKeyValuePair(result)).ToDictionary();
        }


        public static Dictionary<WorkflowInstance, ITryResult<TDomainObject>> ToSafeDomainTypeCache<TDomainObject>(this Dictionary<WorkflowInstance, Dictionary<string, string>> source, Func<IEnumerable<Guid>, IList<TDomainObject>> selector, string propertyName)
            where TDomainObject : IIdentityObject<Guid>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var withTryIdents = source.ChangeValue(propertyDict => TryResult.Catch(() => Guid.Parse(propertyDict[propertyName])));

            var parsedIdents = withTryIdents.Values.GetResults().ToArray();

            var domainObjects = selector(parsedIdents).ToDictionary(obj => obj.Id, obj => obj);

            return withTryIdents.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.
                            SelectMany(domainObjectId => TryResult.Catch(() =>

                                domainObjects.GetMaybeValue(domainObjectId)
                                             .GetValue(() => new ObjectByIdNotFoundException(typeof(TDomainObject), domainObjectId)))));
        }





        public static Expression WithTryResultSelect(this Expression sourceExpression, LambdaExpression selector)
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var parameterType = selector.Parameters.Single().Type;
            var resultType = selector.ReturnType.GetInterfaceImplementationArgument(typeof (ITryResult<>));

            if (resultType == null)
            {
                var selectyMethod = new Func<ITryResult<object>, Func<object, object>, ITryResult<object>>(TryResultExtensions.Select).CreateGenericMethod(parameterType, selector.ReturnType);

                return Expression.Call(null, selectyMethod, sourceExpression, selector);
            }
            else
            {
                var selectManyMethod = new Func<ITryResult<object>, Func<object, ITryResult<object>>, ITryResult<object>>(TryResultExtensions.SelectMany).CreateGenericMethod(parameterType, resultType);

                return Expression.Call(null, selectManyMethod, sourceExpression, selector);
            }
        }

        public static Expression WithTryResultSelect(this Expression sourceExpression, Func<ParameterExpression, Expression> getBody)
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
            if (getBody == null) throw new ArgumentNullException(nameof(getBody));

            var parameter = Expression.Parameter(sourceExpression.Type.GetInterfaceImplementationArgument(typeof(ITryResult<>)) ?? sourceExpression.Type);

            var body = getBody(parameter);

            var selector = Expression.Lambda(body, parameter);

            return sourceExpression.WithTryResultSelect(selector);
        }


        public static Expression WithTryResultSelect(this IEnumerable<Expression> sourceExpressions, Func<ParameterExpression[], Expression> getBody)
        {
            if (sourceExpressions == null) throw new ArgumentNullException(nameof(sourceExpressions));
            if (getBody == null) throw new ArgumentNullException(nameof(getBody));

            using (var sourceEnumerator = sourceExpressions.GetEnumerator())
            {
                return sourceEnumerator.WithTryResultSelect(new ParameterExpression[0], getBody);
            }
        }

        private static Expression WithTryResultSelect(this IEnumerator<Expression> sourceEnumerator, IEnumerable<ParameterExpression> parameters, Func<ParameterExpression[], Expression> getBody)
        {
            if (sourceEnumerator == null) throw new ArgumentNullException(nameof(sourceEnumerator));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (getBody == null) throw new ArgumentNullException(nameof(getBody));


            if (sourceEnumerator.MoveNext())
            {
                return sourceEnumerator.Current.WithTryResultSelect(parameter =>

                    sourceEnumerator.WithTryResultSelect(parameters.Concat(new[] { parameter }), getBody));
            }
            else
            {
                return getBody(parameters.ToArray());
            }
        }
    }
}