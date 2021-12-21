using System.Runtime.Serialization;

using Framework.Validation;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Расписание запуска регулярной задачи
    /// </summary>
    [DataContract(Namespace = "")]
    public class ShedulerTime
    {

        /// <summary>
        /// Создание расписания для месяцев
        /// </summary>
        /// <param name="value">Количество месяцев</param>
        /// <returns>Периодичность</returns>
        public static ShedulerTime FromMonths(int value)
        {
            return new ShedulerTime(value, SheduleValueType.Month);
        }

        /// <summary>
        /// Создание расписания для недель
        /// </summary>
        /// <param name="value">Количество недель</param>
        /// <returns>Периодичность</returns>
        public static ShedulerTime FromWeeks(int value)
        {
            return new ShedulerTime(value, SheduleValueType.Week );
        }

        /// <summary>
        /// Создание расписания для дней
        /// </summary>
        /// <param name="value">Количество дней</param>
        /// <returns>Периодичность</returns>
        public static ShedulerTime FromDays(int value)
        {
            return new ShedulerTime(value, SheduleValueType.Day );
        }

        /// <summary>
        /// Создание расписания для минут
        /// </summary>
        /// <param name="value">Количество минут</param>
        /// <returns>Периодичность</returns>
        public static ShedulerTime FromMinutes(int value)
        {
            return new ShedulerTime(value, SheduleValueType.Minutes );
        }


        private int value;
        private SheduleValueType valueType;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ShedulerTime()
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="value">Значение расписания</param>
        /// <param name="valueType">Тип значения расписания</param>
        public ShedulerTime(int value, SheduleValueType valueType)
        {
            this.value = value;
            this.valueType = valueType;
        }

        /// <summary>
        /// Значение расписания
        /// </summary>
        [SignValidator(SignType.Positive)]
        [DataMember]
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Тип значения расписания
        /// </summary>
        [DataMember]
        public SheduleValueType ValueType
        {
            get { return this.valueType; }
            set { this.valueType = value; }
        }
    }
}