using System.Reflection;

namespace Framework.Core;

public static class ExceptionExtensions
{
    [Obsolete("v10 Use ex.GetBaseException()")]
    public static Exception GetLastInnerException(this TargetInvocationException ex)
    {
        if (ex == null)
        {
            throw new ArgumentNullException(nameof(ex));
        }

        return ex.GetAllElements(e => e.InnerException as TargetInvocationException)
                 .Last()
                 .InnerException;
    }
}
