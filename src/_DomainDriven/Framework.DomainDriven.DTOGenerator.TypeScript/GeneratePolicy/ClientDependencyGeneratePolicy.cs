using System;
using System.Collections.Generic;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript;

[Obsolete("Use TypeScriptDependencyGeneratePolicy", true)]
public class ClientDependencyGeneratePolicy : DependencyGeneratePolicy
{
    public ClientDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base(baseGeneratePolicy, maps)
    {
    }
}
