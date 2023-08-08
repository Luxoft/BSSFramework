using System.Linq.Expressions;
using System.Reflection;
using Framework.Core;

namespace Framework.DomainDriven.BLL;

public class OverrideInstanceContainsIdentMethodVisitor<TIdent> : ExpressionVisitor
{
    private static readonly MethodInfo EnumerableContainsMethod = new Func<IEnumerable<TIdent>, TIdent, bool>(Enumerable.Contains).Method;


    private readonly MethodInfo containsMethod;


    public OverrideInstanceContainsIdentMethodVisitor(MethodInfo containsMethod)
    {
        if (containsMethod == null) throw new ArgumentNullException(nameof(containsMethod));

        this.containsMethod = containsMethod;
    }


    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var request = from obj in node.Object.ToMaybe()

                      where node.Method == this.containsMethod

                      from hashSet in obj.GetDeepMemberConstValue<HashSet<TIdent>>()

                      let list = hashSet.ToList()

                      let res = Expression.Call(null, EnumerableContainsMethod, new[] { Expression.Constant(list) }.Concat(node.Arguments))

                      select res;

        return request.GetValueOrDefault(() => base.VisitMethodCall(node));
    }



    public static readonly OverrideInstanceContainsIdentMethodVisitor<TIdent> CollectionInterface =

            new OverrideInstanceContainsIdentMethodVisitor<TIdent>(typeof(ICollection<TIdent>).GetMethod(CoreExpressionExtensions.GetMemberName((ICollection<TIdent> c) => c.Contains(default(TIdent))), true));


    public static readonly OverrideInstanceContainsIdentMethodVisitor<TIdent> HashSet =

            new OverrideInstanceContainsIdentMethodVisitor<TIdent>(new Func<TIdent, bool>(new HashSet<TIdent>().Contains).Method);
}
