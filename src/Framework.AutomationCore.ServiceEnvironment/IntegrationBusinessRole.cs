using System;
using System.Collections.Generic;

using Automation.Utils;

namespace Automation.ServiceEnvironment;

public class IntegrationBusinessRole : IPermissionDefinition
{
    private readonly string name;

    public IntegrationBusinessRole(string name)
    {
        this.name = name;
    }

    IEnumerable<Tuple<string, Guid>> IPermissionDefinition.GetEntities()
    {
        yield break;
    }

    public string GetRoleName()
    {
        return this.name;
    }

    public override string ToString()
    {
        return this.name;
    }

    public static readonly IntegrationBusinessRole Administrator = new(nameof(Administrator));

    public static readonly IntegrationBusinessRole SystemIntegration = new(nameof(SystemIntegration));
}
