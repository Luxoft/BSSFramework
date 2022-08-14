using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.DomainDriven;

namespace Framework.Authorization.WebApi
{
    public partial class AuthSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSecurityOperations))]
        public IEnumerable<string> GetSecurityOperations()
        {
            return this.EvaluateC(DBSessionMode.Read, context => context.Logics.Operation.GetAvailableOperationCodes().ToList());
        }
    }
}
