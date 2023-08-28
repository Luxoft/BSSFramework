using System.CodeDom;

using Framework.Core;

namespace Framework.CodeDom;

public static class CodeMemberMethodExtensions
{
    public static CodeMemberMethod WithTypeParameters(this CodeMemberMethod codeMemberMethod, IEnumerable<CodeTypeParameter> typeParameters)
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (typeParameters == null) throw new ArgumentNullException(nameof(typeParameters));

        codeMemberMethod.TypeParameters.AddRange(typeParameters.ToArray());

        return codeMemberMethod;
    }

    public static TMethod WithParameters<TMethod>(this TMethod codeMemberMethod, IEnumerable<CodeParameterDeclarationExpression> parameters)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        codeMemberMethod.Parameters.AddRange(parameters.ToArray());

        return codeMemberMethod;
    }

    public static TMethod WithStatements<TMethod>(this TMethod codeMemberMethod, IEnumerable<CodeStatement> statements)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (statements == null) throw new ArgumentNullException(nameof(statements));

        codeMemberMethod.Statements.AddRange(statements.ToArray());

        return codeMemberMethod;
    }

    public static TMethod WithStatement<TMethod>(this TMethod codeMemberMethod, CodeStatement statement)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (statement == null) throw new ArgumentNullException(nameof(statement));

        codeMemberMethod.Statements.Add(statement);

        return codeMemberMethod;
    }

    public static TMethod WithStatement<TMethod>(this TMethod codeMemberMethod, bool condition, Func<CodeStatement> getStatement)
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

    public static TMethod WithStatements<TMethod>(this TMethod codeMemberMethod, bool condition, Func<IEnumerable<CodeStatement>> getStatements)
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

    public static TMethod WithComment<TMethod>(this TMethod codeMemberMethod, string comment)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));

        if (!string.IsNullOrEmpty(comment))
        {
            codeMemberMethod.Comments.AddComments(new[] { comment });
        }

        return codeMemberMethod;
    }

    public static TMethod WithTryBreak<TMethod>(this TMethod codeMemberMethod)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));

        if (codeMemberMethod.Statements.Count == 0)
        {
            codeMemberMethod.Statements.Add(new CodeMethodYieldBreakStatement());
        }

        return codeMemberMethod;
    }


    public static TMethod WithYield<TMethod>(this TMethod codeMemberMethod, IEnumerable<CodeExpression> expressions, bool autoBreak = true)
            where TMethod : CodeMemberMethod
    {
        if (codeMemberMethod == null) throw new ArgumentNullException(nameof(codeMemberMethod));
        if (expressions == null) throw new ArgumentNullException(nameof(expressions));

        var statements = expressions.ToArray(expr => (CodeStatement)new CodeMethodYieldReturnStatement { Expression = expr });

        codeMemberMethod.Statements.AddRange(statements.AnyA() || !autoBreak ? statements : new[] { new CodeMethodYieldBreakStatement() });

        return codeMemberMethod;
    }
}
