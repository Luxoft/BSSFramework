using System;

namespace Framework.HierarchicalExpand
{
    /// <summary>
    /// Интерфейс для получнения реального разворачиваемого типа
    /// </summary>
    public interface IHierarchicalRealTypeResolver
    {
        Type Resolve(Type identity);
    }
