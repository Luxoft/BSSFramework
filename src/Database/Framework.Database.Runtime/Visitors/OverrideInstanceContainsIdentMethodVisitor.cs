using System.Linq.Expressions;
using System.Reflection;

using Anch.Core;

using Framework.Core;

namespace Framework.Database.Visitors;

public class OverrideInstanceContainsIdentMethodVisitor<TIdent>(MethodInfo containsMethod) : ExpressionVisitor
{
    private static readonly MethodInfo EnumerableContainsMethod = new Func<IEnumerable<TIdent>, TIdent, bool>(Enumerable.Contains).Method;


    private readonly MethodInfo containsMethod = containsMethod ?? throw new ArgumentNullException(nameof(containsMethod));

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var request = from obj in node.Object.ToMaybe()

                      where node.Method == this.containsMethod

                      from hashSet in obj.GetConstantValue<HashSet<TIdent>>()

                      let list = hashSet.ToList()

                      let res = Expression.Call(null, EnumerableContainsMethod, new[] { Expression.Constant(list) }.Concat(node.Arguments))

                      select res;

        return request.GetValueOrDefault(() => base.VisitMethodCall(node));
    }



    public static readonly OverrideInstanceContainsIdentMethodVisitor<TIdent> CollectionInterface =

            new(typeof(ICollection<TIdent>).GetMethod(CoreExpressionExtensions.GetMemberName((ICollection<TIdent> c) => c.Contains(default(TIdent)!)), true)!);


    public static readonly OverrideInstanceContainsIdentMethodVisitor<TIdent> HashSet = new(new Func<TIdent, bool>(new HashSet<TIdent>().Contains).Method);
}

