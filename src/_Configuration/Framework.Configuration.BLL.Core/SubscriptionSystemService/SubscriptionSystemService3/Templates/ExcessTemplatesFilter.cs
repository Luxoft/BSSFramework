using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Notification;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Templates
{
    /// <summary>
    /// Фильтр дублирующихся шаблонов уведомлений.
    /// Дублирующимися считаются шаблоны уведомлений с совпадающим значением свойства
    /// <see cref="MessageTemplateNotification.MessageTemplateCode"/>.
    /// </summary>
    /// <remarks>
    /// Класс реализует следующий алгоритм удаления дублирующихся шаблонов:
    /// <list type="number">
    ///     <item>Все шаблоны группируются по свойству <see cref="MessageTemplateNotification.MessageTemplateCode"/>;</item>
    ///     <item>Для каждой группы шаблонов выбирается одна наиболее релевантная подписка.</item>
    ///     <item>Для каждой группы шаблонов собирается полный список получателей To и полный список получателей Cc;</item>
    ///     <item>Если список получателей Cc не пустой, то создаётся один шаблон уведомления для всех получателей;</item>
    ///     <item>
    ///         Если список получателей Cc пустой, то в зависимости от флага SendIndividualLetters
    ///         найденной релевантной подписки, создается либо один шаблон для всех получателей
    ///         (SendIndividualLetters == false), либо отдельный шаблон для каждого получателя
    ///         (SendIndividualLetters == true);
    ///     </item>
    /// </list>
    /// </remarks>
    public class ExcessTemplatesFilter
    {
        /// <summary>Удаляет дублирующиеся шаблоны уведомлений.</summary>
        /// <param name="templates">Исходный список шаблонов уведомлений в котором могут содержаться дубликаты.</param>
        /// <returns>Список шаблонов уведомлений без дубликатов.</returns>
        /// <exception cref="ArgumentNullException">Аргумент templates равен null.</exception>
        public virtual IEnumerable<MessageTemplateNotification> FilterTemplates(
            [NotNull] IEnumerable<MessageTemplateNotification> templates)
        {
            if (templates == null)
            {
                throw new ArgumentNullException(nameof(templates));
            }

            var templateList = templates.ToList();

            var filterTo = new ExcessTemplatesFilterTo();
            var filterCc = new ExcessTemplatesFilterCc();

            var templatesTo = filterTo.ProcessTemplates(templateList);
            var templatesCc = filterCc.ProcessTemplates(templateList);

            var intermediateResult = templatesTo.Concat(templatesCc).ToList();
            var result = RemoveExcessCcRecipients(intermediateResult);

            return result;
        }

        private static IEnumerable<MessageTemplateNotification> RemoveExcessCcRecipients(
            IEnumerable<MessageTemplateNotification> templates)
        {
            var groups = ExcessTemplatesFilterBase.CollapseTemplates(templates);
            var result = groups.SelectMany(RemoveExcessCcRecipients);

            return result;
        }

        private static IEnumerable<MessageTemplateNotification> RemoveExcessCcRecipients(
            IGrouping<MessageTemplateNotification, MessageTemplateNotification> group)
        {
            var templatesTo = ExcessTemplatesFilterBase.GetTemplatesTo(group).ToList();
            var templatesCc = ExcessTemplatesFilterBase.GetTemplatesCc(group);
            var recieversTo = templatesTo.SelectMany(t => t.Receivers).ToList();

            var intermediateResult = templatesCc.Select(t => RemoveExcessCcRecipients(recieversTo, t));
            var result = templatesTo.Concat(intermediateResult);

            return result;
        }

        private static MessageTemplateNotification RemoveExcessCcRecipients(
            IEnumerable<string> recieversTo,
            MessageTemplateNotification template)
        {
            var recieversCc = template.CopyReceivers.Except(recieversTo);
            var result = ExcessTemplatesFilterBase.CopyTemplate(template, template.Receivers, recieversCc, template.ReplyTo);

            return result;
        }
    }
}
