using System;

using Framework.Core;
using Framework.Restriction;

namespace Framework.Configuration.Domain
{

    /// <summary>
    /// Результат выполнения регулярной задачи
    /// </summary>
    public class ExecuteRegularJobResult : DomainObjectBase
    {

        /// <summary>
        /// Создает результат успешной работы регулярной задачи
        /// </summary>
        /// <param name="startedTime">Дата начала работы регулярной задачи</param>
        /// <param name="executionTime">Время выполнения регулярной задачи</param>
        /// <returns>Результат работы regular job</returns>
        public static ExecuteRegularJobResult CreateSuccessed(DateTime startedTime, TimeSpan executionTime)
        {
            return new ExecuteRegularJobResult()
                       {
                           Status = RegularJobStatus.Successed,
                           Time = startedTime.Ticks,
                           ExecutionTime = executionTime.Ticks,
                           Description = string.Empty
                       };
        }

        /// <summary>
        /// Создает результат инициируемой работы регулярной задачи
        /// </summary>
        /// <returns>Результат работы regular job</returns>
        public static ExecuteRegularJobResult CreateInitial()
        {
            return new ExecuteRegularJobResult()
            {
                Status = RegularJobStatus.Initial,
                Time = null,
                ExecutionTime = null,
                Description = string.Empty
            };
        }

        /// <summary>
        /// Создает результат инициируемой работы регулярной задачи
        /// </summary>
        /// <param name="startedTime">Дата начала работы регулярной задачи</param>
        /// <param name="executionTime">Время выполнения регулярной задачи</param>
        /// <param name="format">Формат строки с ошибкой</param>
        /// <param name="args">Аргумент</param>
        /// <returns>Результат работы регулярной задачи</returns>
        public static ExecuteRegularJobResult CreateFail(DateTime startedTime, TimeSpan executionTime, string format, params object[] args)
        {
            return CreateFail(startedTime, executionTime, string.Format(format, args));
        }

        /// <summary>
        /// Создает результат ошибки работы регулярной задачи
        /// </summary>
        /// <param name="startedTime">Дата начала работы регулярной задачи</param>
        /// <param name="executionTime">Время выполнения регулярной задачи</param>
        /// <param name="error">Строка ошибки</param>
        /// <returns></returns>
        public static ExecuteRegularJobResult CreateFail(DateTime startedTime, TimeSpan executionTime, string error)
        {
            return new ExecuteRegularJobResult()
                       {
                           Status = RegularJobStatus.Fail,
                           Description = error,
                           Time = startedTime.Ticks,
                           ExecutionTime = executionTime.Ticks
                       };
        }


        private RegularJobStatus status;
        private string description;
        private long? time;
        private long? executionTime;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ExecuteRegularJobResult()
        {

        }

        /// <summary>
        /// Статус регулярной задачи
        /// </summary>
        public virtual RegularJobStatus Status
        {
            get { return this.status; }
            protected internal set { this.status = value; }
        }

        /// <summary>
        /// Описание результата выполнения регулярной задачи
        /// </summary>
        [MaxLength]
        public virtual string Description
        {
            get { return this.description.TrimNull(); }
            protected internal set { this.description = value.TrimNull(); }
        }

        /// <summary>
        /// Дата начала работы регулярной задачи
        /// </summary>
        public long? Time
        {
            get { return this.time; }
            protected internal set { this.time = value; }
        }

        /// <summary>
        /// Время выполнения регулярной задачи
        /// </summary>
        public virtual long? ExecutionTime
        {
            get { return this.executionTime; }
            protected internal set { this.executionTime = value; }
        }
    }
}