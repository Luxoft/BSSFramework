using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core.Services;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Audit
{
    public class AuditPropertyPair<TDomainObject> : IEnumerable<IAuditProperty>
    {
        private readonly IAuditProperty<TDomainObject, string> _authorAudit;

        private readonly IAuditProperty<TDomainObject, DateTime?> _dateAudit;


        public AuditPropertyPair([NotNull] Expression<Func<TDomainObject, string>> authorPropertyExpr, [NotNull] Expression<Func<TDomainObject, DateTime?>> datePropertyExpr, IUserAuthenticationService userAuthenticationService, IDateTimeService dateTimeService)
            : this(new AuditProperty<TDomainObject, string>(authorPropertyExpr, userAuthenticationService.GetUserName), new AuditProperty<TDomainObject, DateTime?>(datePropertyExpr, () => dateTimeService.Now))
        {
            if (userAuthenticationService == null) throw new ArgumentNullException(nameof(userAuthenticationService));
            if (dateTimeService == null) throw new ArgumentNullException(nameof(dateTimeService));
        }

        public AuditPropertyPair([NotNull] IAuditProperty<TDomainObject, string> authorAudit, [NotNull] IAuditProperty<TDomainObject, DateTime?> dateAudit)
        {
            if (authorAudit == null) throw new ArgumentNullException(nameof(authorAudit));
            if (dateAudit == null) throw new ArgumentNullException(nameof(dateAudit));

            this._authorAudit = authorAudit;
            this._dateAudit = dateAudit;
        }


        public IEnumerator<IAuditProperty> GetEnumerator()
        {
            yield return this._authorAudit;
            yield return this._dateAudit;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class AuditPropertyPair : AuditPropertyPair<IAuditObject>
    {
        public AuditPropertyPair(Expression<Func<IAuditObject, string>> authorPropertyExpr, Expression<Func<IAuditObject, DateTime?>> datePropertyExpr,
                                 [NotNull] IUserAuthenticationService userAuthenticationService,
                                 [NotNull] IDateTimeService dateTimeService)
            : base(authorPropertyExpr, datePropertyExpr, userAuthenticationService, dateTimeService)
        {
        }

        public AuditPropertyPair([NotNull] IAuditProperty<IAuditObject, string> authorAudit, [NotNull] IAuditProperty<IAuditObject, DateTime?> dateAudit)
            : base(authorAudit, dateAudit)
        {

        }


        public static AuditPropertyPair GetCreateAuditProperty(IUserAuthenticationService userAuthenticationService, IDateTimeService dateTimeService) => new AuditPropertyPair(obj => obj.CreatedBy, obj => obj.CreateDate, userAuthenticationService, dateTimeService);

        public static AuditPropertyPair GetModifyAuditProperty(IUserAuthenticationService userAuthenticationService, IDateTimeService dateTimeService) => new AuditPropertyPair(obj => obj.ModifiedBy, obj => obj.ModifyDate, userAuthenticationService, dateTimeService);
    }
}
