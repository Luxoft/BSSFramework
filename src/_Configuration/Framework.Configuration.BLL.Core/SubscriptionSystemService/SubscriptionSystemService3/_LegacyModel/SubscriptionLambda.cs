using System;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Функция определенного вида, используемая в подписках в качестве: условия выполнения подписки, получения её адресатов или контекстов ролей
    /// </summary>
    public class SubscriptionLambda
    {
        private Func<object, object, object> funcValue;
        
        private Type authDomainType;
        
        private Type metadataSourceType;
        
        /// <summary>Получает делегат, исполняющий лямбду.</summary>
        /// <value>Делегат, исполняющий лямбду.</value>
        public virtual Func<object, object, object> FuncValue
        {
            get { return this.funcValue; }
            set { this.funcValue = value; }
        }

        public virtual Type AuthDomainType
        {
            get { return this.authDomainType; }
            set { this.authDomainType = value; }
        }

        /// <summary>
        /// Задаёт или возвращает тип исходной модели лямбды.
        /// </summary>
        /// <value>
        /// Тип исходной модели лямбы.
        /// </value>
        public virtual Type MetadataSourceType
        {
            get { return this.metadataSourceType; }
            set { this.metadataSourceType = value; }
        }
        public virtual bool? RequiredModePrev { get; set; }

        /// <summary>
        /// Признак обязательности текущего состояния элемента в лямбде
        /// </summary>
        /// <remarks>
        /// Чтобы не задавать условие на существование состояния типа prev == null && cur != null, можно указать Required Mode
        /// Отображение на интерфейсе:
        /// Если признак "True", то текущее значение обязательно (not null)
        /// Если признак "False", то текущее значение должно быть пустым (null)
        /// Если признак "Null", то допустим любой из текущих вариантов (uknown)
        /// </remarks>
        public virtual bool? RequiredModeNext { get; set; }

        public virtual SubscriptionType ProcessType
        {
            get { return SubscriptionTypeHelper.GetSubscriptionType(this.RequiredModePrev, this.RequiredModeNext); }
        }
    }
}
