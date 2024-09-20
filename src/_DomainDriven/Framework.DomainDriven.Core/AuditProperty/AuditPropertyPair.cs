using System.Collections;
using System.Linq.Expressions;

using Framework.Core.Services;
using Framework.Persistent;

namespace Framework.DomainDriven.Audit;

public class AuditPropertyPair<TDomainObject>(
    IAuditProperty<TDomainObject, string> authorAudit,
    IAuditProperty<TDomainObject, DateTime?> dateAudit)
    : IEnumerable<IAuditProperty>
{
    public AuditPropertyPair(Expression<Func<TDomainObject, string>> authorPropertyExpr, Expression<Func<TDomainObject, DateTime?>> datePropertyExpr, IUserAuthenticationService userAuthenticationService, TimeProvider timeProvider)
            : this(new AuditProperty<TDomainObject, string>(authorPropertyExpr, userAuthenticationService.GetUserName), new AuditProperty<TDomainObject, DateTime?>(datePropertyExpr, () => timeProvider.GetLocalNow().DateTime))
    {
    }

    public IEnumerator<IAuditProperty> GetEnumerator()
    {
        yield return authorAudit;
        yield return dateAudit;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

public class AuditPropertyPair : AuditPropertyPair<IAuditObject>
{
    public AuditPropertyPair(
        IUserAuthenticationService userAuthenticationService,
        TimeProvider timeProvider,
        Expression<Func<IAuditObject, string>> authorPropertyExpr,
        Expression<Func<IAuditObject, DateTime?>> datePropertyExpr)
        : base(authorPropertyExpr, datePropertyExpr, userAuthenticationService, timeProvider)
    {
    }

    public AuditPropertyPair(IAuditProperty<IAuditObject, string> authorAudit, IAuditProperty<IAuditObject, DateTime?> dateAudit)
        : base(authorAudit, dateAudit)
    {
    }


    public static AuditPropertyPair
        GetCreateAuditProperty(IUserAuthenticationService userAuthenticationService, TimeProvider timeProvider) => new AuditPropertyPair(
        userAuthenticationService,
        timeProvider,
        obj => obj.CreatedBy,
        obj => obj.CreateDate);

    public static AuditPropertyPair
        GetModifyAuditProperty(IUserAuthenticationService userAuthenticationService, TimeProvider timeProvider) => new AuditPropertyPair(
        userAuthenticationService,
        timeProvider,
        obj => obj.ModifiedBy,
        obj => obj.ModifyDate);
}
