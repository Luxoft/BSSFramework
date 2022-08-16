using System;
using Framework.Core;
using Framework.DomainDriven;

namespace Automation.ServiceEnvironment.Services
{
    /// <summary>
    /// Реализация интерфейса <see cref="IDateTimeService"/> для интеграционных тестов
    /// </summary>
    public class TestDateTimeService : IDateTimeService
    {
        private Func<DateTime> GetNow;

        public TestDateTimeService() => this.GetNow = () => DateTime.Now;

        public DateTime Now => this.GetNow();

        public Period CurrentFinancialYear => this.Now.ToFinancialYearPeriod();

        public Period CurrentMonth => this.Now.ToMonth();

        public Period CurrentYear => this.Now.ToYear();

        public Period NextMonth => this.Now.AddMonths(1).ToMonth();

        public Period PrevMonth => this.Now.AddMonths(-1).ToMonth();

        public DateTime Today => this.Now.Date;

        public DateTime UtcNow => this.Now.ToUniversalTime();

        public void SetCurrentDateTime(DateTime dateTime)
        {
            var dateTimeDelta = dateTime - DateTime.Now;
            this.GetNow = () => DateTime.Now + dateTimeDelta;
        }
    }
}