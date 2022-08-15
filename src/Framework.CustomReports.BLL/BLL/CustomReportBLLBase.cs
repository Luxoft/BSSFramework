using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Core;
using Framework.CustomReports.Attributes;
using Framework.CustomReports.Domain;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.Validation;

namespace Framework.CustomReports.BLL
{
    /// <summary>
    /// Базовый класс отчетной BLL
    /// </summary>
    /// <typeparam name="TBLLContext"></typeparam>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    /// <typeparam name="TParameter"></typeparam>
    public abstract class CustomReportBLLBase<TBLLContext, TSecurityOperationCode, TPersistentDomainObjectBase, TReport, TParameter> : ICustomReportBLL<TParameter>
        where TBLLContext :
                IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
                IValidatorContainer

        where TParameter : class, new()

        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>

        where TSecurityOperationCode : struct, Enum

        where TReport : class, new()

    {
        protected internal TBLLContext Context { get; private set; }

        protected CustomReportBLLBase(TBLLContext context)
        {
            this.Context = context;
        }

        public virtual IReportStream GetReportStream(ReportGenerationModel model)
        {
            var parameter = this.CreateParameter(model);

            return this.GetReportStream(parameter);
        }

        public virtual IReportStream GetReportStream(TParameter parameter)
        {
            this.ValidateParameter(parameter);

            return this.GetStream(new TReport(), parameter);
        }

        protected abstract IReportStream GetStream(TReport report, TParameter parameter);

        protected virtual TParameter CreateParameter(ReportGenerationModel model)
        {
            var parameter = new TParameter();

            var properties = typeof(TParameter).GetProperties().Select(z => new
            {
                PropertyInfo = z,
                Name = z.GetCustomAttributes<ReportParameterDisplayNameAttribute>().FirstOrDefault().Maybe(q => q.Value, z.Name)
            }).ToList();

            var joined = properties.GroupJoin
                (model.Items,
                z => z.Name,
                z => z.Parameter.Name,
                (reflectionProperty, generationParameters) => new { reflectionProperty = reflectionProperty, generationParameters = generationParameters.ToList() });

            foreach (var pair in joined.Where(z => z.generationParameters.Any()))
            {
                var expandedPropertyType =
                    pair.reflectionProperty.PropertyInfo.PropertyType.GetCollectionOrArrayElementType() ??
                    pair.reflectionProperty.PropertyInfo.PropertyType;

                if (typeof(TPersistentDomainObjectBase).IsAssignableFrom(expandedPropertyType))
                {
                    object value = null;
                    if (pair.generationParameters.Count > 1 || pair.generationParameters.Any(z => z.Parameter.IsCollection))
                    {
                        var getDomainsFunc = new Func<IList<Guid>, Type, IEnumerable<TPersistentDomainObjectBase>>(this.GetDomainObjects<TPersistentDomainObjectBase>);

                        var genericMethod = getDomainsFunc.CreateGenericMethod(expandedPropertyType);

                        var idents = pair.generationParameters.Select(z => Guid.Parse(z.Value)).ToList();

                        value = genericMethod.Invoke(this, new object[] { idents, pair.reflectionProperty.PropertyInfo.PropertyType });
                    }
                    else
                    {
                        var getDomainFunc = new Func<Guid, TPersistentDomainObjectBase>(this.GetDomain<TPersistentDomainObjectBase>);

                        var genericMethod = getDomainFunc.CreateGenericMethod(expandedPropertyType);

                        value = genericMethod.Invoke(this, new object[] { Guid.Parse(pair.generationParameters.First().Value) });
                    }

                    pair.reflectionProperty.PropertyInfo.SetValue(parameter, value);
                }
                else
                {
                    if (pair.generationParameters.Any(z => z.Parameter.IsCollection))
                    {
                        var castToCollection = new Func<IEnumerable<string>, Type, IEnumerable<object>>(this.CastToCollection<object>);

                        var invokeResult = castToCollection.CreateGenericMethod(expandedPropertyType)
                            .Invoke(this, new object[]
                            {
                                pair.generationParameters.Select(z=>z.Value),
                                pair.reflectionProperty.PropertyInfo.PropertyType
                            });

                        pair.reflectionProperty.PropertyInfo.SetValue(parameter, invokeResult);
                    }
                    else
                    {
                        var convertedValue = this.ConvertToTypedValue(pair.generationParameters.First().Value, pair.reflectionProperty.PropertyInfo.PropertyType);

                        pair.reflectionProperty.PropertyInfo.SetValue(parameter, convertedValue);
                    }
                }
            }
            return parameter;
        }

        protected virtual object ConvertToTypedValue(string value, Type targetType)
        {
            var convertedValue = TypeDescriptor.GetConverter(targetType).ConvertFromString(value);
            return convertedValue;
        }


        protected virtual TDomainObject GetDomain<TDomainObject>(Guid ident)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            // TODO use securityCodeProvider
            return this.Context.Logics.Implemented.Create<TDomainObject>().GetById(ident, true);
        }

        protected virtual IEnumerable<TDomainObject> GetDomainObjects<TDomainObject>(IList<Guid> ident, Type targetType)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var preResult = this.Context.Logics.Implemented.Create<TDomainObject>().GetListByIdents(ident);

            if (targetType.IsArray)
            {
                return preResult.ToArray();
            }

            if (targetType.IsInterface)
            {
                return preResult;
            }

            return (IEnumerable<TDomainObject>)Activator.CreateInstance(targetType, preResult);
        }

        protected virtual IEnumerable<T> CastToCollection<T>(IEnumerable<string> values, Type collectionType)
        {
            var preResult = values.Select(z => this.ConvertToTypedValue(z, typeof(T))).Cast<T>().ToList();

            if (collectionType.IsArray)
            {
                return preResult.ToArray();
            }

            return preResult;

        }

        protected virtual void ValidateParameter(TParameter parameter)
        {
            this.Context.Validator.Validate(parameter);
        }
    }
}
