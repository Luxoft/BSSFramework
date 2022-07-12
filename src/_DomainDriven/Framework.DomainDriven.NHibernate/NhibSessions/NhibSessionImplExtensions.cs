using System;
using System.Reflection;

using Framework.Core;

using NHibernate;
using NHibernate.Event;
using NHibernate.Impl;

namespace Framework.DomainDriven.NHibernate
{
    internal static class NhibSessionImplExtensions
    {
        private static readonly Action<SessionImpl, EventListeners> SetListenersAction = ExpressionHelper.Create((SessionImpl s) => s.Listeners).GetProperty().GetPrivateField().GetSetValueAction<SessionImpl, EventListeners>();

        private static readonly Action<SessionImpl, IInterceptor> SetInterceptorAction = ExpressionHelper.Create((SessionImpl s) => s.Interceptor).GetProperty().GetSetValueAction<SessionImpl, IInterceptor>();

        /// <summary>
        /// Специальный костыль для NHibernate по переопределению Listeners
        /// Применяется для работы с несколькими базами данных.
        /// Обычное изменение Listeners не подходит, так как это повлечет изменение Listeners в SessionConfiguration
        /// </summary>
        /// <param name="source"></param>
        /// <param name="listeners"></param>
        internal static void OverrideListeners(this SessionImpl source, EventListeners listeners)
        {
            SetListenersAction(source, listeners);
        }

        internal static void OverrideInterceptor(this SessionImpl source, IInterceptor listeners)
        {
            SetInterceptorAction(source, listeners);
        }
    }
}
