using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Framework.Configuration.Domain;
using Framework.Configuration.Domain.Models.Filters;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;
using Framework.Validation;

using Serilog;

namespace Framework.Configuration.BLL
{
    /// <summary>
    /// TODO: common queue with info for task, separate thread for listen queue and run task execution
    /// , main thread set statuses for task and return...
    /// </summary>
    public partial class RegularJobBLL
    {
        public override void Save(RegularJob regularJob)
        {
            base.Save(regularJob); // TODO: Добавить валидацию regularJob.Function #IADFRAME-1490
        }

        public List<RunRegularJobModel> GetPulseJobs()
        {
            Log.Verbose("Set Lock 'RegularJobLock' for update");
            this.Context.Logics.NamedLock.Lock(NamedLockOperation.RegularJobLock, LockRole.Update);

            var allJobs = this.GetUnsecureQueryable().Where(z => (z.State == RegularJobState.WaitPulse) && z.Active).ToList();

            Log.Verbose("Found {Count} jobs", allJobs.Count);

            var needRunningJobs = this.GetWaitingPulseRunningJobs(allJobs);

            Log.Verbose("Found {Count} jobs for run", needRunningJobs.Count);

            this.SetPulseTime(allJobs);

            needRunningJobs.Foreach(z => z.State = RegularJobState.SendToProcessing);

            this.Save(allJobs);

            return needRunningJobs.ToList(job => new RunRegularJobModel(job, RunRegularJobMode.RecalculateNextStartTime, Environment.MachineName));
        }

        /// <inheritdoc />
        public void SyncRunAll(RunRegularJobMode runRegularJobMode = RunRegularJobMode.RecalculateNextStartTime)
        {
            var allJobs = this.GetUnsecureQueryable().Where(z => (z.State == RegularJobState.WaitPulse) && z.Active).ToList();

            this.SyncRun(allJobs, runRegularJobMode);
        }

        /// <inheritdoc />
        public void SyncRun(
            IEnumerable<RegularJob> jobs,
            RunRegularJobMode runRegularJobMode = RunRegularJobMode.RecalculateNextStartTime)
        {
            var jobList = jobs.ToList();

            if (jobList.Any(z => z.State != RegularJobState.WaitPulse))
            {
                var noWaitJobs = jobList.Where(z => z.State != RegularJobState.WaitPulse)
                                        .Select(z => $"{z.Name} - {z.State}").Join(",");

                throw new ValidationException($"Can't start jobs. There are incorrect state:'{noWaitJobs}'");
            }

            foreach (var regularJob in jobList)
            {
                this.RunRegularJobInNewSession(regularJob, runRegularJobMode, true);
            }
        }

        /// <inheritdoc />
        public void RunRegularJobInNewSession(RegularJob regularJob, RunRegularJobMode runRegularJobMode, bool sync)
        {
            Log.Verbose("Run regular job {Name}", regularJob.Name);

            var executingResult = ExecuteRegularJobResult.CreateSuccessed(this.Context.DateTimeService.Now, TimeSpan.FromSeconds(0));

            var timer = Stopwatch.StartNew();

            try
            {
                this.EvaluateNewSession(innerContext => innerContext.Logics.RegularJob.PreExecute(regularJob.Id, sync));

                this.Execute(regularJob);

                timer.Stop();

                executingResult = ExecuteRegularJobResult.CreateSuccessed(this.Context.DateTimeService.Now, timer.Elapsed);
            }
            catch (Exception baseEx)
            {
                timer.Stop();

                Exception lastEx;

                if (baseEx is TargetInvocationException exception)
                {
                    lastEx = exception.GetLastInnerException();
                }
                else
                {
                    lastEx = baseEx;
                }

                Log.Error(lastEx, "Exception in regular job processing");

                executingResult = ExecuteRegularJobResult.CreateFail(this.Context.DateTimeService.Now, timer.Elapsed, lastEx.Message);
            }
            finally
            {
                this.EvaluateNewSession(innerContext => innerContext.Logics.RegularJob.PostExecute(regularJob.Id, executingResult, runRegularJobMode));
            }
        }

        public IList<RegularJobRevisionModel> GetRegularJobRevisionModelsBy(RegularJobRevisionFilterModel filter)
        {
            var regularJob = filter.RegularJob;
            var countingEntities = filter.CountingEntities;

            var regularJobs = this.GetDomainObjectRevisions(regularJob.Id, countingEntities)
                .Select(z => new RegularJobRevisionModel(z.Item1, z.Item2))
                .ToList();

            return regularJobs;
        }

        public void Pulse()
        {
            this.Context.Logics.NamedLock.Lock(NamedLockOperation.RegularJobLock, LockRole.Update);

            var allJobs = this.GetUnsecureQueryable().Where(z => z.State == RegularJobState.WaitPulse && z.Active).ToList();

            Log.Verbose("Found {Count} jobs", allJobs.Count);

            var needRunningJobs = this.GetWaitingPulseRunningJobs(allJobs);

            Log.Verbose("Found {Count} jobs for run", needRunningJobs.Count);

            this.RunRegularJobs(allJobs, needRunningJobs, RunRegularJobMode.RecalculateNextStartTime);
        }

        public void PreExecute(Guid regularJobId, bool sync)
        {
            var regularJob = this.GetById(regularJobId, true);

            this.Lock(regularJob, LockRole.Update);

            var executingResult = ExecuteRegularJobResult.CreateSuccessed(this.Context.DateTimeService.Now, TimeSpan.FromSeconds(0));

            if (regularJob.State == (sync ? RegularJobState.WaitPulse : RegularJobState.SendToProcessing))
            {
                regularJob.State = RegularJobState.Running;
            }
            else
            {
                Log.Error("Incorrect for running status. Expected:'{ExpectedStatus}'. Actual:'{ActualStatus}'", RegularJobState.SendToProcessing, regularJob.State);

                executingResult = ExecuteRegularJobResult.CreateFail(this.Context.DateTimeService.Now, TimeSpan.FromSeconds(0), "Incorrect for running status. Expected:'{0}'. Actual:'{1}'", RegularJobState.SendToProcessing, regularJob.State);
            }

            regularJob.ExecutionResult = executingResult;

            this.Save(regularJob);
        }

        public void PostExecute(Guid regularJobId, ExecuteRegularJobResult executingResult, RunRegularJobMode mode)
        {
            var regularJob = this.GetById(regularJobId, true);

            this.Lock(regularJob, LockRole.Update);

            regularJob.ExecutionResult = executingResult;

            regularJob.State = RegularJobState.WaitPulse;

            this.UpdateNextStartTime(regularJob, mode);

            this.Save(regularJob);
        }

        public void ValidateState(RegularJob domain, RegularJobState expectedState)
        {
            if (domain.State != expectedState)
            {
                throw new BusinessLogicException("Incorrect for running status. Expected:'{0}'. Actual:'{1}'", expectedState, domain.State);
            }
        }

        private void EvaluateNewSession(Action<IConfigurationBLLContext> action)
        {
            this.Context.RootContextEvaluator.Evaluate(DBSessionMode.Write, this.Context.Authorization.CurrentPrincipalName, action);
        }

        private void Execute(RegularJob regularJob)
        {
            if (regularJob.WrapUpSession)
            {
                this.EvaluateNewSession(innerContext => innerContext.GetMainTargetSystemService().ExecuteBLLContextLambda(regularJob));
            }
            else
            {
                throw new NotImplementedException("Delete");
            }
        }

        private void ExecuteEnvironmentLambda<TServiceEnvironment>(
            TServiceEnvironment environment,
            string value)
        {
            throw new NotImplementedException("Delete");
        }

        private void RunRegularJobs(IList<RegularJob> allJobs, IList<RegularJob> needRunningJobs, RunRegularJobMode runRegularJobMode)
        {
            this.SetPulseTime(allJobs);

            needRunningJobs.Foreach(z => z.State = RegularJobState.SendToProcessing);

            this.AsyncSendToProcess(needRunningJobs, runRegularJobMode);

            allJobs.Foreach(this.Save);
        }

        private void AsyncSendToProcess(IList<RegularJob> jobs, RunRegularJobMode runRegularJobMode)
        {
            jobs.Foreach(z => this.Context.RegularJobMessageSender.Send(new RunRegularJobModel(z, runRegularJobMode, Environment.MachineName), TransactionMessageMode.Auto));
        }

        private void SetPulseTime(IEnumerable<RegularJob> allJobs)
        {
            allJobs.Foreach(z => z.LastPulseTime = this.Context.DateTimeService.Now);
        }

        private void UpdateNextStartTime(RegularJob job, RunRegularJobMode runRegularJobMode)
        {
            switch (runRegularJobMode)
            {
                case RunRegularJobMode.RecalculateNextStartTime:
                    job.ExpectedNextStartTime = job.ExecutionResult.Status == RegularJobStatus.Successed
                                                    ? this.GetExpectedNextStartTime(job)
                                                    : job.ExpectedNextStartTime;
                    break;

                case RunRegularJobMode.Silent:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(runRegularJobMode.ToString());
            }
        }

        private DateTime GetExpectedNextStartTime(RegularJob job)
        {
            var schedulerTime = job.ShedulerTime;

            Func<DateTime, DateTime> getNextTimeFunc;

            switch (schedulerTime.ValueType)
            {
                case SheduleValueType.Year:
                    getNextTimeFunc = z => z.AddYears(schedulerTime.Value);
                    break;
                case SheduleValueType.Month:
                    getNextTimeFunc = z => z.AddMonths(schedulerTime.Value);
                    break;
                case SheduleValueType.Week:
                    getNextTimeFunc = z => z.AddDays(schedulerTime.Value * 7);
                    break;
                case SheduleValueType.Day:
                    getNextTimeFunc = z => z.AddDays(schedulerTime.Value);
                    break;
                case SheduleValueType.Minutes:
                    getNextTimeFunc = z => z.AddMinutes(schedulerTime.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(schedulerTime.ValueType.ToString());
            }

            return getNextTimeFunc(job.ExpectedNextStartTime);
        }

        private List<RegularJob> GetWaitingPulseRunningJobs(IEnumerable<RegularJob> allJobs)
        {
            var currentTicks = this.Context.DateTimeService.Now;

            var needRunningTasks = allJobs
                .Where(z => currentTicks > z.ExpectedNextStartTime)
                .Where(z => z.State == RegularJobState.WaitPulse)
                .ToList();

            return needRunningTasks;
        }
    }
}
