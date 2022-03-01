using System;
using Framework.Core;
using Framework.DomainDriven;

namespace WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests
{
    /// <summary>
    /// Реализация интерфейса <see cref="IDateTimeService"/> для интеграционных тестов
    /// </summary>
    public class IntegrationTestDateTimeService : IDateTimeService
    {
        /// <summary>
        /// Установка текущей даты для тестов
        /// </summary>
        /// <remarks>Устновка значения как null приведет к исключению при обращении</remarks>
        public static DateTime? CurrentDate { get; set; }

        /// <summary>
        /// Return Now value
        /// </summary>
        public DateTime Now => CurrentDateValue;

        /// <summary>
        /// Return Now value without time
        /// </summary>
        public DateTime Today => CurrentDateValue.Date;

        public Period CurrentFinancialYear => Period.GetFinancialYearPeriod(CurrentDateValue);

        public Period CurrentYear => Period.FromYear(CurrentDateValue.Year);

        public Period CurrentMonth => CurrentDateValue.ToMonth();

        public Period NextMonth => CurrentDateValue.AddMonths(1).ToMonth();

        public Period PrevMonth => CurrentDateValue.AddMonths(-1).ToMonth();

        public DateTime UtcNow => CurrentDateValue.ToUniversalTime();

        private static DateTime CurrentDateValue
        {
            get
            {
                if (CurrentDate == null)
                {
                    throw new InvalidOperationException("Setup CurrentDate before using test DateTimeService");
                }

                return CurrentDate.Value;
            }
        }
    }
}