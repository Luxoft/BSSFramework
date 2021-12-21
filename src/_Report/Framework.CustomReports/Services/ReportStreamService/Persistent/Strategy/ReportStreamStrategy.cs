using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Configuration.Domain.Reports;
using Framework.Core;
using Framework.CustomReports.Services.ExcelBuilder;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.Persistent;

namespace Framework.CustomReports.Services.Persistent.Strategy
{
    internal abstract class ReportStreamStrategy<TMainBLLContext, TDomainObject, TPersistentDomainObjectBase, TSecurityOperationCode>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid> where TMainBLLContext :
        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
        DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<Configuration.BLL.IConfigurationBLLContext>,
        IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>,
        ISecurityServiceContainer<IRootSecurityService<TMainBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>
        where TSecurityOperationCode : struct, Enum

    {
        protected readonly TMainBLLContext context;

        protected readonly SystemMetadata systemMetadata;

        protected ReportStreamStrategy(TMainBLLContext context, SystemMetadata systemMetadata)
        {
            this.context = context;
            this.systemMetadata = systemMetadata;
        }

        public abstract Stream Generate(ReportGenerationModel model, List<(ReportProperty reportProperty, PropertyMetadata[] propertyMetadataChain, PropertyInfo[] propertyInfoChain)> reportPropertyToMetadataLinks);

        protected IExcelReportStreamService GetExcelReportStreamService()
        {
            return new PlainExcelReportStreamService();
        }

        protected virtual EvaluateParameterInfo GetReportParameterInfos(ReportGenerationModel model)
        {
            return new EvaluateParameterInfo(
                model.PredefineGenerationValues.EmptyIfNull().Select(z => new EvaluateParameterInfoItem(z.Name, z.DesignValue))
                .Concat(model.Items.Select(z => new EvaluateParameterInfoItem(z.Parameter.Name, z.DesignValue ?? z.Value)))
                .ToList());
        }

        protected ExcelDesignProperty<TAnon> GetExcelDesignProperty<TAnon>(ReportProperty reportProperty, Func<TAnon, object> renderFunc, Type renderedValueType)
        {
            return new ExcelDesignProperty<TAnon>(
                reportProperty.Alias,
                renderFunc,
                DefaultExcelCellFormat.GetDefaultFormat(renderedValueType),
                this.GetFormula(reportProperty.Formula),
                (column, range) =>
                {
                    column.AutoFit(column.ColumnMin, 60);
                });
        }

        protected ExcelDesignFormula GetFormula(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                return null;
            }

            if (formula.StartsWith("hyperlink"))
            {
                return ExcelDesignFormula.CreateHyperlink(formula);
            }

            return ExcelDesignFormula.CreateExcelFormula(formula);
        }
    }
}
