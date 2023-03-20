using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;

public static class CodeExpressionHelper
{
    public static CodeTypeReference GetAnonymousCodeTypeReference()
    {
        return new CodeTypeReference(Constants.UnknownTypeName);
    }

    public static CodeTypeReferenceExpression BaseTypeReferenceExpression()
    {
        return new CodeTypeReferenceExpression("super");
    }

    public static CodeTypeReference GetVoidCodeTypeReference()
    {
        return new CodeTypeReference("void");
    }

    public static CodeTypeReference GetGuidCodeTypeReference()
    {
        return new CodeTypeReference(Constants.IdentityTypeName);
    }
}
