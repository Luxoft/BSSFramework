using System.CodeDom;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.CodeDom;

public static class CodeMemberMethodExtensions
{
    public static CodeMemberMethod WithTypeParameters([NotNull] this CodeMemberMethod codeMemberMethod, [NotNull] IEnumerable<CodeTypeParameter> typeParameters)
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (typeParameters == null) throw new ArgumentNullException(nameof(typeParameters));

        codeMemberMethod.TypeParameters.AddRange(typeParameters.ToArray());

        return codeMemberMethod;
    }

    public static TMethod WithParameters<TMethod>([NotNull] this TMethod codeMemberMethod, [NotNull] IEnumerable<CodeParameterDeclarationExpression> parameters)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        codeMemberMethod.Parameters.AddRange(parameters.ToArray());

        return codeMemberMethod;
    }

    public static TMethod WithStatements<TMethod>([NotNull] this TMethod codeMemberMethod, [NotNull] IEnumerable<CodeStatement> statements)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (statements == null) throw new ArgumentNullException(nameof(statements));

        codeMemberMethod.Statements.AddRange(statements.ToArray());

        return codeMemberMethod;
    }

    public static TMethod WithStatement<TMethod>([NotNull] this TMethod codeMemberMethod, [NotNull] CodeStatement statement)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (statement == null) throw new ArgumentNullException(nameof(statement));

        codeMemberMethod.Statements.Add(statement);

        return codeMemberMethod;
    }

    public static TMethod WithStatement<TMethod>([NotNull] this TMethod codeMemberMethod, bool condition, [NotNull] Func<CodeStatement> getStatement)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (getStatement == null) throw new ArgumentNullException(nameof(getStatement));

        if (condition)
        {
            codeMemberMethod.Statements.Add(getStatement());
        }

        return codeMemberMethod;
    }

    public static TMethod WithStatements<TMethod>([NotNull] this TMethod codeMemberMethod, bool condition, [NotNull] Func<IEnumerable<CodeStatement>> getStatements)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (getStatements == null) throw new ArgumentNullException(nameof(getStatements));

        if (condition)
        {
            codeMemberMethod.Statements.AddRange(getStatements().ToArray());
        }

        return codeMemberMethod;
    }

    public static TMethod WithComment<TMethod>([NotNull] this TMethod codeMemberMethod, string comment)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));

        if (!string.IsNullOrEmpty(comment))
        {
            codeMemberMethod.Comments.AddComments(new[] { comment });
        }

        return codeMemberMethod;
    }

    public static TMethod WithTryBreak<TMethod>([NotNull] this TMethod codeMemberMethod)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));

        if (codeMemberMethod.Statements.Count == 0)
        {
            codeMemberMethod.Statements.Add(new CodeMethodYieldBreakStatement());
        }

        return codeMemberMethod;
    }


    public static TMethod WithYield<TMethod>([NotNull] this TMethod codeMemberMethod, [NotNull] IEnumerable<CodeExpression> expressions, bool autoBreak = true)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (expressions == null) throw new ArgumentNullException(nameof(expressions));

        var statements = expressions.ToArray(expr => (CodeStatement)new CodeMethodYieldReturnStatement { Expression = expr });

        codeMemberMethod.Statements.AddRange(statements.AnyA() || !autoBreak ? statements : new[] { new CodeMethodYieldBreakStatement() });

        return codeMemberMethod;
    }
}
