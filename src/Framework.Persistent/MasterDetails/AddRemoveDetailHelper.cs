using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Persistent;

public static class AddRemoveDetailHelper
{
    public static void AddDetail<TMaster, TChild>(this TMaster master, TChild child)
            where TMaster : class, IMaster<TChild>
            where TChild : class, IDetail<TMaster>
    {
        if (master == null)
        {
            throw new ArgumentNullException(nameof(master));
        }

        if (child == null)
        {
            throw new ArgumentNullException(nameof(child));
        }

        if (!EqualityComparer<TMaster>.Default.Equals(child.Master, master))
        {
            throw new System.ArgumentException($"child ({typeof(TChild).Name}) has other master ({typeof(TMaster).Name})", nameof(child));
        }

        if (master.Details.Contains(child))
        {
            throw new System.ArgumentException($"master ({typeof(TMaster).Name}) collection already contains child ({typeof(TChild).Name})", nameof(child));
        }

        master.Details.Add(child);
    }

    public static void RemoveDetail<TMaster, TChild>(this TMaster master, TChild child)
            where TMaster : class, IMaster<TChild>
            where TChild : class, IDetail<TMaster>
    {
        if (master == null)
        {
            throw new ArgumentNullException(nameof(master));
        }

        if (child == null)
        {
            throw new ArgumentNullException(nameof(child));
        }

        if (!EqualityComparer<TMaster>.Default.Equals(child.Master, master))
        {
            throw new System.ArgumentException($"child ({typeof(TChild).Name}) has other master ({typeof(TMaster).Name})", nameof(child));
        }

        if (!master.Details.Remove(child))
        {
            throw new System.ArgumentException($"master ({typeof(TMaster).Name}) collection don't contains child ({typeof(TChild).Name})", nameof(child));
        }
    }

    public static void RemoveDetails<TMaster, TChild>(this TMaster master, IEnumerable<TChild> childs)
            where TMaster : class, IMaster<TChild>
            where TChild : class, IDetail<TMaster>
    {
        childs.Foreach(master.RemoveDetail);
    }

    /// <summary>
    /// Удаление детали без валидации мастера
    /// </summary>
    /// <typeparam name="TMaster"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    /// <param name="master"></param>
    /// <param name="child"></param>
    public static void RemoveDetailExt<TMaster, TChild>(this TMaster master, TChild child)
            where TMaster : class, IMaster<TChild>
            where TChild : class, IDetail<TMaster>
    {
        if (master == null)
        {
            throw new ArgumentNullException(nameof(master));
        }

        if (child == null)
        {
            throw new ArgumentNullException(nameof(child));
        }

        if (!master.Details.Remove(child))
        {
            throw new System.ArgumentException($"master ({typeof(TMaster).Name}) collection don't contains child ({typeof(TChild).Name})", nameof(child));
        }
    }

    public static void ClearDetails<TMaster, TChild>(this TMaster master)
            where TMaster : class, IMaster<TChild>
            where TChild : class, IDetail<TMaster>
    {
        if (master == null)
        {
            throw new ArgumentNullException(nameof(master));
        }

        master.Details.ToList().Foreach(master.RemoveDetail);
    }
}
