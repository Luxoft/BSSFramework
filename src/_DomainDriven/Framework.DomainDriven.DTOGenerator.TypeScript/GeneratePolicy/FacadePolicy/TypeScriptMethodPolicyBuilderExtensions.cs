using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade
{
    public static class TypeScriptMethodPolicyBuilderExtensions
    {
        public static IEnumerable<string> GetFormattedMethods<T>(this TypeScriptMethodPolicyBuilder<T> builder)
        {
            return from method in builder.UsedMethods

                   orderby method.Name

                   let parameters = method.GetParametersWithExpandAutoRequest().Select(p => $"{p.ParameterType.ToCSharpFullName()} {p.Name}").Join(", ")

                   select $"{method.ReturnType.ToCSharpFullName()} {method.Name} ({parameters})";
        }
    }
}
