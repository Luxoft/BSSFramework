using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL;

namespace Authorization.WebApi.Controllers
{
    public partial class OperationController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSecurityOperations))]
        public IEnumerable<string> GetSecurityOperations()
        {
            return this.EvaluateC(DBSessionMode.Read, context => context.Logics.Operation.GetAvailableOperationCodes().ToList());
        }
    }
}
