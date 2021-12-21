using System;
using System.CodeDom;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    internal static class CodeParameterDeclarationExpressionCollectionExtensions
    {
        public static CodeParameterDeclarationExpressionCollection WithoutTypes([NotNull] this CodeParameterDeclarationExpressionCollection source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }

            return new CodeParameterDeclarationExpressionCollection(source.OfType<CodeParameterDeclarationExpression>()
                                                                          .ToArray(p => new CodeParameterDeclarationExpression { Name = p.Name }));
        }
    }
}
