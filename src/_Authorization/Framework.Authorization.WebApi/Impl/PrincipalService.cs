using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;

namespace Framework.Authorization.WebApi
{
    public partial class AuthSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetCurrentPrincipal))]
        public PrincipalFullDTO GetCurrentPrincipal()
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
                evaluateData.Context.Logics.Principal.GetCurrent().ToFullDTO(evaluateData.MappingService));
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetVisualPrincipalsWithoutSecurity))]
        public IEnumerable<PrincipalVisualDTO> GetVisualPrincipalsWithoutSecurity()
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
                evaluateData.Context.Logics.Permission.GetAvailablePermissionsQueryable().Any()
                     ? evaluateData.Context.Logics.Principal.GetFullList().ToVisualDTOList(evaluateData.MappingService)
                     : Enumerable.Empty<PrincipalVisualDTO>());
        }
    }
}
