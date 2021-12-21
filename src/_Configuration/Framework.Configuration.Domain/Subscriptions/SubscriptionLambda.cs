using System;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Функция определенного вида, используемая в подписках в качестве: условия выполнения подписки, получения её адресатов или контекстов ролей
    /// </summary>
    [UniqueGroup]
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.SubscriptionView)]
    [ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.SubscriptionEdit)]
    [NotAuditedClass]
    public partial class SubscriptionLambda : BaseDirectory,
        ILambdaObject,
        ISubscriptionLambdaHeader
    {
        private DomainType domainType;

        private string value;

        private SubscriptionLambdaType type;

        private bool withContext;

        private Guid authDomainTypeId;



        private bool? requiredModePrev;

        private bool? requiredModeNext;



        private string formattedType;

        [NotPersistentField]
        private Func<object, object, object> funcValue;

        [NotPersistentField]
        private Type authDomainType;

        [NotPersistentField]
        private Type metadataSourceType;

        /// <summary>
        /// Конструктор
        /// </summary>
        public SubscriptionLambda()
        {

        }

        /// <summary>
        /// Целевая система, в которой применяется лямбда
        /// </summary>
        [ExpandPath("DomainType.TargetSystem")]
        public virtual TargetSystem TargetSystem
        {
            get { return this.DomainType.Maybe(v => v.TargetSystem); }
        }


        //#IADFRAME-243
        //[Required, RestrictionExtension(typeof(RequiredAttribute), OperationContext = ConfigurationOperationContextC.PreSave)]
        public virtual DomainType DomainType
        {
            get { return this.domainType; }
            set { this.domainType = value; }
        }

        /// <summary>
        /// Признак третьего параметра "BLL Context" в сигнатуре лямбды
        /// </summary>
        /// <remarks>
        /// Признак "With Context" - БЛЛ контекст, из которого можно вызвать любой интерфейсный метод БЛЛ
        /// В большинстве случаях лямбды имеют 2-ва входных параметра (предыдущее и текущее состояния), заданного доменного типа
        /// </remarks>
        public virtual bool WithContext
        {
            get { return this.withContext; }
            set { this.withContext = value; }
        }

        /// <summary>
        /// Значение лямбды
        /// </summary>
        [Required, RestrictionExtension(typeof(RequiredAttribute), OperationContext = ConfigurationOperationContextC.PreSave)]
        [MaxLength]
        public virtual string Value
        {
            get { return this.value.TrimNull(); }
            set { this.value = value.TrimNull(); }
        }

        /// <summary>
        /// Тип лямбды
        /// </summary>
        public virtual SubscriptionLambdaType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// ID доменного типа авторизации для типизированного контекста
        /// </summary>
        [AuthDomainTypeRequiredValidator(OperationContext = ConfigurationOperationContextC.PreSave)]
        public virtual Guid AuthDomainTypeId
        {
            get { return this.authDomainTypeId; }
            set { this.authDomainTypeId = value; }
        }

        /// <summary>
        /// Признак обязательности предыдущего состояния элемента в лямбде
        /// </summary>
        /// <remarks>
        /// Чтобы не задавать условие на существование состояния типа prev == null && cur != null, можно указать Required Mode
        /// Отображение на интерфейсе:
        /// Если признак "True", то предыдущее значение обязательно (not null)
        /// Если признак "False", то предыдущее значение должно быть пустым (null)
        /// Если признак "Null", то допустим любой из предыдущих вариантов (uknown)
        /// </remarks>
        public virtual bool? RequiredModePrev
        {
            get { return this.requiredModePrev; }
            set { this.requiredModePrev = value; }
        }

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
        public virtual bool? RequiredModeNext
        {
            get { return this.requiredModeNext; }
            set { this.requiredModeNext = value; }
        }


        /// <summary>
        /// Момент выполнения лямбды
        /// </summary>
        /// <remarks>
        /// Отображение на интерфейсе:
        /// Если предыдущее not null, а текущее null - удаление объекта
        /// Если предыдущее null, а текущее not null - создание объекта
        /// Если предыдущее и текущее not null, то - изменение объекта
        /// </remarks>
        [Required, RestrictionExtension(typeof(RequiredAttribute), CustomError = "Invalid RequiredModePrev/RequiredModeNext combination")]
        public virtual SubscriptionType ProcessType
        {
            get { return SubscriptionTypeHelper.GetSubscriptionType(this.RequiredModePrev, this.RequiredModeNext); }
        }

        /// <summary>
        /// Строковое представление типа лямбды
        /// </summary>
        /// <remarks>
        /// Formatted Type вычисляется из доменного типа и показывает, инстансы каких типов мы можем указывать в «Value»
        /// </remarks>
        [MaxLength]
        public virtual string FormattedType
        {
            get { return this.formattedType; }
            internal protected set { this.formattedType = value; }
        }


        /// <summary>Получает делегат, исполняющий лямбду.</summary>
        /// <value>Делегат, исполняющий лямбду.</value>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [PropertyValidationMode(false)]
        public virtual Func<object, object, object> FuncValue
        {
            get { return this.funcValue; }
            set { this.funcValue = value; }
        }

        [CustomSerialization(CustomSerializationMode.Ignore)]
        [PropertyValidationMode(false)]
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
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [PropertyValidationMode(false)]
        public virtual Type MetadataSourceType
        {
            get { return this.metadataSourceType; }
            set { this.metadataSourceType = value; }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(this.Name)
                ? this.Name
                : this.MetadataSourceType?.ToString() ?? base.ToString();
        }
    }

    public static class SubscriptionLambdaExtensions
    {

        /// <summary>
        /// Предварительная проверка лямбды без проверки ее условия
        /// </summary>
        /// <remarks>
        /// Сверка предыдущего и текущего значений SubscriptionLambda и локационных настроек (Required Mode Prev, Required Mode Next)
        /// </remarks>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="subscriptionLambda">Лямбда подписки</param>
        /// <param name="prev">Предыдущее значение</param>
        /// <param name="next">Текущее значение</param>
        /// <returns>Возвращает "true" или "false"</returns>
        public static bool IsProcessed<T>(this SubscriptionLambda subscriptionLambda, T prev, T next)
           where T : class
        {

            if (subscriptionLambda == null) throw new ArgumentNullException(nameof(subscriptionLambda));

            var isCreate = prev == null && next != null;
            var isUpdate = prev != null && next != null;
            var isDelete = prev != null && next == null;

            if (subscriptionLambda.ProcessType == SubscriptionType.All)
            {
                return true;
            }

            if (subscriptionLambda.ProcessType == SubscriptionType.Create)
            {
                return isCreate;
            }

            if (subscriptionLambda.ProcessType == SubscriptionType.Continue)
            {
                return isUpdate;
            }

            if (subscriptionLambda.ProcessType == SubscriptionType.Remove)
            {
                return isDelete;
            }

            if (subscriptionLambda.ProcessType == SubscriptionType.CreateOrContinue)
            {
                return isCreate || isUpdate;
            }

            if (subscriptionLambda.ProcessType == SubscriptionType.CreateOrContinue)
            {
                return isUpdate || isDelete;
            }

            return false;
        }
    }
}
