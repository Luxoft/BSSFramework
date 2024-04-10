﻿using Framework.DomainDriven;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSecurityOperations))]
    public IEnumerable<string> GetSecurityOperations()
    {
        return this.EvaluateC(DBSessionMode.Read,
                              context => context.AvailableSecurityRoleSource
                                                .GetAvailableSecurityRole()
                                                .GetAwaiter()
                                                .GetResult()
                                                .SelectMany(sr => sr.Operations)
                                                .Distinct()
                                                .Select(op => op.Name)
                                                .ToList());
    }
}
