using System;
using System.Collections.Generic;

using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public partial interface IRegularJobBLL
    {
        void Pulse();

        /// <summary>
        /// Запрос RegularJob-ов ожидающих запуска (Job-ы переводятся в статус Processing)
        /// </summary>
        /// <returns></returns>
        List<RunRegularJobModel> GetPulseJobs();

        /// <summary>
        /// Пост-выполнение job-а
        /// </summary>
        /// <param name="regularJobId">Идент job-а</param>
        /// <param name="sync">Job запущен в синхронном режиме, Procssing-State не проверяется</param>
        void PreExecute(Guid regularJobId, bool sync);

        void PostExecute(Guid regularJobId, ExecuteRegularJobResult executingResult, RunRegularJobMode mode);

        void ValidateState(RegularJob job, RegularJobState expectedState);

        IList<RegularJobRevisionModel> GetRegularJobRevisionModelsBy(Framework.Configuration.Domain.Models.Filters.RegularJobRevisionFilterModel filter);

        /// <summary>
        /// Все доступные Job-ы запускаются синхронно на сервере (состояние SendToProcessing пропускается)
        /// </summary>
        /// <param name="runRegularJobMode">Режим запуска job-ов</param>
        void SyncRunAll(RunRegularJobMode runRegularJobMode = RunRegularJobMode.RecalculateNextStartTime);

        /// <summary>
        /// Job-ы запускаются синхронно на сервере (состояние SendToProcessing пропускается)
        /// </summary>
        /// <param name="jobs">Список job-ов</param>
        /// <param name="runRegularJobMode">Режим запуска job-ов</param>
        void SyncRun(
            IEnumerable<RegularJob> jobs,
            RunRegularJobMode runRegularJobMode = RunRegularJobMode.RecalculateNextStartTime);

        /// <summary>
        /// Обработка job-а в отдельной сессии
        /// </summary>
        /// <param name="regularJob">job</param>
        /// <param name="runRegularJobMode">Режим запуска job-а</param>
        /// <param name="sync">Job запущен в синхронном режиме, Procssing-State не проверяется</param>
        void RunRegularJobInNewSession(RegularJob regularJob, RunRegularJobMode runRegularJobMode, bool sync);
    }
}
