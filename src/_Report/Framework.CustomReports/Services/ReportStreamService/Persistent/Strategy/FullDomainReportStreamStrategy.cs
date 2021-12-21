using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Configuration.Domain.Reports;
using Framework.Core;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services.ExcelBuilder;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.OData;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.CustomReports.Services.Persistent.Strategy
{
    internal class FullDomainReportStreamStrategy<TMainBLLContext, TDomainObject, TPersistentDomainObjectBase,
                                                  TSecurityOperationCode> :
        ReportStreamStrategy<TMainBLLContext, TDomainObject, TPersistentDomainObjectBase, TSecurityOperationCode>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TMainBLLContext : IBLLFactoryContainerContext<IBLLFactoryContainer<
        IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
        DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>,
        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,
        ISecurityServiceContainer<
        IRootSecurityService<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
        ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>

        where TSecurityOperationCode : struct, Enum
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        private static readonly ConcurrentDictionary<Tuple<Guid, string>, (Delegate func, Type propertyType)> propertyIdentToGetFuncDictionary = new ConcurrentDictionary<Tuple<Guid, string>, (Delegate func, Type propertyType)>();

        private static readonly HashSet<Guid> materializedPropertyFuncs = new HashSet<Guid>();

        private static readonly LambdaCompileCacheContainer LambdaCompileAnonCacheContainer =
            new LambdaCompileCacheContainer();

        private readonly ITypeResolver<TypeHeader> typeResolver;

        private readonly IAnonymousTypeBuilder<DomainTypeSubsetMetadata> anonymousTypeBuilder;

        private readonly
            List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[]
            propertyInfoChain)> reportPropertyToMetadataLinks;

        public FullDomainReportStreamStrategy(
            TMainBLLContext context,
            ITypeResolver<TypeHeader> typeResolver,
            SystemMetadata systemMetadata,
            IAnonymousTypeBuilder<DomainTypeSubsetMetadata> anonymousTypeBuilder,
            List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[]
                propertyInfoChain)> reportPropertyToMetadataLinks)
            : base(context, systemMetadata)
        {
            this.typeResolver = typeResolver;
            this.anonymousTypeBuilder = anonymousTypeBuilder;
            this.reportPropertyToMetadataLinks = reportPropertyToMetadataLinks;
        }

        public override Stream Generate(
            ReportGenerationModel model,
            List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[]
                propertyInfoChain)> propertyToMetadataLinks)
        {
            var selectOperation =
                new SelectOperationByReportParser<TDomainObject>(this.typeResolver, this.systemMetadata.Types).Parse(
                    model);

            var baseDomainTypeSubset = this.systemMetadata.GetDomainTypeMetadataFullSubset(
                new TypeHeader(typeof(TDomainObject)),
                selectOperation);

            var nextDomainTypeSubset = baseDomainTypeSubset.OverrideHeader(
                currentTypeHeader =>
                    new ReportTypeHeader(currentTypeHeader.Name, model.Report.Id, model.Report.Version));

            var anonType = this.anonymousTypeBuilder.GetAnonymousType(nextDomainTypeSubset);

            var method = new Func<ReportGenerationModel, SelectOperation, Stream>(this.Generate<object>)
                .CreateGenericMethod(anonType);

            return (Stream)method.Invoke(this, new object[] { model, selectOperation });
        }

        private Stream Generate<TAnonType>(ReportGenerationModel model, SelectOperation selectOperation)
        {
            if (selectOperation == null)
            {
                throw new ArgumentNullException("selectOperation");
            }

            IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, Guid> bll;

            if (this.context.GetSecurityOperation<TDomainObject>(BLLSecurityMode.View) != null)
            {
                bll = this.context.Logics.Implemented.Create<TDomainObject>(BLLSecurityMode.View);
            }
            else
            {
                bll = this.context.Logics.Implemented.Create<TDomainObject>();
            }

            var preLoad = this.GetPropertyInfoChains(this.reportPropertyToMetadataLinks);

            var loadResult = preLoad.Distinct((arg1, arg2) => arg1.SequenceEqual(arg2)).ToList();

            var fetchContainer = new FetchContainer<TDomainObject>(loadResult);

            var compressFetchContainer = fetchContainer.Compress();

            var preResult = bll.GetObjectsByOData(selectOperation, compressFetchContainer);

            var converter =
                new AnonTypeConverter<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode,
                    TDomainObject, TAnonType>(
                    LambdaCompileAnonCacheContainer.Get<TDomainObject, TAnonType>(),
                    this.context.SecurityService);

            var anonResults = preResult.Select(converter.Convert).Items;

            var headers = this.GetDesignPropertyInfos<TAnonType>(model.Report).ToHeaders();

            var generateReportParameterModel = this.GetReportParameterInfos(model);

            return this.GetExcelReportStreamService()
                       .Generate(
                           model.Report.Name,
                           headers,
                           anonResults,
                           generateReportParameterModel);
        }

        private IEnumerable<IEnumerable<PropertyInfo>> GetPropertyInfoChains(
            List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[]
                propertyInfoChain)> reportPropertyToMetadataLinks)
        {
            foreach (var expandReportProperty in reportPropertyToMetadataLinks)
            {
                yield return expandReportProperty.propertyInfoChain
                                                 .TakeWhile(
                                                     z => typeof(TPersistentDomainObjectBase).IsAssignableFrom(
                                                         z.PropertyType))
                                                 .ToList();
            }
        }

        protected IEnumerable<ExcelDesignProperty<TAnon>> GetDesignPropertyInfos<TAnon>(Configuration.Domain.Reports.Report report)
        {
            // TODO: в словарь
            return report.Properties
                         .OrderBy(z => z.Order)
                         .Select(z =>
                                 {
                                     var func = propertyIdentToGetFuncDictionary.GetOrAdd(Tuple.Create(z.Id, z.PropertyPath + report.DomainTypeName + report.Version), (_) =>
                                     {
                                         if (!materializedPropertyFuncs.Add(z.Id))
                                         {
                                             //remove previus funcs
                                             propertyIdentToGetFuncDictionary.Keys.Where(q => q.Item1 == z.Id)
                                                                             .ToList()
                                                                             .Foreach(q =>
                                                                             {
                                                                                 (Delegate func, Type propertyType) @delegate;
                                                                                 propertyIdentToGetFuncDictionary.TryRemove(q, out @delegate);
                                                                             });
                                         }

                                         return this.GetEvaluatePropertyFunc<TAnon>(z);
                                     });

                                     var castedFunc = (Func<TAnon, object>)func.func;

                                     return this.GetExcelDesignProperty(z, castedFunc, func.propertyType);
                                 });
        }

        protected (Func<TAnon, object> func, Type propertyType) GetEvaluatePropertyFunc<TAnon>(ReportProperty reportProperty)
        {
            Expression parameter = Expression.Parameter(typeof(TAnon), "z");

            var queue = new Queue<string>(reportProperty.GetPropertyNameChain());

            var currentType = typeof(TAnon);

            var currentExpression = parameter;

            while (queue.Any())
            {
                var currentProperty = queue.Dequeue();

                var property = currentType.GetProperty(currentProperty, StringComparison.CurrentCultureIgnoreCase, true);

                var expandPath = property.GetExpandPath(true);

                if (expandPath?.Count > 1)
                {
                    expandPath.Foreach(q => queue.Enqueue(q.Name));
                    continue;
                }

                currentExpression = Expression.Property(currentExpression, property.Name);

                currentType = property.PropertyType;

                var tryMaybe = currentType.GetMaybeElementType();
                if (tryMaybe != null)
                {
                    var maybeExtensionType = typeof(CustomEvaluateExtensions);

                    currentExpression = Expression.Call(maybeExtensionType, $"{nameof(CustomEvaluateExtensions.GetMaybeValue)}", new Type[] { tryMaybe }, currentExpression);
                    currentType = tryMaybe;

                }

                var tryNullableFrameworkType = currentType.GetNullableObjectElementType();
                if (tryNullableFrameworkType != null)
                {
                    var maybeExtensionType = typeof(CustomEvaluateExtensions);

                    currentExpression = Expression.Call(maybeExtensionType, $"{nameof(CustomEvaluateExtensions.GetNullableObjectValue)}", new Type[] { tryNullableFrameworkType }, currentExpression);

                    currentType = tryNullableFrameworkType;
                }

                var tryNullableType = currentType.GetNullableElementType();
                if (tryNullableType != null)
                {
                    var maybeExtensionType = typeof(CustomEvaluateExtensions);

                    currentExpression = Expression.Call(maybeExtensionType, $"{nameof(CustomEvaluateExtensions.GetValueOrNull)}", new Type[] { tryNullableType }, currentExpression);

                    currentType = tryNullableType;
                }
            }

            var convert = Expression.Convert(currentExpression, typeof(object));

            var expression = Expression.Lambda<Func<TAnon, object>>(convert, new[] { (ParameterExpression)parameter });

            var injectMaybeExpression = expression.InjectMaybe();

            return (func: injectMaybeExpression.Compile(), propertyType: currentType);
        }

    }
}
