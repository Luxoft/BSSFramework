using System.Reflection;

using Anch.Core;

using NHibernate;
using NHibernate.Event;
using NHibernate.Impl;

namespace Framework.Database.NHibernate.Sessions;

internal static class NhibSessionImplExtensions
{
    private static readonly FieldInfo ListenersField = typeof(SessionImpl).GetField("listeners", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly Action<SessionImpl, IInterceptor> SetInterceptorAction = ExpressionHelper.Create((SessionImpl s) => s.Interceptor).GetProperty().GetSetValueAction<SessionImpl, IInterceptor>();

    /// <summary>
    /// Special hack for NHibernate to override Listeners
    /// Used for working with several databases.
    /// A regular change of Listeners is not suitable because it would change Listeners in SessionConfiguration
    /// </summary>
    /// <param name="source"></param>
    /// <param name="listeners"></param>
    internal static void OverrideListeners(this SessionImpl source, EventListeners listeners) => ListenersField.SetValue(source, listeners);

    internal static void OverrideInterceptor(this SessionImpl source, IInterceptor interceptor) => SetInterceptorAction(source, interceptor);
}
