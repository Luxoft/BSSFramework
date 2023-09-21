using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class BusinessRoleOperationLinkMapping : AuthBaseMap<BusinessRoleOperationLink>
    {
        public BusinessRoleOperationLinkMapping()
        {
            this.Map(x => x.IsDenormalized);
            this.References(x => x.BusinessRole)
                .Column($"{nameof(BusinessRoleOperationLink.BusinessRole)}Id")
                .UniqueKey("UIX_businessRole_operationBusinessRoleOperationLink");
            this.References(x => x.Operation)
                .Column($"{nameof(BusinessRoleOperationLink.Operation)}Id")
                .UniqueKey("UIX_businessRole_operationBusinessRoleOperationLink");
        }
    }
}
