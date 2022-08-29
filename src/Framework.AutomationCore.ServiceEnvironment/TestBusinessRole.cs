using System;
using System.Collections.Generic;

using Automation.Utils;

namespace Automation.ServiceEnvironment;

public class TestBusinessRole : IPermissionDefinition
{
    private readonly string name;

    public TestBusinessRole(string name)
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

    public static readonly TestBusinessRole Administrator = new(nameof(Administrator));

    public static readonly TestBusinessRole SystemIntegration = new(nameof(SystemIntegration));
}
