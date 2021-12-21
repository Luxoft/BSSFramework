using System;
using System.Reflection;

using NHibernate.Event;
using NHibernate.Impl;

namespace Framework.DomainDriven.NHibernate
{
    internal static class NhibSessionImplExtensions
    {
        private const string ListenersFieldName = "listeners";

        private static readonly FieldInfo ListenersField = typeof(SessionImpl).GetField(ListenersFieldName, BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Специальный костыль для NHibernate по переопределению Listeners
        /// Применяется для работы с несколькими базами данных.
        /// Обычное изменение Listeners не подходит, так как это повлечет изменение Listeners в SessionFactory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="listeners"></param>
        internal static void OverrideListeners(this SessionImpl source, EventListeners listeners)
        {
            if (ListenersField == null)
            {
                throw new ArgumentNullException($"{typeof(SessionImpl).Name} has no expected private field '{ListenersFieldName}'");
            }

            ListenersField.SetValue(source, listeners);
        }
    }
}
