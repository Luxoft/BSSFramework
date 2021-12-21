using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.OData;
using Framework.Persistent;
using Framework.SecuritySystem;
using JetBrains.Annotations;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Framework.DomainDriven.SerializeMetadata
{
    public class ODataReportBuilder<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode, TIdent> : BLLContextContainer<TBLLContext>, IODataReportBuilder

        where TBLLContext : class, IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, TIdent>>>,

            ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,

            IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,

            IConfigurationBLLContextContainer<IConfigurationBLLContext>,

            IAuthorizationBLLContextContainer<IAuthorizationBLLContextBase>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>

        where TSecurityOperationCode : struct, Enum
    {
        private readonly ISystemMetadataTypeBuilder _systemMetadataTypeBuilder;


        public ODataReportBuilder(TBLLContext context, [NotNull] ISystemMetadataTypeBuilder systemMetadataTypeBuilder)
            : base(context)
        {
            if (systemMetadataTypeBuilder == null) throw new ArgumentNullException(nameof(systemMetadataTypeBuilder));

            this._systemMetadataTypeBuilder = systemMetadataTypeBuilder;
        }


        public ISelectOperationResult<object> GetDynamicResult(TypeHeader domainTypeHeader, SelectOperation selectOperation)
        {
            if (domainTypeHeader == null) throw new ArgumentNullException(nameof(domainTypeHeader));
            if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));


            var domainTypeSubset = this._systemMetadataTypeBuilder.SystemMetadata.GetDomainTypeMetadataFullSubset(domainTypeHeader, selectOperation);

            var domainType = this._systemMetadataTypeBuilder.TypeResolver.Resolve(domainTypeHeader, true);

            var anonType = this._systemMetadataTypeBuilder.AnonymousTypeBuilder.GetAnonymousType(domainTypeSubset);

            var method = new Func<SelectOperation, DomainTypeSubsetMetadata, SelectOperationResult<object>>(this.GetDynamicResultByOData<TPersistentDomainObjectBase, object>)
                .CreateGenericMethod(domainType, anonType);

            return (ISelectOperationResult<object>)method.Invoke(this, new object[] { selectOperation, domainTypeSubset });
        }

        public Stream GetReport(TypeHeader domainTypeHeader, SelectOperation selectOperation)
        {
            if (domainTypeHeader == null) throw new ArgumentNullException(nameof(domainTypeHeader));
            if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));


            var selectResult = this.GetDynamicResult(domainTypeHeader, selectOperation);

            return (Stream)new Func<SelectOperationResult<object>, Stream>(this.GetReport<object>)
                .CreateGenericMethod(selectResult.ElementType)
                .Invoke(this, new object[] { selectResult });
        }

        private SelectOperationResult<TAnonType> GetDynamicResultByOData<TDomainObject, TAnonType>(SelectOperation selectOperation, DomainTypeSubsetMetadata domainTypeSubsetMetadata)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));

            var bll = this.Context.Logics.Default.Create<TDomainObject>(BLLSecurityMode.View);

            var fetchContainer = this.Context.FetchService.GetContainer<TDomainObject>(selectOperation);

            var preResult = bll.GetObjectsByOData(selectOperation, fetchContainer);

            var converter = new AnonTypeConverter<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode, TDomainObject, TAnonType>(
                LambdaCompileAnonCacheContainer.Get<TDomainObject, TAnonType>(),
                this.Context.SecurityService);

            return preResult.Select(converter.Convert);
        }




        private Stream GetReport<TODataObject>(SelectOperationResult<TODataObject> selectResult)
        {
            if (selectResult == null) throw new ArgumentNullException(nameof(selectResult));

            var plainType = LambdaCompileAnonPlainConvertHelper<TODataObject>.ConvertExpression.Body.Type;

            return (Stream)new Func<SelectOperationResult<object>, Stream>(this.GetReport<object, object>)
                .CreateGenericMethod(typeof(TODataObject), plainType)
                .Invoke(this, new object[] { selectResult });
        }

        private Stream GetReport<TODataObject, TPlainODataObject>(SelectOperationResult<TODataObject> selectResult)
        {
            if (selectResult == null) throw new ArgumentNullException(nameof(selectResult));

            var convertFunc = LambdaCompileAnonPlainConvertHelper<TODataObject, TPlainODataObject>.ConvertFunc;

            var properties = typeof(TPlainODataObject).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select((property, columnIndex) => new { Property = property, ColumnIndex = columnIndex })
                .ToArray();

            var plainData = selectResult.Items.ToArray(convertFunc);

            using (var myPackage = new ExcelPackage())
            {
                var sheet = myPackage.Workbook.Worksheets.Add("Data");

                foreach (var pair in properties)
                {
                    {
                        var headerCell = sheet.Cells[1, pair.ColumnIndex + 1];

                        headerCell.Value = pair.Property.Name;

                        var cellStyle = headerCell.Style.Fill;
                        cellStyle.PatternType = ExcelFillStyle.Solid;
                        cellStyle.BackgroundColor.SetColor(Color.Gray);
                    }


                    for (var rowIndex = 0; rowIndex < plainData.Length; rowIndex++)
                    {
                        var valueCell = sheet.Cells[rowIndex + 1, pair.ColumnIndex + 1];

                        var value = pair.Property.GetValue(plainData[rowIndex], null);

                        valueCell.Value = value.ToString();
                    }
                }

                sheet.Cells.AutoFitColumns();

                var result = new MemoryStream();
                myPackage.SaveAs(result);

                result.Seek(0, SeekOrigin.Begin);

                return result;
            }
        }


        private static readonly IAnonymousTypeBuilder<TypeMap> PlainODataTypeBuilder =

            new AnonymousTypeByPropertyBuilder<TypeMap, TypeMapMember>(new AnonymousTypeBuilderStorage("OData_MaybePlainTypeExpander")).WithCompressName().WithCache().WithLock();


        private static class LambdaCompileAnonPlainConvertHelper<TODataObject>
        {
            public static readonly LambdaExpression ConvertExpression =

                new MaybePlainTypeExpander("_", PlainODataTypeBuilder).GetExpressionConverter(typeof(TODataObject))
                    .GetConvertExpressionBase();
        }

        private static class LambdaCompileAnonPlainConvertHelper<TODataObject, TPlainODataObject>
        {
            public static readonly Func<TODataObject, TPlainODataObject> ConvertFunc =

                (LambdaCompileAnonPlainConvertHelper<TODataObject>.ConvertExpression as Expression<Func<TODataObject, TPlainODataObject>>).Compile();
        }

        private static readonly LambdaCompileCacheContainer LambdaCompileAnonCacheContainer = new LambdaCompileCacheContainer();
    }
}
