using Framework.DomainDriven;
using Framework.Configuration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Framework.Core;
using Framework.Validation;
using Framework.Configuration.Domain.Models.Filters;
using Framework.Configuration.Domain.Models.Create;
using System.Collections.ObjectModel;

namespace Framework.Configuration.BLL
{
    public partial class UserActionBLL
    {
        public UserAction Create(UserActionCreateModel createModel)
        {
            var domainTypeCache = this.GetDomainTypesByName(createModel.DomainType);

            this.Validation(createModel, domainTypeCache);

            var createdUserAction = this.CreateDomainObjectBy(createModel, domainTypeCache.First());

            this.Save(createdUserAction);

            return createdUserAction;
        }

        private void Validation(UserActionCreateModel createModel, ReadOnlyCollection<DomainType> domainTypeCache)
        {
            if (domainTypeCache.Count == 0)
            {
                throw new ValidationException("DomainType '{0}' is not registered in the system", createModel.DomainType);
            }

            if (domainTypeCache.Count > 1)
            {
                throw new ValidationException("Found more than one match to the DomainType '{0}'", createModel.DomainType);
            }
        }

        private UserAction CreateDomainObjectBy(UserActionCreateModel createModel, DomainType domainType)
        {
            var userAction = new UserAction();

            this.MapUserAction(userAction, createModel, domainType);

            this.MapUserActionObjects(userAction, createModel);

            return userAction;
        }

        private void MapUserAction(UserAction userAction, UserActionCreateModel createModel, DomainType domainType)
        {
            userAction.DomainType = domainType;
            userAction.Name = createModel.Name;
            userAction.UserName = this.Context.Authorization.CurrentPrincipalName;
        }

        private void MapUserActionObjects(UserAction userAction, UserActionCreateModel createModel)
        {
            foreach (var objectIdentity in createModel.ObjectIdentities)
            {
                var userActionObject = userAction.CreateDetail();
                userActionObject.ObjectIdentity = objectIdentity.ObjectIdentity;
                userActionObject.Name = objectIdentity.Name;
            }
        }

        private ReadOnlyCollection<DomainType> GetDomainTypesByName(string domainTypeName)
        {
            return this.Context.Logics.DomainType.GetUnsecureQueryable().Where(z => z.Name == domainTypeName).ToReadOnlyCollection();
        }
    }
}