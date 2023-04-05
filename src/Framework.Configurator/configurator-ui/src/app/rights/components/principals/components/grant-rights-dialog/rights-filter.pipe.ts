import { Pipe, PipeTransform } from '@angular/core';
import { IPermission } from '../view-principal-dialog/view-principal-dialog.component';

@Pipe({
  name: 'rightsFilter',
  pure: false,
  standalone: true,
})
export class RightsFilterPipe implements PipeTransform {
  transform(
    permission: IPermission[] | undefined,
    filter: { contextId: string; search: string }[] | null,
    roleFilter: string | null
  ): IPermission[] {
    const newPermission = [];
    if (filter && filter.length > 0) {
      newPermission.push(
        ...(permission || []).filter(
          (permissions) =>
            permissions.Contexts.filter(
              (x) =>
                x.Entities.filter((entity) => {
                  const search = filter.find((f) => f.contextId === x.Id);
                  return search ? entity.Name.toLocaleLowerCase().includes(search.search.toLocaleLowerCase()) : true;
                }).length && filter.find((c) => c.contextId === x.Id)
            ).length
        )
      );
    } else {
      newPermission.push(...(permission || []));
    }

    return newPermission.filter((c) => c.Role.toLocaleLowerCase().includes((roleFilter || '').toLocaleLowerCase()));
  }
}
