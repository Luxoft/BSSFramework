using System.CodeDom;
using System.Runtime.Serialization;

using Framework.CodeDom;

namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class GetCodeTypeReferenceExtensions
{
    public static CodeAttributeDeclaration ToKnownTypeCodeAttributeDeclaration(this CodeTypeReference codeTypeReference)
    {
        return new CodeAttributeDeclaration(
                                            typeof(KnownTypeAttribute).ToTypeReference(),
                                            new CodeAttributeArgument(codeTypeReference.ToTypeOfExpression()));
    }
}
