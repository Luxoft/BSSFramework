using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL;

public class ReportSecurityProvider : ISecurityProvider<Domain.Reports.Report>
{
    private readonly IConfigurationBLLContext context;

    private readonly Lazy<Expression<Func<Domain.Reports.Report, bool>>> securityFilterExpr;

    private readonly Lazy<Func<Domain.Reports.Report, bool>> securityFilterFunc;

    public ReportSecurityProvider(IConfigurationBLLContext context)
    {
        this.context = context;
        this.securityFilterExpr = new Lazy<Expression<Func<Domain.Reports.Report, bool>>>(() => this.GetSecurityFilter(this.GetSecurityFilterModel()));
        this.securityFilterFunc = new Lazy<Func<Domain.Reports.Report, bool>>(() => this.securityFilterExpr.Value.Compile());
    }

    public IConfigurationBLLContext Context => this.context;

    public IQueryable<Domain.Reports.Report> InjectFilter(IQueryable<Domain.Reports.Report> queryable)
    {
        var securityExpression = this.securityFilterExpr.Value;

        return queryable.Where(securityExpression);
    }

    public bool HasAccess(Domain.Reports.Report domainObject)
    {
        return this.securityFilterFunc.Value(domainObject);
    }

    public Exception GetAccessDeniedException(Domain.Reports.Report domainObject, Func<string, string> formatMessageFunc = null)
    {
        return this.context.AccessDeniedExceptionService.GetAccessDeniedException(domainObject, formatMessageFunc: formatMessageFunc);
    }

    public UnboundedList<string> GetAccessors(Domain.Reports.Report domainObject)
    {
        return UnboundedList<string>.Empty;
    }

    private Expression<Func<Domain.Reports.Report, bool>> GetSecurityFilter(CustomReportSecurityFilterModel filterModel)
    {
        Expression<Func<Domain.Reports.Report, bool>> securityFilter = z => z.CreatedBy == filterModel.Owner;

        securityFilter = securityFilter.BuildOr(z => z.AccessableOperations.Any(q => filterModel.OperationIdents.Contains(q.Value)));

        securityFilter = securityFilter.BuildOr(z => z.AccessableBusinessRoles.Any(q => filterModel.BusinessRoleIdents.Contains(q.Value)));

        securityFilter = securityFilter.BuildOr(z => z.AccessablePrincipals.Any(q => q.Value == filterModel.Owner));

        securityFilter = securityFilter.BuildOr(z => !z.AccessablePrincipals.Any() && !z.AccessableOperations.Any() && !z.AccessableBusinessRoles.Any());

        return securityFilter;
    }

    private CustomReportSecurityFilterModel GetSecurityFilterModel()
    {
        var currentPrincipalName = this.context.Authorization.CurrentPrincipalName;

        var businessRoles = this.context.Authorization.Logics.Permission.GetUnsecureQueryable()
                                .Where(z => z.Principal.Name == currentPrincipalName)
                                .Select(z => z.Role.Id)
                                .Distinct();

        var operations = this.context.Authorization.Logics.Default.Create<BusinessRoleOperationLink>()
                             .GetUnsecureQueryable()
                             .Where(z => businessRoles.Contains(z.BusinessRole.Id))
                             .Select(z => z.Operation.Id)
                             .Distinct();

        var filter = new CustomReportSecurityFilterModel(currentPrincipalName, businessRoles, operations);

        return filter;
    }

    private class CustomReportSecurityFilterModel
    {
        public CustomReportSecurityFilterModel(string owner, IQueryable<Guid> businessRoles, IQueryable<Guid> operations)
        {
            this.Owner = owner;
            this.BusinessRoleIdents = businessRoles;
            this.OperationIdents = operations;
        }

        public IQueryable<Guid> BusinessRoleIdents { get; private set; }

        public IQueryable<Guid> OperationIdents { get; private set; }

        public string Owner { get; private set; }
    }
}
