import { Pipe, PipeTransform } from '@angular/core';
import { IContextWithRoleRestrictions, IPermission } from '../principal.models';
import { IRoleContext } from './grant-rights-dialog.models';

@Pipe({
  name: 'contextFilter',
  standalone: true,
})
export class ContextFilterPipe implements PipeTransform {
  transform(permission: IPermission, context: IRoleContext): IContextWithRoleRestrictions | undefined {
    return permission.Contexts.find((x) => x.Id === context.Id);
  }
}
