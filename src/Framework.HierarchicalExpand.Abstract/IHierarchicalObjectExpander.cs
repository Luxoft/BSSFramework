using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Framework.HierarchicalExpand
{
    public interface IHierarchicalObjectExpander<TIdent>
    {
        /// <summary>
        /// Получение полного списка связанных идентефикаторов
        /// </summary>
        /// <param name="idents">Список базовых идентефикаторов</param>
        /// <param name="expandType">Тип разворачивания</param>
        /// <returns>HashSet/IQueryable></returns>
        IEnumerable<TIdent> Expand([NotNull] IEnumerable<TIdent> idents, HierarchicalExpandType expandType);

        /// <summary>
        /// Получение полного списка связанных идентефикаторов вместе с родителем
        /// </summary>
        /// <param name="idents">Список базовых идентефикаторов</param>
        /// <param name="expandType">Тип разворачивания</param>
        /// <returns></returns>
        Dictionary<TIdent, TIdent> ExpandWithParents([NotNull] IEnumerable<TIdent> idents, HierarchicalExpandType expandType);

        /// <summary>
        /// Получение полного списка связанных идентефикаторов вместе с родителем
        /// </summary>
        /// <param name="idents">Список базовых идентефикаторов</param>
        /// <param name="expandType">Тип разворачивания</param>
        /// <returns></returns>
        Dictionary<TIdent, TIdent> ExpandWithParents([NotNull] IQueryable<TIdent> idents, HierarchicalExpandType expandType);
    }
}
