using System;

using Framework.Core;

namespace Framework.Events
{
    /// <summary>
    ///
    /// </summary>
    public struct TypeEvent
    {
        public static TypeEvent SaveAndRemove<T>(Func<T, bool> isSaveProcessingFunc = null, Func<T, bool> isRemoveProcessingFunc = null)
        {
            return TypeEvent.Create(EventOperation.Save | EventOperation.Remove, isSaveProcessingFunc, isRemoveProcessingFunc);
        }

        public static TypeEvent Save<T>(Func<T, bool> isSaveFunc = null)
        {
            return TypeEvent.Create(EventOperation.Save, isSaveFunc);
        }

        public static TypeEvent Create<T>(EventOperation eventOperation, Func<T, bool> isSaveProcessingFunc = null, Func<T, bool> isRemoveProcessingFunc = null)
        {
            Func<object, bool> defaultFunc = z => true;
            var isSaveUntypedFunc = defaultFunc;
            var isRemoveUntypeFunc = defaultFunc;

            if (isSaveProcessingFunc != null)
            {
                isSaveUntypedFunc = z => isSaveProcessingFunc((T)z);
            }

            if (null != isRemoveProcessingFunc)
            {
                isRemoveUntypeFunc = z => isRemoveProcessingFunc((T)z);
            }

            return new TypeEvent(typeof(T), eventOperation, isSaveUntypedFunc, isRemoveUntypeFunc);
        }


        public TypeEvent(Type type, EventOperation operation, Func<object, bool> isSaveProcessingFunc, Func<object, bool> isRemoveProcessingFunc) : this()
        {
            this.Type = type;

            this.Operation = operation;

            this.IsSaveProcessingFunc = isSaveProcessingFunc;

            this.IsRemoveProcessingFunc = isRemoveProcessingFunc;
        }

        /// <summary>
        ///
        /// </summary>
        public Type Type { get; private set; }
        /// <summary>
        ///
        /// </summary>
        public EventOperation Operation { get; private set; }

        public Func<object, bool> IsSaveProcessingFunc { get; private set; }

        public Func<object, bool> IsRemoveProcessingFunc { get; private set; }
    }
}