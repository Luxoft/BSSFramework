using System.Collections;
using System.Linq.Expressions;

using Framework.Core.Services;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.Audit;

public class AuditPropertyPair<TDomainObject>(
    IAuditProperty<TDomainObject, string> authorAudit,
    IAuditProperty<TDomainObject, DateTime?> dateAudit)
    : IEnumerable<IAuditProperty>
{
    public AuditPropertyPair(Expression<Func<TDomainObject, string>> authorPropertyExpr, Expression<Func<TDomainObject, DateTime?>> datePropertyExpr, ICurrentUser currentUser, TimeProvider timeProvider)
            : this(new AuditProperty<TDomainObject, string>(authorPropertyExpr, () => currentUser.Name), new AuditProperty<TDomainObject, DateTime?>(datePropertyExpr, () => timeProvider.GetLocalNow().DateTime))
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
        ICurrentUser currentUser,
        TimeProvider timeProvider,
        Expression<Func<IAuditObject, string>> authorPropertyExpr,
        Expression<Func<IAuditObject, DateTime?>> datePropertyExpr)
        : base(authorPropertyExpr, datePropertyExpr, currentUser, timeProvider)
    {
    }

    public AuditPropertyPair(IAuditProperty<IAuditObject, string> authorAudit, IAuditProperty<IAuditObject, DateTime?> dateAudit)
        : base(authorAudit, dateAudit)
    {
    }


    public static AuditPropertyPair
        GetCreateAuditProperty(ICurrentUser currentUser, TimeProvider timeProvider) => new AuditPropertyPair(
        currentUser,
        timeProvider,
        obj => obj.CreatedBy,
        obj => obj.CreateDate);

    public static AuditPropertyPair
        GetModifyAuditProperty(ICurrentUser currentUser, TimeProvider timeProvider) => new AuditPropertyPair(
        currentUser,
        timeProvider,
        obj => obj.ModifiedBy,
        obj => obj.ModifyDate);
}
