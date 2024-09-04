import { Pipe, PipeTransform } from '@angular/core';
import { IPermission } from '../principal.models';

@Pipe({
  name: 'rightsFilter',
  standalone: true,
})
export class RightsFilterPipe implements PipeTransform {
  transform(
    filter: {
      contexts?: { contextId: string; search: string }[];
      role?: string;
      comment?: string;
    } | null,
    permission: IPermission[] | undefined
  ): IPermission[] {
    const newPermission = [];
    if (filter?.contexts && filter.contexts.length > 0) {
      newPermission.push(
        ...(permission || [])
          .filter(
            (permissions) =>
              permissions.Contexts.filter(
                (x) =>
                  x.Entities.filter((entity) => {
                    const search = filter.contexts?.find((f) => f.contextId === x.Id);
                    return search ? entity.Name.toLocaleLowerCase().includes(search.search.toLocaleLowerCase()) : true;
                  }).length && filter.contexts?.find((c) => c.contextId === x.Id)
              ).length
          )
          .map((permission) => ({
            ...permission,
            Contexts: permission.Contexts.map((context) => ({
              ...context,
              Entities: [
                ...context.Entities.filter((x) =>
                  x.Name.toLocaleLowerCase().includes(
                    filter.contexts?.find((co) => co.contextId === context.Id)?.search.toLocaleLowerCase() || ''
                  )
                ),
                ...context.Entities.filter(
                  (x) =>
                    !x.Name.toLocaleLowerCase().includes(
                      filter.contexts?.find((co) => co.contextId === context.Id)?.search.toLocaleLowerCase() || ''
                    )
                ),
              ],
            })),
          }))
      );
    } else {
      newPermission.push(...(permission || []));
    }

    return newPermission.filter(
      (c) =>
        c.Role.toLocaleLowerCase().includes((filter?.role || '').toLocaleLowerCase()) &&
        (c.Comment || '').toLocaleLowerCase().includes((filter?.comment || '').toLocaleLowerCase())
    );
  }
}
