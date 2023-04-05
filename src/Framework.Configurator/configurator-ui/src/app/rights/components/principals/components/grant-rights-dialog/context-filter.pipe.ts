import { Pipe, PipeTransform } from '@angular/core';
import { IEntity, IPermission } from '../view-principal-dialog/view-principal-dialog.component';
import { IRoleContext } from './grant-rights-dialog.models';

@Pipe({
  name: 'contextFilter',
  standalone: true,
})
export class ContextFilterPipe implements PipeTransform {
  transform(permission: IPermission, context: IRoleContext): IEntity[] {
    return permission.Contexts.find((x) => x.Id === context.Id)?.Entities ?? [];
  }
}
