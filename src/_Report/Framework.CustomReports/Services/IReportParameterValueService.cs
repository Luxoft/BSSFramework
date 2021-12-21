using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.Configuration.Domain.Reports;
using Framework.CustomReports.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.SerializeMetadata;
using Framework.OData;
using Framework.Persistent;

namespace Framework.CustomReports.Services
{
    public interface IReportParameterValueService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>

        where TBLLContext :
            IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>,
            IAuthorizationBLLContextContainer<IAuthorizationBLLContext>,
            IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<TPersistentDomainObjectBase, TSecurityOperationCode, Guid>>>,
            ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
            IFetchServiceContainer<TPersistentDomainObjectBase, FetchBuildRule>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TSecurityOperationCode : struct, Enum
    {
        ISelectOperationResult<ReportParameterValue> GetParameterValuesBy(
            ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext,
            ReportParameter parameter,
            SelectOperation<ReportParameterValue> selectOperation);

        ISelectOperationResult<ReportParameterValue> GetParameterValuesBy(
                ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext,
                string typeName,
                SelectOperation<ReportParameterValue> selectOperation);

        IList<int> GetParameterValuePositions(ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext, IList<ReportParameterValue> parameterValue);

        /// <summary>
        /// Get design(display) entity name by id and type
        /// </summary>
        /// <param name="reportServiceContext">context</param>
        /// <param name="id">Id</param>
        /// <param name="typeHeader">Type header</param>
        /// <returns>design(display) entity name</returns>
        string GetDesignValue(ReportServiceContext<TBLLContext, TSecurityOperationCode> reportServiceContext, Guid id, TypeHeader typeHeader);
    }
}
