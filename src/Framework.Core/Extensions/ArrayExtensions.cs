using System;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class ArrayExtensions
    {
        public static T[] CloneA<T>(this T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return (T[])array.Clone();
        }

        public static bool AnyA(this Array array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return array.Length != 0;
        }

        public static T[] GetSubArray<T>(this T[] array, int startIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return array.GetSubArray(startIndex, array.Length - startIndex);
        }

        public static T[] GetSubArray<T>(this T[] array, int startIndex, int length)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            var subArray = new T[length];

            array.CopyTo(subArray, startIndex);

            return subArray;
        }

        public static T LastA<T>(this T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return array.AnyA() ? array[array.Length - 1] : array.Last();
        }

        public static T FirstA<T>(this T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return array.AnyA() ? array[0] : array.First();
        }


        public static Type GetElementType(this Array array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return array.GetType().GetElementType();
        }


        public static MemoryStream ToMemoryStream([NotNull] this byte[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new MemoryStream(source);
        }

        public static string ToBase64String([NotNull] this byte[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return Convert.ToBase64String(source);
        }
    }
}
