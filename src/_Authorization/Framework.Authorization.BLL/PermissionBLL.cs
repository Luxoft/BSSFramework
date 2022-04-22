using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL
{
    public partial class PermissionBLL
    {
        public new void Save(Permission permission, bool withValidate)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            base.Save(permission, withValidate);
        }

        public override void Save(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            if (this.Context.CurrentPrincipal.RunAs != null)
            {
                throw new BusinessLogicException("RunAs mode must be disabled");
            }

            base.Save(permission);

            permission.DelegatedTo.Foreach(delegatedPermission => this.Context.Logics.Permission.Save(delegatedPermission, false));
        }

        protected override void PreRecalculate(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            permission.IsDelegatedTo = permission.DelegatedTo.Any();

            this.RecalculateDenormalizedItems(permission);

            base.PreRecalculate(permission);
        }


        private void RecalculateDenormalizedItems([NotNull] Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var expectedItems = from entityType in this.Context.Logics.EntityType.GetFullList()

                                join filterItem in permission.FilterItems on entityType equals filterItem.EntityType into filterItemGroup

                                from accessId in GetAccessIdents(filterItemGroup.ToArray(fi => fi.Entity.EntityId))

                                select new { EntityType = entityType, EntityId = accessId };

            permission.DenormalizedItems.Merge(expectedItems, pair => pair, di => new { di.EntityType, di.EntityId }, pair => new DenormalizedPermissionItem(permission, pair.EntityType, pair.EntityId), permission.RemoveDetails);
        }

        private static IEnumerable<Guid> GetAccessIdents(Guid[] baseIdents)
        {
            foreach (var baseIdent in baseIdents)
            {
                yield return baseIdent;
            }

            if (!baseIdents.Any())
            {
                yield return DenormalizedPermissionItem.GrandAccessGuid;
            }

            yield return DenormalizedPermissionItem.LowestAccessGuid;
        }


        protected override void PostValidate(Permission permission, AuthorizationOperationContext operationContext)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            this.ValidatePermissionDelegated(permission, ValidatePermissonDelegateMode.All);

            this.ValidateApprovingPermission(permission);
        }

        public void ValidatePermissionDelegated(Permission permission, ValidatePermissonDelegateMode mode)
        {
            if (permission.IsDelegatedFrom)
            {
                this.ValidatePermissionDelegatedFrom(permission, mode);
            }

            this.ValidatePermissionDelegatedTo(permission, mode);
        }

        public void ValidateApprovingPermission(Permission permission)
        {
            if (!permission.IsNew && permission.Role.RequiredApprove && this.Context.TrackingService.GetChanges(permission).HasChange(p => p.FilterItems, p => p.Period))
            {
                var prevVersion = this.GetObjectsByPrevRevision(permission.Id);

                if (!this.IsCorrentPeriodSubset(permission, prevVersion))
                {
                    throw new ValidationException(
                        $"Permission with Approving Role can't be changed, because selected period \"{permission.Period}\" not subset of \"{prevVersion.Period}\"");
                }

                if (this.Context.DateTimeService.IsActivePeriod(permission))
                {
                    var invalidEntityGroups = this.GetInvalidDelegatedPermissionSecurities(permission, prevVersion).ToList();

                    if (invalidEntityGroups.Any())
                    {
                        throw new ValidationException(
                            $"Permission with Approving Role can't be changed, because permission have no access to new object subset ({invalidEntityGroups.Join(" | ", g => $"{g.Key.Name}: {g.Value.Join(", ", s => s.Name)}")})");
                    }
                }
            }
        }

        private void ValidatePermissionDelegatedTo(Permission permission, ValidatePermissonDelegateMode mode)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            foreach (var subPermission in permission.DelegatedTo)
            {
                this.ValidatePermissionDelegatedFrom(subPermission, mode);
            }
        }

        private void ValidatePermissionDelegatedFrom(Permission permission, ValidatePermissonDelegateMode mode)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));
            if (permission.DelegatedFrom == null) { throw new System.ArgumentException("is not delegated permission"); }

            if (mode.HasFlag(ValidatePermissonDelegateMode.ApproveState))
            {
                if (permission.DelegatedFrom.Status != PermissionStatus.Approved)
                {
                    throw new ValidationException(
                        $"Invalid delegated permission period. Owner Permission must have status \"{PermissionStatus.Approved}\"");
                }
            }

            if (mode.HasFlag(ValidatePermissonDelegateMode.Role))
            {
                if (!permission.DelegatedFrom.Role.GetGraphElements(r => r.SubBusinessRoles).Contains(permission.Role))
                {
                    throw new ValidationException(
                        $"Invalid delegated permission role. Selected role \"{permission.Role.Name}\" not derived from role \"{permission.DelegatedFrom.Role.Name}\"");
                }
            }

            if (mode.HasFlag(ValidatePermissonDelegateMode.Period))
            {
                if (!this.IsCorrentPeriodSubset(permission))
                {
                    throw new ValidationException(
                        $"Invalid delegated permission period. Selected period \"{permission.Period}\" not subset of \"{permission.DelegatedFrom.Period}\"");
                }
            }

            if (mode.HasFlag(ValidatePermissonDelegateMode.SecurityObjects) && this.Context.DateTimeService.IsActivePeriod(permission))
            {
                var invalidEntityGroups = this.GetInvalidDelegatedPermissionSecurities(permission).ToList();

                if (invalidEntityGroups.Any())
                {
                    throw new ValidationException(
                        string.Format(
                            "Can't delegate permission from {0} to {1}, because {0} have no access to objects ({2})",
                            permission.DelegatedFromPrincipal.Name,
                            permission.Principal.Name,
                            invalidEntityGroups.Join(
                                " | ",
                                g => $"{g.Key.Name}: {g.Value.Join(", ", s => s.Name)}")));
                }
            }
        }

        private bool IsCorrentPeriodSubset([NotNull] Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var delegatedFromPermission = permission.DelegatedFrom.FromMaybe(() => new ValidationException("Pemission not delegated"));

            return this.IsCorrentPeriodSubset(permission, delegatedFromPermission);
        }

        private bool IsCorrentPeriodSubset([NotNull] Permission subPermission, [NotNull] Permission parentPermission)
        {
            if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
            if (parentPermission == null) throw new ArgumentNullException(nameof(parentPermission));

            return subPermission.Period.IsEmpty || parentPermission.Period.Contains(subPermission.Period);
        }

        private Dictionary<EntityType, IEnumerable<SecurityEntity>> GetInvalidDelegatedPermissionSecurities(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var delegatedFromPermission = permission.DelegatedFrom.FromMaybe(() => new ValidationException("Pemission not delegated"));

            return this.GetInvalidDelegatedPermissionSecurities(permission, delegatedFromPermission);
        }

        private Dictionary<EntityType, IEnumerable<SecurityEntity>> GetInvalidDelegatedPermissionSecurities(Permission subPermission, [NotNull] Permission parentPermission)
        {
            if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
            if (parentPermission == null) throw new ArgumentNullException(nameof(parentPermission));

            var allowedEntitiesRequest = from filterItem in parentPermission.FilterItems

                                         group filterItem.Entity.EntityId by filterItem.Entity.EntityType;

            var allowedEntitiesDict = allowedEntitiesRequest.ToDictionary(g => g.Key, g => g.ToList());

            var requaredEntitiesRequest = (from filterItem in subPermission.FilterItems

                                           group filterItem.Entity.EntityId by filterItem.Entity.EntityType).ToArray();

            var invalidRequest1 = from requeredGroup in requaredEntitiesRequest

                                  let allSecurityEntities = this.Context.ExternalSource.GetTyped(requeredGroup.Key).GetSecurityEntities()

                                  let entityType = requeredGroup.Key

                                  let preAllowedEntities = allowedEntitiesDict.GetValueOrDefault(entityType).Maybe(v => v.Distinct())

                                  where preAllowedEntities != null // доступны все

                                  let allowedEntities = entityType.Expandable ? preAllowedEntities.Distinct()
                                                                                                  .GetAllElements(allowedEntityId => allSecurityEntities.Where(v => v.ParentId == allowedEntityId).Select(v => v.Id))
                                                                                                  .Distinct()
                                                                                                  .ToList()

                                                                              : preAllowedEntities.Distinct().ToList()

                                  from entityId in requeredGroup

                                  let securityObject = allSecurityEntities.SingleOrDefault(v => v.Id == entityId)

                                  where securityObject != null // Протухшая безопасность

                                  let hasAccess = allowedEntities.Contains(entityId)

                                  where !hasAccess

                                  group securityObject by entityType into g

                                  let key = g.Key

                                  let value = (IEnumerable<SecurityEntity>)g

                                  select key.ToKeyValuePair(value);

            var invalidRequest2 = from entityType in allowedEntitiesDict.Keys

                                  join requeredGroup in requaredEntitiesRequest on entityType equals requeredGroup.Key into g

                                  where !g.Any()

                                  let key = entityType

                                  let value = (IEnumerable<SecurityEntity>)new[] { new SecurityEntity { Name = "[Not Selected Element]" } }

                                  select key.ToKeyValuePair(value);

            return invalidRequest1.Concat(invalidRequest2).ToDictionary();
        }

        public void ChangeDelegatePermissions(ChangePermissionDelegatesModel changePermissionDelegatesModel)
        {
            if (changePermissionDelegatesModel == null) throw new ArgumentNullException(nameof(changePermissionDelegatesModel));

            this.Context.Validator.Validate(changePermissionDelegatesModel);

            var delegatedFromPermission = changePermissionDelegatesModel.DelegateFromPermission;

            if (delegatedFromPermission.Status != PermissionStatus.Approved)
            {
                throw new BusinessLogicException($"Source permission ({delegatedFromPermission.Principal.Name}) must be {PermissionStatus.Approved}");
            }

            var currentDelegatedToPermissions = delegatedFromPermission.DelegatedTo.ToList();

            var expectedDelegatedToPermissions = changePermissionDelegatesModel.Items.ToList(item => item.Permission);

            var removingItems = currentDelegatedToPermissions.GetMergeResult(expectedDelegatedToPermissions).RemovingItems;

            delegatedFromPermission.RemoveDetails(removingItems);

            this.Save(delegatedFromPermission);
        }

        public void UpdateDelegatePermissions(UpdatePermissionDelegatesModel updatePermissionDelegatesModel)
        {
            if (updatePermissionDelegatesModel == null) throw new ArgumentNullException(nameof(updatePermissionDelegatesModel));

            this.Context.Validator.Validate(updatePermissionDelegatesModel);

            var changePermissionDelegatesModel = new ChangePermissionDelegatesModel
            {
                DelegateFromPermission = updatePermissionDelegatesModel.DelegateFromPermission,

                Items = updatePermissionDelegatesModel.DelegateFromPermission.DelegatedTo.ToList(subPerm =>

                    new DelegateToItemModel { Permission = subPerm, Principal = subPerm.Principal })
            };

            changePermissionDelegatesModel.Merge(updatePermissionDelegatesModel);

            this.ChangeDelegatePermissions(changePermissionDelegatesModel);
        }

        public void WithdrawDelegation(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var newPeriod = permission.Period.StartDate.ToPeriod(this.Context.DateTimeService.Today.SubtractDay());

            permission.GetAllChildren().Foreach(p => p.Period = newPeriod);

            this.Save(permission);
        }

        public IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true)
        {
            return this.GetAvailablePermissionsQueryable(new AvailablePermissionFilter(
                this.Context.DateTimeService,
                withRunAs ? this.Context.RunAsManager.PrincipalName : this.Context.CurrentPrincipalName));
        }

        public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
        {
            return this.GetSecureQueryable().Where(filter.ToFilterExpression());
        }
    }
    public static class DateTimeServiceExtensions
    {
        public static bool IsActivePeriod(this IDateTimeService dateTimeService, [NotNull] IPeriodObject periodObject)
        {
            if (periodObject == null) throw new ArgumentNullException(nameof(periodObject));
            if (dateTimeService == null) throw new ArgumentNullException(nameof(dateTimeService));

            return periodObject.Period.Contains(dateTimeService.Today);
        }
    }
}
