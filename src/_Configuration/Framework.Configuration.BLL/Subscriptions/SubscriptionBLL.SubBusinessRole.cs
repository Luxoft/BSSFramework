using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Configuration.Domain;
using Framework.Exceptions;

namespace Framework.Configuration.BLL
{
    public partial class SubscriptionBLL
    {
        private IEnumerable<SubBusinessRole> GetNotSynchronizedSubBusinessRoles(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            return from subBusinessRole in subscription.SubBusinessRoles

                   let authRoleId = subBusinessRole.BusinessRoleId

                   where !this.Context.Authorization.Logics.BusinessRole.GetUnsecureQueryable().Any(authRole => authRole.Id == authRoleId)

                   select subBusinessRole;
        }

        private BusinessLogicException GetValidateBusunessRoleResult(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            var notSynchronizedSubBusinessRoles = this.GetNotSynchronizedSubBusinessRoles(subscription).ToList();

            if (notSynchronizedSubBusinessRoles.Any())
            {
                var errorMessage =
                    $"Subscription with code {subscription.Code} has not synchronized subBusinessRoles: {notSynchronizedSubBusinessRoles.Join(",", subBusinessRole => $"{subBusinessRole.Name} ({subBusinessRole.BusinessRoleId})")}";

                return new BusinessLogicException(errorMessage);
            }
            else
            {
                return null;
            }
        }


        public void ValidateAllBusunessRoles()
        {
            var errors = this.GetFullList().Select(this.GetValidateBusunessRoleResult).Where(res => res != null).ToList();

            if (errors.Any())
            {
                throw new BusinessLogicAggregateException(errors);
            }
        }

        public void ValidateBusunessRole(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            this.GetValidateBusunessRoleResult(subscription).Maybe(s => { throw s; });
        }


        public void SynchronizeAllBusunessRoles(bool strong)
        {
            this.GetFullList().Foreach(s => this.SynchronizeBusunessRole(s, strong));
        }

        public void SynchronizeBusunessRole(Subscription subscription, bool strong)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            var notSynchronizedSubBusinessRolesRequest =

                from notSynchronizedSubBusinessRole in this.GetNotSynchronizedSubBusinessRoles(subscription)

                let authBusinessRole = this.Context.Authorization.Logics.BusinessRole.GetByName(notSynchronizedSubBusinessRole.Name, strong)

                where authBusinessRole != null

                select new
                {
                    NotSynchronizedSubBusinessRole = notSynchronizedSubBusinessRole,
                    AuthRoleId = authBusinessRole.Id
                };

            var notSynchronizedSubBusinessRoles = notSynchronizedSubBusinessRolesRequest.ToList();

            if (notSynchronizedSubBusinessRoles.Any())
            {
                foreach (var notSynchronizedSubBusinessRolePair in notSynchronizedSubBusinessRoles)
                {
                    notSynchronizedSubBusinessRolePair.NotSynchronizedSubBusinessRole.BusinessRoleId = notSynchronizedSubBusinessRolePair.AuthRoleId;
                }

                base.Save(subscription, false);
            }
        }


        private void InitBusunessRoleNames(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            subscription.SubBusinessRoles.Foreach(this.InitSubBusinessRoleName);
        }

        private void InitSubBusinessRoleName(SubBusinessRole subBusinessRole)
        {
            if (subBusinessRole == null) throw new ArgumentNullException(nameof(subBusinessRole));

            var authBusinessRole = this.Context.Authorization.Logics.BusinessRole.GetById(subBusinessRole.BusinessRoleId, true);

            subBusinessRole.Name = authBusinessRole.Name;
        }
    }
}
