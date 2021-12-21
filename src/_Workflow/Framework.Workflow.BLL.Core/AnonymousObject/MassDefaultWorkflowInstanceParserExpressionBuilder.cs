using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    internal class MassDefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>
        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        protected readonly Type AnonType;


        public MassDefaultWorkflowInstanceParserExpressionBuilder(Type anonType)
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
                            var bindings = from property in this.AnonType.GetProperties()

                                           let bindExpr = this.GetMemberBinding(workflowInstancePairExpr, property, caches)

                                           select Expression.Bind(property, bindExpr);

                            return Expression.MemberInit(Expression.New(this.AnonType), bindings);

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

                var valueExpression = ExpressionHelper.Create((Dictionary<string, string> wfParameters) => wfParameters[propertyName])
                                                      .ExpandConst()
                                                      .GetBodyWithOverrideParameters(valueExpr);

                if (property.PropertyType == typeof(string))
                {
                    return valueExpression;
                }
                else
                {
                    return ParserHelper.GetParseExpression(property.PropertyType).GetBodyWithOverrideParameters(valueExpression);
                }
            });
        }

        private Expression GetMemberBindingByCache<TProperty>(Expression workflowInstanceExpr, PropertyInfo property, ParameterExpression cache)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (cache == null) throw new ArgumentNullException(nameof(cache));


            if (property.Name == WorkflowParameter.OwnerWorkflowName)
            {
                return ExpressionHelper.Create((WorkflowInstance wf, Dictionary<WorkflowInstance, TProperty> typedCache) => typedCache[wf.Owner])
                                       .GetBodyWithOverrideParameters(workflowInstanceExpr, cache);
            }
            else
            {
                return ExpressionHelper.Create((WorkflowInstance wf, Dictionary<WorkflowInstance, TProperty> typedCache) => typedCache[wf])
                                       .GetBodyWithOverrideParameters(workflowInstanceExpr, cache);
            }
       }


        private Expression GetDomainObjectCache<TDomainObject>(Expression contextExpr, PropertyInfo property, Expression workflowInstanceWithParametersExpr)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var propertyName = property.Name;



            var toDomainTypeCacheExpr = ExpressionHelper.Create((TBLLContext context, Dictionary<WorkflowInstance, Dictionary<string, string>> workflowInstanceWithParameters) =>

                workflowInstanceWithParameters.ToDomainTypeCache(idents => context.Logics.Default.Create<TDomainObject>().GetObjectsByIdents(idents, default(IFetchContainer<TDomainObject>)), propertyName))
                                                        .ExpandConst();

            return toDomainTypeCacheExpr.GetBodyWithOverrideParameters(contextExpr, workflowInstanceWithParametersExpr);
        }

        private Expression GetOwnerCache<TOwnerWorkflow>(Expression contextExpr, Expression workflowInstanceWithParametersExpr)
            where TOwnerWorkflow : class
        {
            var ownerBuilder = new MassDefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>(typeof(TOwnerWorkflow));

            var ownerWorkflowInstancesParameterExpr = Expression.Parameter(typeof(IEnumerable<WorkflowInstance>));


            var selectorBody = ownerBuilder.GetLambdaParseExpression().GetBodyWithOverrideParameters(contextExpr, ownerWorkflowInstancesParameterExpr);


            var keys = Expression.Property(workflowInstanceWithParametersExpr, "Keys");

            var toOwnersMethod = new Func<IEnumerable<WorkflowInstance>, Func<IEnumerable<WorkflowInstance>, IEnumerable<TOwnerWorkflow>>, Dictionary<WorkflowInstance, TOwnerWorkflow>>(MassWorkflowInstanceExtensions.ToOwners).Method;


            return Expression.Call(toOwnersMethod, keys, Expression.Lambda(selectorBody, ownerWorkflowInstancesParameterExpr));
        }
    }




    internal static class MassWorkflowInstanceExtensions
    {
        public static Dictionary<WorkflowInstance, TOwnerWorkflow> ToOwners<TOwnerWorkflow>(this IEnumerable<WorkflowInstance> source, Func<IEnumerable<WorkflowInstance>, IEnumerable<TOwnerWorkflow>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var owners = source.Select(wfInstance => wfInstance.Owner).Distinct().ToList();

            return owners.ZipStrong(selector(owners), (ownerWorkflow, result) => ownerWorkflow.ToKeyValuePair(result)).ToDictionary();
        }


        public static Dictionary<WorkflowInstance, TDomainObject> ToDomainTypeCache<TDomainObject>(this Dictionary<WorkflowInstance, Dictionary<string, string>> source, Func<IEnumerable<Guid>, IEnumerable<TDomainObject>> selector, string propertyName)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var domainObjects = source.Select(pair => new Guid(pair.Value[propertyName])).Pipe(selector);

            return source.ZipStrong(domainObjects, ((pair, domainObject) => pair.Key.ToKeyValuePair(domainObject))).ToDictionary();
        }

        public static Dictionary<WorkflowInstance, Dictionary<string, string>> ToParametersCache(this IEnumerable<WorkflowInstance> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.ToDictionary(wfInstance => wfInstance, wfInstance => wfInstance.GetParameters());
        }
    }
}