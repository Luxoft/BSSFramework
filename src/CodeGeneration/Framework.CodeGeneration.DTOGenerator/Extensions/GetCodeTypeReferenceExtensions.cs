using System.CodeDom;
using System.Runtime.Serialization;

using Framework.CodeDom.Extensions;

namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class GetCodeTypeReferenceExtensions
{
    public static CodeAttributeDeclaration ToKnownTypeCodeAttributeDeclaration(this CodeTypeReference codeTypeReference) =>
        new(
            typeof(KnownTypeAttribute).ToTypeReference(),
            new CodeAttributeArgument(codeTypeReference.ToTypeOfExpression()));
}
