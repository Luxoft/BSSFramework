using System.CodeDom;

namespace Framework.DomainDriven.Generation;

public interface IMethodGeneratorInfo
{
    string Name { get; }

    IEnumerable<CodeParameterDeclarationExpression> Parameters { get; }

    CodeTypeReference ReturnType { get; }
}


public static class MethodGeneratorInfoExtensions
{
    public static CodeParameterDeclarationExpression GetParameter(this IMethodGeneratorInfo info)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));

        return info.Parameters.Single();
    }
}
