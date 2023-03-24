using System;
using System.Collections.Generic;

using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using System.Linq;

using Framework.Configuration.Core;
using Framework.Core;
using Framework.DomainDriven.Serialization;

namespace Framework.Configuration.Domain.Reports;

[ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.ReportView)]
[ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.ReportEdit)]
[BLLViewRole, BLLSaveRole, BLLRemoveRole]
[NotAuditedClass]
public class Report : AuditPersistentDomainObjectBase,
                      IMaster<ReportProperty>,
                      IMaster<ReportFilter>,
                      IMaster<ReportParameter>,
                      IMaster<AccessableBusinessRoleReportRight>,
                      IMaster<AccessableOperationReportRight>,
                      IMaster<AccessablePrincipalReportRight>,
                      IVersionObject<long>
{
    private string name;

    private string description;

    private string owner;

    private string domainTypeName;

    private int? securityOperationCode;

    private long version;

    private ICollection<ReportProperty> properties = new List<ReportProperty>();

    private ICollection<ReportFilter> filters = new List<ReportFilter>();

    private ICollection<ReportParameter> parameters = new List<ReportParameter>();

    private ICollection<AccessableBusinessRoleReportRight> accessableBusinessRoles = new List<AccessableBusinessRoleReportRight>();

    private ICollection<AccessableOperationReportRight> accessableOperations = new List<AccessableOperationReportRight>();

    private ICollection<AccessablePrincipalReportRight> accessablePrincipals = new List<AccessablePrincipalReportRight>();

    private ReportType reportType = ReportType.Persistent;

    public Report()
    {
    }

    public Report(Guid id)
    {
        this.Id = id;
    }

    [Required]
    public virtual string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public virtual int? SecurityOperationCode
    {
        get { return this.securityOperationCode; }
        set { this.securityOperationCode = value; }
    }

    [MaxLength]
    public virtual string Description
    {
        get { return this.description; }
        set { this.description = value; }
    }

    public virtual string Owner
    {
        get { return this.owner; }
        set { this.owner = value; }
    }

    public virtual string DomainTypeName
    {
        get { return this.domainTypeName; }
        set { this.domainTypeName = value; }
    }

    [CustomSerialization(CustomSerializationMode.Normal, DTORole.Client)]
    public override bool Active
    {
        get { return base.Active; }
        set { base.Active = value; }
    }

    public virtual string SortBy
    {
        get
        {
            return this.Properties
                       .Where(x => x.SortType != 0)
                       .OrderBy(x => x.SortOrdered)
                       .Select(x => x.Alias)
                       .Join(", ");
        }
    }

    public virtual IEnumerable<ReportProperty> Properties
    {
        get { return this.properties; }
    }

    public virtual IEnumerable<ReportFilter> Filters
    {
        get { return this.filters; }
    }

    [Version]
    public virtual long Version
    {
        get { return this.version; }
        set { this.version = value; }
    }

    [UniqueGroup]
    public virtual IEnumerable<ReportParameter> Parameters
    {
        get { return this.parameters; }
    }

    public virtual IEnumerable<AccessableOperationReportRight> AccessableOperations
    {
        get { return this.accessableOperations; }
    }

    public virtual IEnumerable<AccessableBusinessRoleReportRight> AccessableBusinessRoles
    {
        get { return this.accessableBusinessRoles; }
    }

    public virtual IEnumerable<AccessablePrincipalReportRight> AccessablePrincipals
    {
        get { return this.accessablePrincipals; }
    }

    public virtual ReportType ReportType
    {
        get { return this.reportType; }
        set { this.reportType = value; }
    }

    ICollection<ReportProperty> IMaster<ReportProperty>.Details
    {
        get { return (ICollection<ReportProperty>)this.Properties; }
    }

    ICollection<ReportFilter> IMaster<ReportFilter>.Details
    {
        get { return (ICollection<ReportFilter>)this.Filters; }
    }

    ICollection<ReportParameter> IMaster<ReportParameter>.Details
    {
        get { return (ICollection<ReportParameter>)this.Parameters; }
    }

    ICollection<AccessableBusinessRoleReportRight> IMaster<AccessableBusinessRoleReportRight>.Details
    {
        get { return (ICollection<AccessableBusinessRoleReportRight>)this.AccessableBusinessRoles; }
    }

    ICollection<AccessableOperationReportRight> IMaster<AccessableOperationReportRight>.Details
    {
        get { return (ICollection<AccessableOperationReportRight>)this.AccessableOperations; }
    }

    ICollection<AccessablePrincipalReportRight> IMaster<AccessablePrincipalReportRight>.Details
    {
        get { return (ICollection<AccessablePrincipalReportRight>)this.AccessablePrincipals; }
    }
}
