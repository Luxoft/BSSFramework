import { Pipe, PipeTransform } from '@angular/core';
import { IEntity } from '../../../view-principal-dialog/view-principal-dialog.component';

@Pipe({
  name: 'contextCheck',
  standalone: true,
})
export class ContextCheckPipe implements PipeTransform {
  transform(entyty: IEntity, list: IEntity[]): boolean {
    return Boolean(list.find((item) => item.Id === entyty.Id));
  }
}
