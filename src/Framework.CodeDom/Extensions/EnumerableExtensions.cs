using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;

using CommonFramework;

using Framework.Core;

namespace Framework.CodeDom;

public static class EnumerableExtensions
{
    internal static void CopyUserDataFrom(this CodeObject targetCodeObject, CodeObject sourceCodeObject)
    {
        if (targetCodeObject == null) throw new ArgumentNullException(nameof(targetCodeObject));
        if (sourceCodeObject == null) throw new ArgumentNullException(nameof(sourceCodeObject));

        targetCodeObject.UserData.CopyFrom(sourceCodeObject.UserData);
    }

    internal static T WithCopyUserDataFrom<T>(this T targetCodeObject, T sourceCodeObject)
            where T : CodeObject
    {
        if (targetCodeObject == null) throw new ArgumentNullException(nameof(targetCodeObject));
        if (sourceCodeObject == null) throw new ArgumentNullException(nameof(sourceCodeObject));

        targetCodeObject.UserData.CopyFrom(sourceCodeObject.UserData);

        return targetCodeObject;
    }



    internal static void CopyFrom(this IDictionary targetDictionary, IDictionary sourceDictionary)
    {
        if (targetDictionary == null) throw new ArgumentNullException(nameof(targetDictionary));
        if (sourceDictionary == null) throw new ArgumentNullException(nameof(sourceDictionary));

        foreach (DictionaryEntry entry in sourceDictionary)
        {
            targetDictionary[entry.Key] = entry.Value;
        }
    }

    public static void AddComments(this CodeCommentStatementCollection collection, IEnumerable<string> comments, bool docComment = true, bool summary = true)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (comments == null) throw new ArgumentNullException(nameof(comments));

        var cacheComments = comments.ToList();

        if (cacheComments.Any())
        {
            if (summary)
            {
                collection.Add(new CodeCommentStatement("<summary>", docComment));
            }

            cacheComments.Foreach(comment =>
                                  {
                                      collection.Add(new CodeCommentStatement(comment, docComment));
                                  });

            if (summary)
            {
                collection.Add(new CodeCommentStatement("</summary>", docComment));
            }
        }
    }

    public static CodeParameterDeclarationExpression[] ToArrayExceptNull(this CodeParameterDeclarationExpressionCollection source, Func<CodeParameterDeclarationExpression, CodeParameterDeclarationExpression> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeParameterDeclarationExpression>(selector);
    }

    public static CodeExpression[] ToArrayExceptNull(this CodeExpressionCollection source, Func<CodeExpression, CodeExpression> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeExpression>(selector);
    }

    public static CodeStatement[] ToArrayExceptNull(this CodeStatementCollection source, Func<CodeStatement, CodeStatement> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeStatement>(selector);
    }

    public static CodeTypeParameter[] ToArrayExceptNull(this CodeTypeParameterCollection source, Func<CodeTypeParameter, CodeTypeParameter> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeTypeParameter>(selector);
    }

    public static CodeTypeReference[] ToArrayExceptNull(this CodeTypeReferenceCollection source, Func<CodeTypeReference, CodeTypeReference> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeTypeReference>(selector);
    }

    public static CodeCommentStatement[] ToArrayExceptNull(this CodeCommentStatementCollection source, Func<CodeCommentStatement, CodeCommentStatement> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeCommentStatement>(selector);
    }

    public static CodeAttributeDeclaration[] ToArrayExceptNull(this CodeAttributeDeclarationCollection source, Func<CodeAttributeDeclaration, CodeAttributeDeclaration> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeAttributeDeclaration>(selector);
    }

    public static CodeNamespace[] ToArrayExceptNull(this CodeNamespaceCollection source, Func<CodeNamespace, CodeNamespace> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<CodeNamespace>(selector);
    }

    public static string[] ToArrayExceptNull(this StringCollection source, Func<string, string> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToArrayExceptNull<string>(selector);
    }


    public static IEnumerable<T> ToCache<T>(this IEnumerable<T> source)
    {
        return LazyHelper.Create<IEnumerable<T>>(source.ToList).Unwrap();
    }

    public static Lazy<TCollection> ToLazyCache<T, TCollection>(this IEnumerable<T> source, Func<IEnumerable<T>, TCollection> createFunc)
            where TCollection : IEnumerable<T>
    {
        return LazyHelper.Create<TCollection>(() => createFunc(source));
    }



    private static T[] ToArrayExceptNull<T>(this IEnumerable source, Func<T, T> selector)
            where T : class
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Cast<T>().ToArrayExceptNull(selector);
    }



    public static TResult[] ToArrayExceptNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
            where TResult : class
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(selector).ToArrayExceptNull();
    }

    private static T[] ToArrayExceptNull<T>(this IEnumerable<T> source)
            where T : class
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Where(v => v != null).ToArray();
    }
}
