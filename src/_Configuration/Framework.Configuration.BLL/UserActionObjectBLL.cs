using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Framework.Core;
using Framework.DomainDriven;
using Framework.Configuration.Domain;
using Framework.Configuration.Domain.Models.Filters;

namespace Framework.Configuration.BLL
{
    public partial class UserActionObjectBLL
    {
        public List<UserActionObject> GetListBy(UserActionObjectRootFilterModel filter, IFetchContainer<UserActionObject> fetchs)
        {
            var bll = this.Context.Logics.Default.Create<UserActionObject>();

            var precondition = this.GetPrecondition(filter);

            var queryable = bll.GetSecureQueryable(fetchs)
                .Where(precondition);

            queryable = this.AddPostConditionTo(queryable, filter);

            return queryable.ToList();
        }

        private Expression<Func<UserActionObject, bool>> GetPrecondition(UserActionObjectRootFilterModel filter)
        {
            var actionNames = filter.ActionNames;
            var filterDomainTypes = filter.DomainTypeNames;
            var count = filter.CountingEntities;
            var userLogin = this.Context.Authorization.CurrentPrincipalName;
            var period = filter.Period;

            Expression<Func<UserActionObject, bool>> condition = z => z.UserAction.UserName == userLogin;

            if (!period.IsEmpty)
            {
                condition = condition.BuildAnd(z => period.Contains(z.CreateDate.Value));
            }

            if (filterDomainTypes.Any())
            {
                condition = condition.BuildAnd(z => filterDomainTypes.Contains(z.UserAction.DomainType.Name));
            }

            if (actionNames.Any())
            {
                condition = condition.BuildAnd(z => actionNames.Contains(z.UserAction.Name));
            }

            return condition;
        }

        private IQueryable<UserActionObject> AddPostConditionTo(IQueryable<UserActionObject> queryable, UserActionObjectRootFilterModel filter)
        {
            var count = filter.CountingEntities;

            if (count > 0)
            {
                queryable = queryable.OrderByDescending(z => z.CreateDate).Take(count);
            }

            return queryable;
        }
    }
}
